using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace Momentum.BackOffice.Shared;

public sealed class FluentValidator<TValidator> : ComponentBase where TValidator : IValidator
{
	#region Fields

	private readonly char[] _separators = ['.', '['];
	private TValidator _validator = default!;
	private bool _isReadOnly;

	#endregion //Fields

	#region Public Properties

	[Inject] public IServiceProvider ServiceProvider { get; set; } = null!;

	public TValidator Validator => _validator;

	#endregion //Public Properties

	#region Public Properties

	[Parameter]
	public bool IsReadOnly
	{
		get => _isReadOnly;
		set
		{
			_isReadOnly = value;
			EditContext.Validate();
		}
	}

	[CascadingParameter] internal EditContext EditContext { get; private set; } = null!;

	#endregion //Public Properties

	#region Private Methods

	protected override void OnInitialized()
	{
		_validator = (TValidator)ServiceProvider.GetRequiredService(typeof(TValidator));
		var messages = new ValidationMessageStore(EditContext);

		EditContext.OnFieldChanged += (sender, _)
			=> ValidateModel((EditContext)sender!, messages);

		EditContext.OnValidationRequested += (sender, _)
			=> ValidateModel((EditContext)sender!, messages);
	}

	private void ValidateModel(EditContext editContext, ValidationMessageStore messages)
	{
		messages.Clear();

		if (!IsReadOnly)
		{
			var context = new ValidationContext<object>(editContext.Model);
			var validationResult = _validator.Validate(context);
			foreach (var error in validationResult.Errors)
			{
				var fieldIdentifier = ToFieldIdentifier(editContext, error.PropertyName);
				messages.Add(fieldIdentifier, error.ErrorMessage);
			}
		}

		editContext.NotifyValidationStateChanged();
	}

	private FieldIdentifier ToFieldIdentifier(EditContext editContext, string propertyPath)
	{
		var obj = editContext.Model;

		while (true)
		{
			var nextTokenEnd = propertyPath.IndexOfAny(_separators);
			if (nextTokenEnd < 0)
			{
				return new FieldIdentifier(obj, propertyPath);
			}

			var nextToken = propertyPath.Substring(0, nextTokenEnd);
			propertyPath = propertyPath.Substring(nextTokenEnd + 1);

			object? newObj = null;
			if (nextToken.EndsWith("]"))
			{
				// It's an indexer
				// This code assumes C# conventions (one indexer named Item with one param)
				nextToken = nextToken.Substring(0, nextToken.Length - 1);
				var properties = obj.GetType().GetProperties()
					.Where(p => p.Name == "Item" && p.GetIndexParameters().Length == 1 && p.GetIndexParameters()[0].ParameterType == typeof(int));
				var prop = properties.FirstOrDefault();
				if (prop is not null)
				{
					var indexerType = prop.GetIndexParameters()[0].ParameterType;
					var indexerValue = Convert.ChangeType(nextToken, indexerType);
					newObj = prop.GetValue(obj, [indexerValue]);
				}
			}
			else
			{
				// It's a regular property
				var prop = obj.GetType().GetProperty(nextToken);
				if (prop == null)
				{
					throw new InvalidOperationException($"Could not find property named {nextToken} on object of type {obj.GetType().FullName}.");
				}
				newObj = prop.GetValue(obj);
			}

			if (newObj == null)
			{
				// This is as far as we can go
				return new FieldIdentifier(obj, nextToken);
			}

			obj = newObj;
		}
	}

	#endregion //Private Methods
}