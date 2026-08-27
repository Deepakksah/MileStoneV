using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace VideoOS.IdentityServer.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class SIDAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (value == null)
		{
			return ValidationResult.Success;
		}
		try
		{
			new SecurityIdentifier((string)value);
		}
		catch (ArgumentException)
		{
			return new ValidationResult("Must be a correct SID format.");
		}
		return ValidationResult.Success;
	}
}
