using BlazorBootstrap;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Momentum.BackOffice.Extensions;
using Momentum.BackOffice.Services;

namespace Momentum.BackOffice.Features.Events;

[Route($"/{PageRoutes.Events}")]
public partial class EventPage : IDisposable
{
	#region Fields

	private List<EventViewModel> _model = [];
	private List<EventViewModel> _filteredModel = [];
	private string _searchText = string.Empty;
	private Modal? _addModal;
	private Modal? _deleteModal;
	private Modal? _infoModal;
	private Modal? _editEventModal;
	private Modal? _checkoutModal;
	private EventViewModel _selectedEvent = new();
	private CheckoutViewModel _checkout = new();
	private EditContext _checkoutContext = null!;
	private bool _isCheckoutValid;
	private bool _isProcessingPayment;
	private EventEntryViewModel _newEntry = new();
	private EventEntryViewModel? _editEventEntry;
	private List<LocationOptionViewModel> _availableLocationsForEdit = [];
	private List<ProductOptionViewModel> _availableProductsForEdit = [];
	private int _editLocationPeople = 1;
	private int _editLocationHours = 1;
	private bool _isValidForm;
	private EditContext _editContext = null!;
	private EditContext _editEventDetailsContext = null!;
	private bool _isEditMode;

	private CancellationTokenSource? _cts;

	#endregion // Fields

	#region Private Properties

	[Inject] private IEventService EventService { get; set; } = null!;
	[Inject] private IProductService ProductService { get; set; } = null!;
	[Inject] private IEventOrderService EventOrderService { get; set; } = null!;

	#endregion // Private Properties

	#region Private Methods

	private string GetEventTypeString(int type)
	{
		return type switch
		{
			1 => "Wedding",
			2 => "Birthday",
			3 => "Teambuilding",
			4 => "Festival",
			5 => "Other",
			6 => "Meeting",
			7 => "Party",
			8 => "Community",
			9 => "Conference",
			10 => "Gala",
			11 => "Product Launch",
			12 => "Baby Shower",
			_ => "Unknown"
		};
	}

	private string GetProductTypeString(int type)
	{
		return type switch
		{
			2 => "Food",
			3 => "Photography",
			4 => "Music",
			5 => "Videography",
			6 => "Decoration",
			7 => "Entertainment",
			8 => "Bar",
			9 => "Security",
			10 => "Cleaning",
			11 => "Logistics",
			12 => "Transport",
			13 => "Print",
			_ => "Service"
		};
	}

	private LocationOptionViewModel? GetEditSelectedLocation()
	{
		if (_editEventEntry?.SelectedLocationId is not { } id || id == Guid.Empty)
			return null;

		return _availableLocationsForEdit.FirstOrDefault(l => l.Id == id);
	}

	private void SelectEditLocation(Guid locationId)
	{
		if (_editEventEntry == null)
			return;

		if (_editEventEntry.SelectedLocationId != locationId)
		{
			_editLocationPeople = 1;
			_editLocationHours = 1;
		}

		_editEventEntry.SelectedLocationId = locationId;
	}

	private decimal GetEditLocationLineTotal()
	{
		var location = GetEditSelectedLocation();
		if (location == null)
			return 0m;

		return location.Price
			* (location.IsPerPerson ? Math.Max(1, _editLocationPeople) : 1)
			* (location.IsHourly ? Math.Max(1, _editLocationHours) : 1);
	}

	private static decimal GetProductLineTotal(ProductOptionViewModel product)
	{
		return product.Price
			* (product.IsPerPerson ? Math.Max(1, product.NumberOfPeople) : 1)
			* (product.IsHourly ? Math.Max(1, product.NumberOfHours) : 1);
	}

	private decimal GetEditModalTotalPrice()
	{
		if (_editEventEntry == null)
			return 0m;

		decimal total = GetEditLocationLineTotal();

		// Add selected products prices
		foreach (var productId in _editEventEntry.SelectedProductIds)
		{
			var product = _availableProductsForEdit.FirstOrDefault(p => p.Id == productId);
			if (product != null)
			{
				total += GetProductLineTotal(product);
			}
		}

		return total;
	}

	protected override async Task OnInitializedAsync()
	{
		_newEntry = new EventEntryViewModel();
		_editContext = new EditContext(_newEntry);
		_editContext.OnFieldChanged += FieldChange;

		await LoadEvents();
	}

	private void FieldChange(object? sender, FieldChangedEventArgs e)
	{
		// The add and edit modals use different EditContexts, so validate the one
		// that actually raised the change instead of always using the add context.
		if (sender is EditContext editContext)
		{
			_isValidForm = editContext.Validate();
		}
	}

	private async Task LoadEvents()
	{
		await SafeExecute(async () =>
		{
			var response = await EventService.GetEvents();
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			if (response.Content is not null)
			{
				_model = [];

				// Fetch all event orders to get location and products data
				var eventOrdersResponse = await EventOrderService.GetEventOrders();
				var eventOrders = eventOrdersResponse.Content ?? [];

				foreach (var eventData in response.Content)
				{
					var eventViewModel = new EventViewModel
					{
						Id = eventData.Id,
						Type = eventData.Type,
						Name = eventData.Name,
						Date = eventData.Date,
						SelectedLocationId = null,
						LocationName = null,
						LocationPrice = 0
					};

					// Find associated event order
					var eventOrder = eventOrders.FirstOrDefault(eo => eo.EventId == eventData.Id);
					if (eventOrder != null)
					{
						eventViewModel.EventOrderId = eventOrder.Id;
						eventViewModel.IsPaid = eventOrder.IsPaid;

						// Find location product (Type = 1)
						var locationProduct = eventOrder.Products?.FirstOrDefault(p => p.Product?.Type == 1);
						if (locationProduct != null && locationProduct.Product != null)
						{
							eventViewModel.SelectedLocationId = locationProduct.ProductId;
							eventViewModel.LocationName = locationProduct.Product.Name;
							eventViewModel.LocationPrice = locationProduct.UnitPrice;
							eventViewModel.LocationIsPerPerson = locationProduct.Product.IsPerPerson;
							eventViewModel.LocationIsHourly = locationProduct.Product.IsHourly;
							eventViewModel.LocationPeople = locationProduct.NumberOfPeople;
							eventViewModel.LocationHours = locationProduct.NumberOfHours;
						}

						// Get non-location products
						eventViewModel.SelectedProducts = eventOrder.Products?
							.Where(p => p.Product?.Type != 1)
							.Select(p => new EventProductDetailViewModel
							{
								ProductId = p.ProductId,
								ProductName = p.Product?.Name ?? "Unknown",
								Price = p.UnitPrice,
								IsPerPerson = p.Product?.IsPerPerson ?? false,
								IsHourly = p.Product?.IsHourly ?? false,
								NumberOfPeople = p.NumberOfPeople,
								NumberOfHours = p.NumberOfHours
							})
							.ToList() ?? [];

						eventViewModel.TotalPrice = eventOrder.TotalPrice;
					}

					_model.Add(eventViewModel);
				}

				FilterList();
			}
		});
	}

	private void OnSearchInput(ChangeEventArgs e)
	{
		_searchText = e.Value?.ToString()?.Trim() ?? string.Empty;

		_cts?.Cancel();
		_cts = new CancellationTokenSource();

		_ = Task.Delay(300, _cts.Token)
			.ContinueWith(t =>
			{
				if (!t.IsCanceled)
				{
					InvokeAsync(() =>
					{
						FilterList();
						StateHasChanged();
					});
				}
			});
	}

	private void FilterList()
	{
		if (string.IsNullOrWhiteSpace(_searchText))
		{
			_filteredModel = [.. _model];
		}
		else
		{
			_filteredModel = _model
				.Where(x => x.Name?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true)
				.ToList();
		}
	}

	private async Task SaveEvent()
	{
		if (!_editContext!.Validate())
		{
			return;
		}

		await SafeExecute(async () =>
		{
			if (_isEditMode)
			{
				var updateRequest = new UpdateEventRequest
				{
					Id = _selectedEvent.Id,
					Type = _newEntry.Type,
					Name = _newEntry.Name!,
					Date = _newEntry.Date!
				};
				var response = await EventService.UpdateEvent(_selectedEvent.Id, updateRequest);
				if (response.Error is not null)
				{
					await ShowException(response.Error);
					return;
				}

				await LoadEvents();
				await _addModal!.HideAsync();
				await ShowSuccess("Successfully updated");
			}
			else
			{
				var request = new InsertEventRequest
				{
					Type = _newEntry.Type,
					Name = _newEntry.Name!,
					Date = _newEntry.Date!
				};
				var response = await EventService.InsertEvent(request);
				if (response.Error is not null)
				{
					await ShowException(response.Error);
					return;
				}

				await LoadEvents();
				await _addModal!.HideAsync();
				await ShowSuccess("Successfully added");
			}
		});
	}

	private void RefreshModalData()
	{
		_isEditMode = false;
		_newEntry = new EventEntryViewModel();
		_editContext = new EditContext(_newEntry);
		_editContext.OnFieldChanged += FieldChange;
		_isValidForm = false;
	}

	private async Task OpenAddModal()
	{
		RefreshModalData();
		await _addModal!.ShowAsync();
	}

	private async Task OpenEditModal(Guid id)
	{
		_selectedEvent = _model.First(x => x.Id == id);
		_editEventEntry = new EventEntryViewModel
		{
			Type = _selectedEvent.Type,
			Name = _selectedEvent.Name,
			Date = _selectedEvent.Date,
			SelectedLocationId = _selectedEvent.SelectedLocationId ?? Guid.Empty,
			SelectedProductIds = _selectedEvent.SelectedProducts.Select(p => p.ProductId).ToList()
		};

		_editEventDetailsContext = new EditContext(_editEventEntry);
		_editEventDetailsContext.OnFieldChanged += FieldChange;
		_isEditMode = true;
		_isValidForm = true;

		_editLocationPeople = _selectedEvent.LocationPeople < 1 ? 1 : _selectedEvent.LocationPeople;
		_editLocationHours = _selectedEvent.LocationHours < 1 ? 1 : _selectedEvent.LocationHours;

		await LoadAvailableLocationsAndProducts();
		await _editEventModal!.ShowAsync();
	}

	private async Task CloseAddModal()
	{
		await _addModal!.HideAsync();
	}

	private async Task OpenDeleteModal(Guid id)
	{
		_selectedEvent = _model.First(x => x.Id == id);
		await _deleteModal!.ShowAsync();
	}

	private async Task CloseDeleteModal()
	{
		await _deleteModal!.HideAsync();
	}

	private async Task ConfirmDeleteEvent()
	{
		await SafeExecute(async () =>
		{
			var response = await EventService.DeleteEvent(_selectedEvent.Id);
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			await LoadEvents();
			await _deleteModal!.HideAsync();
			await ShowSuccess("Successfully deleted");
		});
	}

	private async Task OpenInfoModal(Guid id)
	{
		_selectedEvent = _model.First(x => x.Id == id);
		await _infoModal!.ShowAsync();
	}

	private async Task CloseInfoModal()
	{
		await _infoModal!.HideAsync();
	}

	private async Task CloseEditEventModal()
	{
		await _editEventModal!.HideAsync();
	}

	private async Task OpenCheckoutModal(Guid id)
	{
		_selectedEvent = _model.First(x => x.Id == id);
		_checkout = new CheckoutViewModel();
		_checkoutContext = new EditContext(_checkout);
		_checkoutContext.OnFieldChanged += CheckoutFieldChange;
		_isCheckoutValid = false;
		_isProcessingPayment = false;
		await _checkoutModal!.ShowAsync();
	}

	private async Task CloseCheckoutModal()
	{
		await _checkoutModal!.HideAsync();
	}

	private void CheckoutFieldChange(object? sender, FieldChangedEventArgs e)
	{
		_isCheckoutValid = _checkoutContext.Validate();
	}

	private void RefreshCheckoutModalData()
	{
		if (_checkoutContext is not null)
		{
			_checkoutContext.OnFieldChanged -= CheckoutFieldChange;
		}

		// Card details only ever live in this transient model and are never persisted or sent to the server.
		_checkout = new CheckoutViewModel();
		_isCheckoutValid = false;
		_isProcessingPayment = false;
	}

	private async Task ProcessPayment()
	{
		if (!_checkoutContext.Validate())
		{
			return;
		}

		if (_selectedEvent.EventOrderId is not { } orderId || orderId == Guid.Empty)
		{
			await ShowException(new Exception("There is no order to pay for this event yet."));
			return;
		}

		_isProcessingPayment = true;

		await SafeExecute(async () =>
		{
			var response = await EventOrderService.PayEventOrder(orderId);
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			await LoadEvents();
			await _checkoutModal!.HideAsync();
			await ShowSuccess("Payment completed successfully");
		});

		_isProcessingPayment = false;
	}

	private async Task LoadAvailableLocationsAndProducts()
	{
		await SafeExecute(async () =>
		{
			var productsResponse = await ProductService.GetProducts();
			if (productsResponse.Error is null && productsResponse.Content is not null)
			{
				var allProducts = productsResponse.Content.ToList();

				_availableLocationsForEdit = allProducts
					.Where(p => p.Type == 1)
					.Select(p => new LocationOptionViewModel
					{
						Id = p.Id,
						Name = p.Name,
						Price = p.Price,
						IsPerPerson = p.IsPerPerson,
						IsHourly = p.IsHourly
					})
					.ToList();

				_availableProductsForEdit = allProducts
					.Where(p => p.Type != 1)
					.Select(p =>
					{
						var existing = _selectedEvent.SelectedProducts.FirstOrDefault(sp => sp.ProductId == p.Id);
						return new ProductOptionViewModel
						{
							Id = p.Id,
							Name = p.Name,
							Price = p.Price,
							Type = p.Type,
							IsPerPerson = p.IsPerPerson,
							IsHourly = p.IsHourly,
							NumberOfPeople = existing?.NumberOfPeople ?? 1,
							NumberOfHours = existing?.NumberOfHours ?? 1
						};
					})
					.OrderBy(p => p.Name)
					.ToList();
			}
		});
	}

	private void UpdateProductSelection(Guid productId, bool isSelected)
	{
		if (_editEventEntry == null)
			return;

		if (isSelected && !_editEventEntry.SelectedProductIds.Contains(productId))
		{
			_editEventEntry.SelectedProductIds.Add(productId);
		}
		else if (!isSelected && _editEventEntry.SelectedProductIds.Contains(productId))
		{
			_editEventEntry.SelectedProductIds.Remove(productId);
		}
	}

	private async Task SaveEventWithLocationAndProducts()
	{
		if (_editEventEntry == null)
			return;

		await SafeExecute(async () =>
		{
			// First, find the existing event order for this event
			var eventOrdersResponse = await EventOrderService.GetEventOrders();
			var existingEventOrder = eventOrdersResponse.Content?.FirstOrDefault(eo => eo.EventId == _selectedEvent.Id);

			if (existingEventOrder == null)
			{
				await ShowException(new Exception("No event order found for this event"));
				return;
			}

			// Build the products list with location as the first product if selected
			var productsForUpdate = new List<EventOrderProductRequest>();

			// Add location if selected
			if (_editEventEntry.SelectedLocationId.HasValue && _editEventEntry.SelectedLocationId != Guid.Empty)
			{
				productsForUpdate.Add(new EventOrderProductRequest
				{
					ProductId = _editEventEntry.SelectedLocationId.Value,
					Quantity = 1
				});
			}

			// Add other products
			foreach (var productId in _editEventEntry.SelectedProductIds)
			{
				productsForUpdate.Add(new EventOrderProductRequest
				{
					ProductId = productId,
					Quantity = 1
				});
			}

			// Update the event order with new location and products
			var updateRequest = new CreateEventOrderRequest
			{
				PersonId = existingEventOrder.PersonId,
				EventId = _selectedEvent.Id,
				Products = productsForUpdate
			};

			var response = await EventOrderService.UpdateEventOrder(existingEventOrder.Id, updateRequest);
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}
		});
	}

	private async Task SaveEditEventDetails()
	{
		if (_editEventEntry == null || !_editEventDetailsContext!.Validate())
		{
			return;
		}

		await SafeExecute(async () =>
		{
			var updateRequest = new UpdateEventRequest
			{
				Id = _selectedEvent.Id,
				Type = _editEventEntry.Type,
				Name = _editEventEntry.Name!,
				Date = _editEventEntry.Date!
			};
			var response = await EventService.UpdateEvent(_selectedEvent.Id, updateRequest);
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			await LoadEvents();
			await _editEventModal!.HideAsync();
			await ShowSuccess("Event updated successfully");
		});
	}

	private async Task SaveEditEventComplete()
	{
		if (_editEventEntry == null || !_editEventDetailsContext!.Validate())
		{
			return;
		}

		await SafeExecute(async () =>
		{
			// First, update the event details
			var updateEventRequest = new UpdateEventRequest
			{
				Id = _selectedEvent.Id,
				Type = _editEventEntry.Type,
				Name = _editEventEntry.Name!,
				Date = _editEventEntry.Date!
			};
			var eventResponse = await EventService.UpdateEvent(_selectedEvent.Id, updateEventRequest);
			if (eventResponse.Error is not null)
			{
				await ShowException(eventResponse.Error);
				return;
			}

			// Then, update the location and products in the event order
			var eventOrdersResponse = await EventOrderService.GetEventOrders();
			var existingEventOrder = eventOrdersResponse.Content?.FirstOrDefault(eo => eo.EventId == _selectedEvent.Id);

			if (existingEventOrder != null)
			{
				var productsForUpdate = new List<EventOrderProductRequest>();

				// Add location if selected
				if (_editEventEntry.SelectedLocationId.HasValue && _editEventEntry.SelectedLocationId != Guid.Empty)
				{
					var location = GetEditSelectedLocation();
					productsForUpdate.Add(new EventOrderProductRequest
					{
						ProductId = _editEventEntry.SelectedLocationId.Value,
						Quantity = 1,
						NumberOfPeople = location?.IsPerPerson == true ? Math.Max(1, _editLocationPeople) : 1,
						NumberOfHours = location?.IsHourly == true ? Math.Max(1, _editLocationHours) : 1
					});
				}

				// Add other products
				foreach (var productId in _editEventEntry.SelectedProductIds)
				{
					var product = _availableProductsForEdit.FirstOrDefault(p => p.Id == productId);
					productsForUpdate.Add(new EventOrderProductRequest
					{
						ProductId = productId,
						Quantity = 1,
						NumberOfPeople = product?.IsPerPerson == true ? Math.Max(1, product.NumberOfPeople) : 1,
						NumberOfHours = product?.IsHourly == true ? Math.Max(1, product.NumberOfHours) : 1
					});
				}

				var updateOrderRequest = new CreateEventOrderRequest
				{
					PersonId = existingEventOrder.PersonId,
					EventId = _selectedEvent.Id,
					Products = productsForUpdate
				};

				var orderResponse = await EventOrderService.UpdateEventOrder(existingEventOrder.Id, updateOrderRequest);
				if (orderResponse.Error is not null)
				{
					await ShowException(orderResponse.Error);
					return;
				}
			}

			await LoadEvents();
			await _editEventModal!.HideAsync();
			await ShowSuccess("Event and venue updated successfully");
		});
	}

	private void RefreshEditEventModalData()
	{
		_editEventEntry = null;
		_availableLocationsForEdit = [];
		_availableProductsForEdit = [];
		_editEventDetailsContext = null!;
		_isEditMode = false;
	}

	private void RefreshEditVenueProductsModalData()
	{
		_editEventEntry = null;
		_availableLocationsForEdit = [];
		_availableProductsForEdit = [];
	}

	#endregion // Private Methods

	#region IDispose Members

	public void Dispose()
	{
		if (_editContext is not null)
		{
			_editContext.OnFieldChanged -= FieldChange;
		}

		if (_checkoutContext is not null)
		{
			_checkoutContext.OnFieldChanged -= CheckoutFieldChange;
		}

		_cts?.Dispose();
	}

	#endregion // IDispose Members
}
