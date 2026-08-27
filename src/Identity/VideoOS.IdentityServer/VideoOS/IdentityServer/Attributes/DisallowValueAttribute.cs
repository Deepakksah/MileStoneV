using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VideoOS.IdentityServer.Attributes;

public class DisallowValueAttribute : ValidationAttribute
{
	private readonly object[] _notAllowedValues;

	public DisallowValueAttribute(params object[] notAllowedValues)
	{
		_notAllowedValues = notAllowedValues ?? throw new ArgumentNullException("notAllowedValues");
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if (value == null || !_notAllowedValues.Any())
		{
			return ValidationResult.Success;
		}
		object[] notAllowedValues = _notAllowedValues;
		foreach (object obj in notAllowedValues)
		{
			if (value == obj)
			{
				return new ValidationResult(GetErrorMessage());
			}
		}
		return ValidationResult.Success;
	}

	private string GetErrorMessage()
	{
		return "Cannot have a value of (" + string.Join(", ", _notAllowedValues) + ")";
	}
}
