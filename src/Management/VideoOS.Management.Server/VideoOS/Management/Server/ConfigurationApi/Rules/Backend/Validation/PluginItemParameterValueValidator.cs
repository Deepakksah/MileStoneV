using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class PluginItemParameterValueValidator : IValueValidator
{
	private IMIPObjectProxy _mipObjectProxy;

	public PluginItemParameterValueValidator(IMIPObjectProxy mipObjectProxy)
	{
		_mipObjectProxy = mipObjectProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		try
		{
			if (_mipObjectProxy.QueryMIPObject(requestContext, itemId) != null)
			{
				return "MIPItem";
			}
		}
		catch (MIPObjectNotFoundException)
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
		MIPObject mIPObject = null;
		try
		{
			mIPObject = _mipObjectProxy.QueryMIPObject(requestContext, value.ToString());
		}
		catch (MIPObjectNotFoundException)
		{
		}
		if (mIPObject == null)
		{
			return false;
		}
		return string.Equals(expectedType, mIPObject.Kind, StringComparison.OrdinalIgnoreCase);
	}
}
