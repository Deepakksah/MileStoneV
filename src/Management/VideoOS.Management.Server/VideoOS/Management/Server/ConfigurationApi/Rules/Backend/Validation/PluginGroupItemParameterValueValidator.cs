using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class PluginGroupItemParameterValueValidator : IValueValidator
{
	private IMIPServerGroupProxy _mipServerGroupProxy;

	public PluginGroupItemParameterValueValidator(IMIPServerGroupProxy mipServerGroupProxy)
	{
		_mipServerGroupProxy = mipServerGroupProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		try
		{
			if (_mipServerGroupProxy.QueryServerGroupById(requestContext, itemId) != null)
			{
				return "MIPKind";
			}
		}
		catch (ServerGroupNotFoundException)
		{
		}
		return null;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		MIPServerGroup mIPServerGroup = null;
		try
		{
			mIPServerGroup = _mipServerGroupProxy.QueryServerGroupById(requestContext, value.ToString());
		}
		catch (ServerGroupNotFoundException)
		{
		}
		if (mIPServerGroup == null)
		{
			return false;
		}
		return string.Equals(expectedType, mIPServerGroup.Kind, StringComparison.OrdinalIgnoreCase);
	}
}
