using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.IdentityServer.Services;

namespace VideoOS.IdentityServer.Validators;

public class PasswordRequirementsAttribute : ValidationAttribute
{
	public override bool RequiresValidationContext => true;

	protected override ValidationResult IsValid(object value, ValidationContext context)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (!(context.GetService(typeof(IPasswordRequirements)) is IPasswordRequirements passwordRequirements))
		{
			throw new NullReferenceException();
		}
		if (!((ValidationAttribute)new RegularExpressionAttribute(passwordRequirements.Pattern)).IsValid(value))
		{
			return new ValidationResult(((ValidationAttribute)this).ErrorMessageString);
		}
		return ValidationResult.Success;
	}
}
