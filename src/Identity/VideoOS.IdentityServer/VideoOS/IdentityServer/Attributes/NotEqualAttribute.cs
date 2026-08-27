using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VideoOS.IdentityServer.Attributes;

public class NotEqualAttribute(string otherProperty) : ValidationAttribute, IClientModelValidator
{
	private string OtherProperty { get; } = otherProperty;

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		object obj = validationContext.ObjectType.GetProperty(OtherProperty)?.GetValue(validationContext.ObjectInstance);
		if (value == null || obj == null)
		{
			return ValidationResult.Success;
		}
		if (!string.Equals(value.ToString(), obj.ToString()))
		{
			return ValidationResult.Success;
		}
		return new ValidationResult(((ValidationAttribute)this).ErrorMessageString);
	}

	public void AddValidation(ClientModelValidationContext context)
	{
		MergeAttribute(context.Attributes, "data-val", "true");
		MergeAttribute(context.Attributes, "data-val-notequalto", ((ValidationAttribute)this).ErrorMessageString);
		MergeAttribute(context.Attributes, "data-val-notequalto-property", "*." + OtherProperty);
	}

	private void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
	{
		attributes.TryAdd(key, value);
	}
}
