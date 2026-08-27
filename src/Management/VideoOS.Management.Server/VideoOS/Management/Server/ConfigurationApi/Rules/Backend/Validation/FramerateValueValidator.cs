using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class FramerateValueValidator : IValueValidator
{
	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType = null, IDictionary<string, RuleParameter> parameters = null)
	{
		if (!double.TryParse(value.ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
		{
			return false;
		}
		string[] array = result.ToString(CultureInfo.InvariantCulture).Split('.');
		if (array.Length != 1)
		{
			if (array.Length == 2)
			{
				return array[1].Length <= 2;
			}
			return false;
		}
		return true;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		return null;
	}
}
