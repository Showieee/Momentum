using BlazorBootstrap;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Momentum.BackOffice.Extensions;
using Momentum.BackOffice.Services;

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
        _searchText = e.Value?.ToString()?.Trim().ToUpper() ?? string.Empty;

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
                x.LastName?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true)
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
        });
    }

    private void RefreshModalData()
    {
        _newEntry = new PersonEntryViewModel();
        _editContext = new EditContext(_newEntry);
        _editContext.OnFieldChanged += FieldChange;
        _isValidForm = false;
    }

    private async Task OpenAddModal()
    {
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

            // Refresh list after deletion
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