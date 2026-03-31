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
	private EventViewModel _selectedEvent = new();
	private EventEntryViewModel _newEntry = new();
	private bool _isValidForm;
	private EditContext _editContext = null!;
	private bool _isEditMode;

	private CancellationTokenSource? _cts;

	#endregion // Fields

	#region Private Properties

	[Inject] private IEventService EventService { get; set; } = null!;

	#endregion // Private Properties

	#region Private Methods

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
				_model = response.Content.Select(e => new EventViewModel
				{
					Id = e.Id,
					Type = e.Type,
					Name = e.Name,
					Date = e.Date
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
		_newEntry = new EventEntryViewModel
		{
			Type = _selectedEvent.Type,
			Name = _selectedEvent.Name,
			Date = _selectedEvent.Date
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
