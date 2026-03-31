using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Momentum.BackOffice.Extensions;
using Momentum.BackOffice.Services;

namespace Momentum.BackOffice.Features.EventOrders;

[Route("/create-event-order")]
public partial class CreateEventOrderPage : IDisposable
{
    #region Fields

    private int CurrentStep { get; set; }
    private CreateEventOrderViewModel _model = new();
    private PersonDetail? _selectedPerson;
    private LocationOptionViewModel? _selectedLocation;
    private bool _isSubmitting;
    private HashSet<string> _expandedCategories = [];
    private List<ProductGroupViewModel> _productGroups = [];

    #endregion // Fields

    #region Private Properties

    [Inject] private IPersonService PersonService { get; set; } = null!;
    [Inject] private IProductService ProductService { get; set; } = null!;
    [Inject] private IEventOrderService EventOrderService { get; set; } = null!;
    [Inject] private IEventService EventService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    #endregion // Private Properties

    #region Lifecycle Methods

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
        _expandedCategories.Add("Location (Accommodation)");
    }

    #endregion // Lifecycle Methods

    #region Private Methods

    private async Task LoadData()
    {
        await SafeExecute(async () =>
        {
            // Load persons
            var personsResponse = await PersonService.GetPersons();
            if (personsResponse.Error is null && personsResponse.Content is not null)
            {
                _model.AvailablePersons = personsResponse.Content
                    .Select(p => new PersonOptionViewModel
                    {
                        Id = p.Id,
                        FullName = $"{p.FirstName} {p.LastName}",
                        CNP = p.CNP
                    })
                    .ToList();
            }

            // Load products
            var productsResponse = await ProductService.GetProducts();
            if (productsResponse.Error is null && productsResponse.Content is not null)
            {
                _model.AvailableProducts = productsResponse.Content
                    .Select(p => new ProductSelectionViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        Type = p.Type,
                        TypeName = GetProductTypeName(p.Type)
                    })
                    .OrderBy(p => p.Type)
                    .ToList();

                // Build product groups
                BuildProductGroups();

                // Filter locations for step 3
                _model.AvailableLocations = _model.AvailableProducts
                    .Where(p => p.Type == 1)
                    .Select(p => new LocationOptionViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        CompanyName = "Event Venue" // This should come from the API response with company info
                    })
                    .ToList();
            }
        });
    }

    private void BuildProductGroups()
    {
        _productGroups = _model.AvailableProducts
            .Where(p => p.Type != 1) // Exclude locations from product step
            .GroupBy(p => p.TypeName)
            .OrderBy(g => g.Key)
            .Select(g => new ProductGroupViewModel
            {
                TypeName = g.Key,
                Count = g.Count(),
                Products = g.OrderBy(p => p.Name).ToList()
            })
            .ToList();
    }

    private string GetProductTypeName(int type)
    {
        return type switch
        {
            1 => "Location (Accommodation)",
            2 => "Food",
            3 => "Photography",
            4 => "Music",
            5 => "Videography",
            6 => "Decoration",
            _ => "Unknown"
        };
    }

    private bool CanProceedToNext()
    {
        return CurrentStep switch
        {
            0 => _model.SelectedPersonId != Guid.Empty,
            1 => !string.IsNullOrWhiteSpace(_model.EventName) &&
                 _model.EventType > 0 &&
                 !string.IsNullOrWhiteSpace(_model.EventDate),
            2 => _model.SelectedLocationId != Guid.Empty,
            3 => _model.SelectedProducts.Count > 0,
            _ => true
        };
    }

    private void NextStep()
    {
        if (CanProceedToNext())
        {
            CurrentStep++;
            StateHasChanged();
        }
    }

    private void PreviousStep()
    {
        if (CurrentStep > 0)
        {
            CurrentStep--;
            StateHasChanged();
        }
    }

    private void SelectPerson(PersonOptionViewModel person)
    {
        _model.SelectedPersonId = person.Id;
        var response = new PersonDetail
        {
            Id = person.Id,
            FirstName = person.FullName.Split(' ')[0],
            LastName = string.Join(" ", person.FullName.Split(' ').Skip(1)),
            CNP = person.CNP
        };
        _selectedPerson = response;
    }

    private void SelectLocation(LocationOptionViewModel location)
    {
        _model.SelectedLocationId = location.Id;
        _selectedLocation = location;
    }

    private void ToggleCategoryExpanded(string categoryName)
    {
        if (_expandedCategories.Contains(categoryName))
        {
            _expandedCategories.Remove(categoryName);
        }
        else
        {
            _expandedCategories.Add(categoryName);
        }
    }

    private bool IsProductSelected(Guid productId)
    {
        return _model.SelectedProducts.Any(p => p.ProductId == productId);
    }

    private decimal GetSelectedProductQuantity(Guid productId)
    {
        return _model.SelectedProducts.FirstOrDefault(p => p.ProductId == productId)?.Quantity ?? 1;
    }

    private void AddProduct(ProductSelectionViewModel product)
    {
        var selectedProduct = new SelectedProductViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            UnitPrice = product.Price,
            Quantity = 1
        };
        _model.SelectedProducts.Add(selectedProduct);
    }

    private void RemoveProduct(Guid productId)
    {
        var product = _model.SelectedProducts.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            _model.SelectedProducts.Remove(product);
        }
    }

    private void IncreaseQuantity(Guid productId)
    {
        var product = _model.SelectedProducts.FirstOrDefault(p => p.ProductId == productId);
        if (product != null)
        {
            product.Quantity++;
        }
    }

    private void DecreaseQuantity(Guid productId)
    {
        var product = _model.SelectedProducts.FirstOrDefault(p => p.ProductId == productId);
        if (product != null && product.Quantity > 1)
        {
            product.Quantity--;
        }
    }

    private void UpdateQuantity(Guid productId, object? value)
    {
        if (value != null && decimal.TryParse(value.ToString(), out var quantity) && quantity > 0)
        {
            var product = _model.SelectedProducts.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                product.Quantity = quantity;
            }
        }
    }

    private async Task SubmitOrder()
    {
        _isSubmitting = true;
        await SafeExecute(async () =>
        {
            // First create the event
            var eventCreateRequest = new InsertEventRequest
            {
                Type = _model.EventType,
                Name = _model.EventName ?? "",
                Date = _model.EventDate ?? ""
            };

            var eventResponse = await EventService.InsertEvent(eventCreateRequest);
            if (eventResponse.IsSuccessStatusCode == false)
            {
                await ShowException(new Exception("Failed to create event"));
                return;
            }

            // Get event ID from response headers or assume it's created
            // Since the API returns IApiResponse with no content, we need to get the event ID differently
            // For now, load events and get the latest one
            var eventsResponse = await EventService.GetEvents();
            if (eventsResponse.IsSuccessStatusCode == false || eventsResponse.Content == null)
            {
                await ShowException(new Exception("Failed to retrieve event"));
                return;
            }

            var createdEvent = eventsResponse.Content.FirstOrDefault();
            if (createdEvent == null)
            {
                await ShowException(new Exception("No event found after creation"));
                return;
            }

            // Then create the event order
            var orderRequest = new CreateEventOrderRequest
            {
                PersonId = _model.SelectedPersonId,
                EventId = createdEvent.Id,
                Products = _model.SelectedProducts
                    .Select(p => new BackOffice.Services.EventOrderProductRequest
                    {
                        ProductId = p.ProductId,
                        Quantity = (int)p.Quantity
                    })
                    .ToList()
            };

            var result = await EventOrderService.CreateEventOrder(orderRequest);
            if (result.IsSuccessStatusCode)
            {
                await ShowSuccess("Event order created successfully!");
                NavigationManager.NavigateTo(PageRoutes.Events);
            }
            else
            {
                await ShowException(new Exception("Failed to create event order"));
            }
        });
        _isSubmitting = false;
    }

    #endregion // Private Methods

    #region Nested Classes

    public class ProductGroupViewModel
    {
        public string TypeName { get; set; } = string.Empty;
        public int Count { get; set; }
        public List<ProductSelectionViewModel> Products { get; set; } = [];
    }

    #endregion // Nested Classes

    public void Dispose()
    {
        // Cleanup if needed
    }
}
