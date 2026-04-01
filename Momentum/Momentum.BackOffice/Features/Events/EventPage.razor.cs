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
	private EventViewModel _selectedEvent = new();
	private EventEntryViewModel _newEntry = new();
	private EventEntryViewModel? _editEventEntry;
	private List<LocationOptionViewModel> _availableLocationsForEdit = [];
	private List<ProductOptionViewModel> _availableProductsForEdit = [];
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
			_ => "Service"
		};
	}

	private decimal GetEditModalTotalPrice()
	{
		if (_editEventEntry == null)
			return 0m;

		decimal total = 0m;

		// Add location price
		if (_editEventEntry.SelectedLocationId.HasValue && _editEventEntry.SelectedLocationId != Guid.Empty)
		{
			var location = _availableLocationsForEdit.FirstOrDefault(l => l.Id == _editEventEntry.SelectedLocationId);
			if (location != null)
			{
				total += location.Price;
			}
		}

		// Add selected products prices
		foreach (var productId in _editEventEntry.SelectedProductIds)
		{
			var product = _availableProductsForEdit.FirstOrDefault(p => p.Id == productId);
			if (product != null)
			{
				total += product.Price;
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
		_isValidForm = _editContext!.Validate();
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
						// Find location product (Type = 1)
						var locationProduct = eventOrder.Products?.FirstOrDefault(p => p.Product?.Type == 1);
						if (locationProduct != null && locationProduct.Product != null)
						{
							eventViewModel.SelectedLocationId = locationProduct.ProductId;
							eventViewModel.LocationName = locationProduct.Product.Name;
							eventViewModel.LocationPrice = locationProduct.UnitPrice;
						}

						// Get non-location products
						eventViewModel.SelectedProducts = eventOrder.Products?
							.Where(p => p.Product?.Type != 1)
							.Select(p => new EventProductDetailViewModel
							{
								ProductId = p.ProductId,
								ProductName = p.Product?.Name ?? "Unknown",
								Price = p.UnitPrice
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
						Price = p.Price
					})
					.ToList();

				_availableProductsForEdit = allProducts
					.Where(p => p.Type != 1)
					.Select(p => new ProductOptionViewModel
					{
						Id = p.Id,
						Name = p.Name,
						Price = p.Price,
						Type = p.Type
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

		_cts?.Dispose();
	}

	#endregion // IDispose Members
}
