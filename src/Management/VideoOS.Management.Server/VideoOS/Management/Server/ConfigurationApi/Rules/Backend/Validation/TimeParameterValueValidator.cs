using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class TimeParameterValueValidator : IValueValidator
{
	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType = null, IDictionary<string, RuleParameter> parameters = null)
	{
		string[] formats = new string[4] { "hh\\:mm", "h\\:mm", "hh\\:mm\\:ss", "h\\:mm\\:ss" };
		TimeSpan result;
		return TimeSpan.TryParseExact(value.ToString(), formats, CultureInfo.InvariantCulture, out result);
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		return null;
	}
}
