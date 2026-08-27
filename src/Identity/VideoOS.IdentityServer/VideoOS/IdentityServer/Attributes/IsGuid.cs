using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class IsGuid : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (value == null)
		{
			return ValidationResult.Success;
		}
		if (!Guid.TryParse((string)value, out var _))
		{
			return new ValidationResult("Must be a correct Guid format.");
		}
		return ValidationResult.Success;
	}
}
