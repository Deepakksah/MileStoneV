using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class UriValidator : ValidationRule
{
	private static readonly string[] ValidSchemes = new string[2]
	{
		Uri.UriSchemeHttp,
		Uri.UriSchemeHttps
	};

	public override ValidationResult Validate(object value, CultureInfo cultureInfo)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (!Uri.TryCreate(value.ToString(), UriKind.Absolute, out var result) || !ValidSchemes.Contains(result.Scheme))
		{
			return new ValidationResult(isValid: false, value.ToString());
		}
		return ValidationResult.ValidResult;
	}
}
