using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VideoOS.IdentityServer.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class NonEmptyList : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		IEnumerable<object> source = value as IEnumerable<object>;
		if (value == null)
		{
			return new ValidationResult("List must not be null.");
		}
		if (source.Count() == 0)
		{
			return new ValidationResult("List must not be empty.");
		}
		return ValidationResult.Success;
	}
}
