using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Attributes;

public class HttpOrHttpsAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		if (value == null)
		{
			return ValidationResult.Success;
		}
		if (!(value is Uri { IsAbsoluteUri: not false } uri))
		{
			return new ValidationResult("Invalid URI format.");
		}
		if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
		{
			return new ValidationResult("URI must be either HTTP or HTTPS.");
		}
		return ValidationResult.Success;
	}
}
