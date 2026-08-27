using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Attributes;

public class MaxLengthUriAttribute : ValidationAttribute
{
	public int Maximum { get; set; }

	public MaxLengthUriAttribute(int maximum)
	{
		Maximum = maximum;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		if (value == null)
		{
			return ValidationResult.Success;
		}
		if (!(value is Uri))
		{
			return new ValidationResult("MaxLengthUri attribute is only applicable on type uri");
		}
		if (value.ToString().Length > Maximum)
		{
			return new ValidationResult("Uri length cannot be more than " + Maximum);
		}
		return ValidationResult.Success;
	}
}
