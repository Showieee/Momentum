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
    private bool _isSubmitting;

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
                    .ToList();
            }
        });
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
            2 => _model.SelectedProducts.Count > 0,
            3 => true,
            _ => false
        };
    }

    private void LoadSelectedPersonDetails()
    {
        var selectedPerson = _model.AvailablePersons
            .FirstOrDefault(p => p.Id == _model.SelectedPersonId);

        if (selectedPerson != null)
        {
            var nameParts = selectedPerson.FullName.Split(' ');
            _selectedPerson = new PersonDetail 
            { 
                Id = selectedPerson.Id, 
                FirstName = nameParts.FirstOrDefault() ?? "", 
                LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "",
                CNP = selectedPerson.CNP
            };
        }
    }

    private void OnPersonSelected(ChangeEventArgs e)
    {
        if (Guid.TryParse(e.Value?.ToString(), out var personId))
        {
            _model.SelectedPersonId = personId;
        }
    }

    private bool IsProductSelected(Guid productId)
    {
        return _model.SelectedProducts.Any(p => p.ProductId == productId);
    }

    private void ToggleProductSelection(ProductSelectionViewModel product)
    {
        if (IsProductSelected(product.Id))
        {
            RemoveProduct(product.Id);
        }
        else
        {
            _model.SelectedProducts.Add(new SelectedProductViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = 1
            });
        }
    }

    private void RemoveProduct(Guid productId)
    {
        _model.SelectedProducts.RemoveAll(p => p.ProductId == productId);
    }

    private async Task NextStep()
    {
        if (CanProceedToNext())
        {
            if (CurrentStep == 2)
            {
                LoadSelectedPersonDetails();
            }
            CurrentStep++;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task PreviousStep()
    {
        if (CurrentStep > 0)
        {
            CurrentStep--;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task SubmitOrder()
    {
        if (!CanProceedToNext())
        {
            await ShowError("Please complete all required steps");
            return;
        }

        _isSubmitting = true;

        try
        {
            await SafeExecute(async () =>
            {
                // First, create the event
                var eventRequest = new InsertEventRequest
                {
                    Type = _model.EventType,
                    Name = _model.EventName,
                    Date = _model.EventDate
                };

                var eventResponse = await EventService.InsertEvent(eventRequest);
                if (eventResponse.Error is not null)
                {
                    await ShowException(eventResponse.Error);
                    return;
                }

                // Get the created event to retrieve its ID
                var eventsListResponse = await EventService.GetEvents();
                if (eventsListResponse.Error is not null || eventsListResponse.Content is null)
                {
                    await ShowException(eventResponse.Error);
                    return;
                }

                // Get the most recently created event (by matching name and date)
                var createdEvent = eventsListResponse.Content
                    .FirstOrDefault(e => e.Name == _model.EventName && e.Date == _model.EventDate);

                if (createdEvent is null)
                {
                    await ShowError("Failed to retrieve created event");
                    return;
                }

                // Now create the event order with the created event
                var orderRequest = new CreateEventOrderRequest
                {
                    PersonId = _model.SelectedPersonId,
                    EventId = createdEvent.Id,
                    Products = _model.SelectedProducts
                        .Select(p => new EventOrderProductRequest
                        {
                            ProductId = p.ProductId,
                            Quantity = p.Quantity
                        })
                        .ToList()
                };

                var orderResponse = await EventOrderService.CreateEventOrder(orderRequest);

                if (orderResponse.Error is not null)
                {
                    await ShowException(orderResponse.Error);
                    return;
                }

                await ShowSuccess("Event order created successfully!");
                NavigationManager.NavigateTo($"/{PageRoutes.Events}");
            });
        }
        finally
        {
            _isSubmitting = false;
        }
    }

    private async Task ShowError(string message)
    {
        await InvokeAsync(() => Task.CompletedTask);
    }

    #endregion // Private Methods

    #region IDisposable

    public void Dispose()
    {
        // Cleanup if needed
    }

    #endregion // IDisposable
}
