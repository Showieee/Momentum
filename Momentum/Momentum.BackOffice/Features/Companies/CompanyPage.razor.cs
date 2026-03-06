using BlazorBootstrap;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Momentum.BackOffice.Extensions;
using Momentum.BackOffice.Services;
using Momentum.Shared.Models.Requests;

namespace Momentum.BackOffice.Features.Companies;

[Route($"/{PageRoutes.Companies}")]
public partial class CompanyPage : IDisposable
{
	#region Fields

	private List<CompanyViewModel> _model = [];
	private List<CompanyViewModel> _filteredModel = [];
	private string _searchText = string.Empty;
	private Modal? _addModal;
	private Modal? _deleteModal;
	private CompanyViewModel _selectedCompany = new();
	private CompanyEntryViewModel _newEntry = new();
	private bool _isValidForm;
	private EditContext _editContext = null!;
	private bool _isEditMode;

	private CancellationTokenSource? _cts;

	#endregion // Fields

	#region Private Properties

	[Inject] private ICompanyService CompanyService { get; set; } = null!;

	#endregion // Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		_newEntry = new CompanyEntryViewModel();
		_editContext = new EditContext(_newEntry);
		_editContext.OnFieldChanged += FieldChange;

		await LoadCompanies();
	}

	private void FieldChange(object? sender, FieldChangedEventArgs e)
	{
		_isValidForm = _editContext!.Validate();
	}

	private async Task LoadCompanies()
	{
		await SafeExecute(async () =>
		{
			var response = await CompanyService.GetCompanies();
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			if (response.Content is not null)
			{
				_model = response.Content.Adapt<List<CompanyViewModel>>();
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
				.Where(x => x.Name?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true ||
				x.CUI?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true)
				.ToList();
		}
	}

	private async Task SaveCompany()
	{
		if (!_editContext!.Validate())
		{
			return;
		}

		await SafeExecute(async () =>
		{
			if (_isEditMode)
			{
				var updateRequest = new UpdateCompanyRequest
				{
					Id = _selectedCompany.Id,
					Name = _newEntry.Name!,
					CUI = _newEntry.CUI!,
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
				var response = await CompanyService.UpdateCompany(_selectedCompany.Id, updateRequest);
				if (response.Error is not null)
				{
					await ShowException(response.Error);
					return;
				}

				await LoadCompanies();
				await _addModal!.HideAsync();
				await ShowSuccess("Successfully updated");
			}
			else
			{
				var request = _newEntry.ToRequest();
				var response = await CompanyService.InsertCompany(request);
				if (response.Error is not null)
				{
					await ShowException(response.Error);
					return;
				}

				await LoadCompanies();
				await _addModal!.HideAsync();
				await ShowSuccess("Successfully added");
			}
		});
	}

	private void RefreshModalData()
	{
		_isEditMode = false;
		_newEntry = new CompanyEntryViewModel();
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
		_selectedCompany = _model.First(x => x.Id == id);
		_newEntry = new CompanyEntryViewModel
		{
			Name = _selectedCompany.Name,
			CUI = _selectedCompany.CUI,
			Address = _selectedCompany.Address != null ? new()
			{
				StreetName = _selectedCompany.Address.StreetName,
				StreetNumber = _selectedCompany.Address.StreetNumber,
				City = _selectedCompany.Address.City,
				State = _selectedCompany.Address.State,
				Country = _selectedCompany.Address.Country,
				Email = _selectedCompany.Address.Email,
				PhoneNumber = _selectedCompany.Address.PhoneNumber
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
		_selectedCompany = _model.First(x => x.Id == id);
		await _deleteModal!.ShowAsync();
	}

	private async Task CloseDeleteModal()
	{
		await _deleteModal!.HideAsync();
	}

	private async Task ConfirmDeleteCompany()
	{
		await SafeExecute(async () =>
		{
			var response = await CompanyService.DeleteCompany(_selectedCompany.Id);
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			await LoadCompanies();
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
