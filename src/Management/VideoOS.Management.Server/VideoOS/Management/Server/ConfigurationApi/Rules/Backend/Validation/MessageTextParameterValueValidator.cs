using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class MessageTextParameterValueValidator : IValueValidator
{
	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType = null, IDictionary<string, RuleParameter> parameters = null)
	{
		return value.ToString().Length <= 200;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		return null;
	}
}
