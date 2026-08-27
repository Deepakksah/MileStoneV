using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class AccessControlGroupEventParameterValueValidator : IValueValidator
{
	private readonly IMIPServerGroupProxy _serverGroupProxy;

	public AccessControlGroupEventParameterValueValidator(IMIPServerGroupProxy serverGroupProxy)
	{
		_serverGroupProxy = serverGroupProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		MIPServerGroup mIPServerGroup = _serverGroupProxy.QueryServerGroupById(requestContext, itemId);
		if (mIPServerGroup != null && string.Equals(mIPServerGroup.PluginId, SourceGuid.AccessControl.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			return "AccessControlRuleGroup";
		}
		return null;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		string itemType = GetItemType(requestContext, value.ToString(), parameters);
		return string.Equals(expectedType, itemType, StringComparison.OrdinalIgnoreCase);
	}
}
