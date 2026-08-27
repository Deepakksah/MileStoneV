using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ErrorHelper
{
	public static ValidateResult CreateValidateResult(Exception exception, IEnumerable<Property> properties)
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		if (properties == null)
		{
			throw new ArgumentNullException("properties");
		}
		ArgumentNullException argumentNullException = exception as ArgumentNullException;
		if (argumentNullException != null)
		{
			Property property = properties.FirstOrDefault((Property p) => string.Equals(p.Key, argumentNullException.ParamName, StringComparison.OrdinalIgnoreCase));
			if (property == null)
			{
				return null;
			}
			return ErrorResultPropertyNullOrEmpty(property);
		}
		ArgumentException argumentException = exception as ArgumentException;
		if (argumentException != null)
		{
			Property property2 = properties.FirstOrDefault((Property p) => string.Equals(p.Key, argumentException.ParamName, StringComparison.OrdinalIgnoreCase));
			if (property2 == null)
			{
				return null;
			}
			return ErrorResultPropertyOutOfRange(property2);
		}
		PropertyInvalidException propertyInvalidException = exception as PropertyInvalidException;
		if (propertyInvalidException != null)
		{
			Property property3 = properties.FirstOrDefault((Property p) => string.Equals(p.Key, propertyInvalidException.PropertyName, StringComparison.OrdinalIgnoreCase));
			if (property3 == null)
			{
				return null;
			}
			return ErrorResultPropertyInvalid(property3, propertyInvalidException.Message);
		}
		return null;
	}

	public static ValidateResult ErrorResultPropertyNullOrEmpty(Property property)
	{
		ValidateResult validateResult = new ValidateResult();
		validateResult.ErrorResults = new ErrorResult[1]
		{
			new ErrorResult
			{
				ErrorProperty = property.Key,
				ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorPropertyNullOrEmpty, property.DisplayName),
				ErrorTextId = "ErrorPropertyNullOrEmpty"
			}
		};
		validateResult.ValidatedOk = false;
		validateResult.ResultItem = null;
		return validateResult;
	}

	public static ValidateResult ErrorResultPropertyOutOfRange(Property property)
	{
		ValidateResult validateResult = new ValidateResult();
		validateResult.ErrorResults = new ErrorResult[1]
		{
			new ErrorResult
			{
				ErrorProperty = property.Key,
				ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, property.DisplayName),
				ErrorTextId = "ErrorOutOfRange"
			}
		};
		validateResult.ValidatedOk = false;
		validateResult.ResultItem = null;
		return validateResult;
	}

	public static ErrorResult ErrorPropertyOutOfRange(Property property)
	{
		return new ErrorResult
		{
			ErrorProperty = property.Key,
			ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, property.DisplayName),
			ErrorTextId = "ErrorOutOfRange"
		};
	}

	public static ValidateResult ErrorResultPropertyInvalid(Property property, string text)
	{
		ValidateResult validateResult = new ValidateResult();
		validateResult.ErrorResults = new ErrorResult[1]
		{
			new ErrorResult
			{
				ErrorProperty = property.Key,
				ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorPropertyInvalid, property.DisplayName, text ?? ""),
				ErrorTextId = "ErrorPropertyInvalid"
			}
		};
		validateResult.ValidatedOk = false;
		validateResult.ResultItem = null;
		return validateResult;
	}

	public static ValidateResult ErrorResultPropertyMissing(string key)
	{
		ValidateResult validateResult = new ValidateResult();
		validateResult.ErrorResults = new ErrorResult[1]
		{
			new ErrorResult
			{
				ErrorProperty = key,
				ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorPropertyInvalid, key, "missing"),
				ErrorTextId = "ErrorPropertyMissing"
			}
		};
		validateResult.ValidatedOk = false;
		validateResult.ResultItem = null;
		return validateResult;
	}

	public static ValidateResult ErrorResultNameAlreadyExists()
	{
		ValidateResult validateResult = new ValidateResult();
		validateResult.ErrorResults = new ErrorResult[1]
		{
			new ErrorResult
			{
				ErrorProperty = "PropertyNameAlreadyExists",
				ErrorText = ConfigurationApiResources.ErrorNameAlreadyExists,
				ErrorTextId = "ErrorNameAlreadyExists"
			}
		};
		validateResult.ValidatedOk = false;
		validateResult.ResultItem = null;
		return validateResult;
	}
}
