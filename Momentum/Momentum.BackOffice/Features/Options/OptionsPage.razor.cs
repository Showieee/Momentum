using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Components;
using Momentum.BackOffice.Services;
using Momentum.BackOffice.Services.Models;
using Momentum.BackOffice.Shared;

namespace Momentum.BackOffice.Features.Options;

[Route($"/{PageRoutes.Options}")]
public partial class OptionsPage
{
	#region Fields

	private OptionsViewModel? _model;
	private FluentValidator<IValidator<OptionsViewModel>> _formValidator = null!;

	#endregion //Fields

	#region Private Properties

	[Inject] private IOptionsService OptionsService { get; set; } = null!;

	private OptionsViewModel Model
	{
		get
		{
			if (_model is null)
			{
				Model = new OptionsViewModel();
			}

			return _model!;
		}
		set => _model = value;
	}

	#endregion // Private Properties

	#region Private Methods

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		await SafeExecute(async () =>
		{
			var response = await OptionsService.GetOptions();
			if (response.Error is not null)
			{
				await ShowException(response.Error);
				return;
			}

			if (response.Content is not null)
			{
				var options = response.Content;
				var model = options.Adapt<OptionsViewModel>();
				Model = model;
			}
		});
	}

	private async Task Submit()
	{
		await SafeExecute(async () =>
		{
			var options = Model.Adapt<UpsertOptionsRequest>();
			await OptionsService.UpsertOptions(options);
			await ShowSuccess("Saved Successfully");
		});
	}

	#endregion //Private Methods
}