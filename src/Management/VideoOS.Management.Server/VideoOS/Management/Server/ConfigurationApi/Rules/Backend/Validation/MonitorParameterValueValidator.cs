using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class MonitorParameterValueValidator : IValueValidator
{
	private readonly IMonitorProxy _monitorProxy;

	public MonitorParameterValueValidator(IMonitorProxy monitorProxy)
	{
		_monitorProxy = monitorProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		if (_monitorProxy.QueryMonitor(requestContext, itemId) == null)
		{
			return null;
		}
		return "Monitor";
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters = null)
	{
		if (expectedType == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString()), expectedType, StringComparison.OrdinalIgnoreCase);
	}
}
