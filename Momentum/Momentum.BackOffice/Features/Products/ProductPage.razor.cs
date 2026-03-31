using BlazorBootstrap;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Momentum.BackOffice.Extensions;
using Momentum.BackOffice.Services;

namespace Momentum.BackOffice.Features.Products;

[Route($"/{PageRoutes.Products}")]
public partial class ProductPage : IDisposable
{
	#region Fields

	private List<ProductViewModel> _model = [];
	private List<ProductViewModel> _filteredModel = [];
	private List<CompanyInfo> _companies = [];
	private string _searchText = string.Empty;
	private Modal? _addModal;
	private Modal? _deleteModal;
	private ProductViewModel _selectedProduct = new();
	private ProductEntryViewModel _newEntry = new();
	private bool _isValidForm;
	private EditContext _editContext = null!;
	private bool _isEditMode;

	private CancellationTokenSource? _cts;

	#endregion // Fields

	#region Private Properties

	[Inject] private IProductService ProductService { get; set; } = null!;
	[Inject] private ICompanyService CompanyService { get; set; } = null!;

	#endregion // Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		_newEntry = new ProductEntryViewModel();
		_editContext = new EditContext(_newEntry);
		_editContext.OnFieldChanged += FieldChange;

		await LoadCompanies();
		await LoadProducts();
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
				return;
			}

			if (response.Content is not null)
			{
				_companies = response.Content
					.Select(c => new CompanyInfo { Id = c.Id, Name = c.Name })
					.ToList();
			}
		});
	}

	private async Task LoadProducts()
	{
		await SafeExecute(async () =>
		{
			var response = await ProductService.GetProducts();
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			if (response.Content is not null)
			{
				_model = response.Content.Select(p => new ProductViewModel
				{
					Id = p.Id,
					Type = p.Type,
					Name = p.Name,
					Description = p.Description,
					Price = p.Price,
					CompanyId = p.CompanyId,
					CompanyName = p.Company?.Name ?? string.Empty
				}).ToList();
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
				x.CompanyName?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) == true)
				.ToList();
		}
	}

	private async Task SaveProduct()
	{
		if (!_editContext!.Validate())
		{
			return;
		}

		await SafeExecute(async () =>
		{
			if (_isEditMode)
			{
				var updateRequest = new UpdateProductRequest
				{
					Id = _selectedProduct.Id,
					Type = _newEntry.Type,
					Name = _newEntry.Name!,
					Description = _newEntry.Description,
					Price = _newEntry.Price,
					CompanyId = _newEntry.CompanyId
				};
				var response = await ProductService.UpdateProduct(_selectedProduct.Id, updateRequest);
				if (response.Error is not null)
				{
					await ShowException(response.Error);
					return;
				}

				await LoadProducts();
				await _addModal!.HideAsync();
				await ShowSuccess("Successfully updated");
			}
			else
			{
				var request = new InsertProductRequest
				{
					Type = _newEntry.Type,
					Name = _newEntry.Name!,
					Description = _newEntry.Description,
					Price = _newEntry.Price,
					CompanyId = _newEntry.CompanyId
				};
				var response = await ProductService.InsertProduct(request);
				if (response.Error is not null)
				{
					await ShowException(response.Error);
					return;
				}

				await LoadProducts();
				await _addModal!.HideAsync();
				await ShowSuccess("Successfully added");
			}
		});
	}

	private void RefreshModalData()
	{
		_isEditMode = false;
		_newEntry = new ProductEntryViewModel();
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
		_selectedProduct = _model.First(x => x.Id == id);
		_newEntry = new ProductEntryViewModel
		{
			Type = _selectedProduct.Type,
			Name = _selectedProduct.Name,
			Description = _selectedProduct.Description,
			Price = _selectedProduct.Price,
			CompanyId = _selectedProduct.CompanyId
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
		_selectedProduct = _model.First(x => x.Id == id);
		await _deleteModal!.ShowAsync();
	}

	private async Task CloseDeleteModal()
	{
		await _deleteModal!.HideAsync();
	}

	private async Task ConfirmDeleteProduct()
	{
		await SafeExecute(async () =>
		{
			var response = await ProductService.DeleteProduct(_selectedProduct.Id);
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			await LoadProducts();
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

		_cts?.Dispose();
	}

	#endregion // IDispose Members

	private class CompanyInfo
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
