using BlazorBootstrap;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Momentum.BackOffice.Extensions;
using Momentum.BackOffice.Services;
using Momentum.Shared.Models.Requests;

namespace Momentum.BackOffice.Features.Persons;

[Route($"/{PageRoutes.Persons}")]
public partial class PersonPage : IDisposable
{
    #region Fields

    private List<PersonViewModel> _model = [];
    private List<PersonViewModel> _filteredModel = [];
    private string _searchText = string.Empty;
    private Modal? _addModal;
    private Modal? _deleteModal;
    private PersonViewModel _selectedPerson = new();
    private PersonEntryViewModel _newEntry = new();
    private bool _isValidForm;
    private EditContext _editContext = null!;
    private bool _isEditMode;

    private CancellationTokenSource? _cts;

    #endregion // Fields

    #region Private Properties

    [Inject] private IPersonService PersonService { get; set; } = null!;

    #endregion // Private Properties

    #region Private Methods

    protected override async Task OnInitializedAsync()
    {
        _newEntry = new PersonEntryViewModel();
        _editContext = new EditContext(_newEntry);
        _editContext.OnFieldChanged += FieldChange;

        await LoadPersons();
    }

    private void FieldChange(object? sender, FieldChangedEventArgs e)
    {
        _isValidForm = _editContext!.Validate();
    }

    private async Task LoadPersons()
    {
        await SafeExecute(async () =>
        {
            var response = await PersonService.GetPersons();
            if (response.Error is not null)
            {
                await ShowException(response.Error);
                return;
            }

            if (response.Content is not null)
            {
                _model = response.Content.Adapt<List<PersonViewModel>>();
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
                .Where(x => x.FirstName?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true ||
                x.LastName?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true ||
                x.CNP?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
        }
    }

    private async Task AddPerson()
    {
        if (!_editContext!.Validate())
        {
            return;
        }

        await SafeExecute(async () =>
        {
            if (_isEditMode)
            {
                var updateRequest = new UpdatePersonRequest
                {
                    Id = _selectedPerson.Id,
                    FirstName = _newEntry.FirstName!,
                    LastName = _newEntry.LastName!,
                    CNP = _newEntry.CNP!,
                    Address = new()
                    {
                        StreetName = _newEntry.Address?.StreetName ?? string.Empty,
                        StreetNumber = _newEntry.Address?.StreetNumber ?? string.Empty,
                        City = _newEntry.Address?.City ?? string.Empty,
                        State = _newEntry.Address?.State ?? string.Empty,
                        Country = _newEntry.Address?.Country ?? string.Empty,
                        Email = _newEntry.Address?.Email ?? string.Empty,
                        PhoneNumber = _newEntry.Address?.PhoneNumber
                    }
                };
                var response = await PersonService.UpdatePerson(_selectedPerson.Id, updateRequest);
                if (response.Error is not null)
                {
                    await ShowException(response.Error);
                    return;
                }

                await LoadPersons();
                await _addModal!.HideAsync();
                await ShowSuccess("Successfully updated");
            }
            else
            {
                var request = _newEntry.ToRequest();
                var response = await PersonService.InsertPerson(request);
                if (response.Error is not null)
                {
                    await ShowException(response.Error);
                    return;
                }

                await LoadPersons();
                await _addModal!.HideAsync();
                await ShowSuccess("Successfully added");
            }
        });
    }

    private void RefreshModalData()
    {
        _isEditMode = false;
        _newEntry = new PersonEntryViewModel();
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
        _selectedPerson = _model.First(x => x.Id == id);
        _newEntry = new PersonEntryViewModel
        {
            FirstName = _selectedPerson.FirstName,
            LastName = _selectedPerson.LastName,
            CNP = _selectedPerson.CNP,
            Address = _selectedPerson.Address != null ? new()
            {
                StreetName = _selectedPerson.Address.StreetName,
                StreetNumber = _selectedPerson.Address.StreetNumber,
                City = _selectedPerson.Address.City,
                State = _selectedPerson.Address.State,
                Country = _selectedPerson.Address.Country,
                Email = _selectedPerson.Address.Email,
                PhoneNumber = _selectedPerson.Address.PhoneNumber
            } : new()
        };

        _editContext = new EditContext(_newEntry);
        _editContext.OnFieldChanged += FieldChange;
        _isEditMode = true;
        _isValidForm = true;

        await _addModal!.ShowAsync();
    }

    private async Task CloseAddModal()
    {
        await _addModal!.HideAsync();
    }

    private async Task OpenDeleteModal(Guid id)
    {
        _selectedPerson = _model.First(x => x.Id == id);
        await _deleteModal!.ShowAsync();
    }

    private async Task CloseDeleteModal()
    {
        await _deleteModal!.HideAsync();
    }

    private async Task ConfirmDeletePerson()
    {
        await SafeExecute(async () =>
        {
            var response = await PersonService.DeletePerson(_selectedPerson.Id);
            if (response.Error is not null)
            {
                await ShowException(response.Error);
                return;
            }

            await LoadPersons();
            await _deleteModal!.HideAsync();
            await ShowSuccess("Successfully deleted");
        });
    }

    #endregion // Private Methods

    #region IDispose Members

    public void Dispose()
    {
        if (_editContext is not null)
        {
            _editContext.OnFieldChanged -= FieldChange;
        }
    }

    #endregion //IDispose Members
}