using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class PatrollingProfileParameterValueValidator : IValueValidator
{
	private readonly IDeviceProxy _deviceProxy;

	private readonly IPatrollingProfileProxy _patrollingProfileProxy;

	public PatrollingProfileParameterValueValidator(IPatrollingProfileProxy patrollingProfileProxy, IDeviceProxy deviceProxy)
	{
		_patrollingProfileProxy = patrollingProfileProxy;
		_deviceProxy = deviceProxy;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters = null)
	{
		if (expectedType == null)
		{
			return false;
		}
		string value2 = parameters["DeviceId"].Value;
		if (value2 == null)
		{
			return false;
		}
		if (_deviceProxy.QueryDeviceById(requestContext, value2) == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString(), parameters), expectedType, StringComparison.OrdinalIgnoreCase);
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		if (_patrollingProfileProxy.QueryPatrollingProfile(requestContext, itemId) == null)
		{
			return null;
		}
		return "PatrollingProfile";
	}
}
