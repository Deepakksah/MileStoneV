using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class TimeProfileParameterValueValidator : IValueValidator
{
	private readonly ITimeProfileProxy _timeProfileProxy;

	public TimeProfileParameterValueValidator(ITimeProfileProxy timeProfileProxy)
	{
		_timeProfileProxy = timeProfileProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		if (_timeProfileProxy.QueryTimeProfile(requestContext, itemId) == null)
		{
			return null;
		}
		return "TimeProfile";
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString(), parameters), expectedType, StringComparison.OrdinalIgnoreCase);
	}
}
