using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class AccessControlEventParameterValueValidator : IValueValidator
{
	private readonly IMIPObjectProxy _mipObjectProxy;

	private readonly IMIPServerGroupProxy _mipServerGroupProxy;

	private const string AllSystemsGroupId = "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD";

	public AccessControlEventParameterValueValidator(IMIPObjectProxy mipObjectProxy, IMIPServerGroupProxy mipServerGroupProxy)
	{
		_mipObjectProxy = mipObjectProxy;
		_mipServerGroupProxy = mipServerGroupProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		string serverGroupId = _mipObjectProxy.QueryMIPObject(requestContext, itemId)?.RelationIds.Split(';').FirstOrDefault().Trim();
		MIPServerGroup mIPServerGroup = _mipServerGroupProxy.QueryServerGroupById(requestContext, serverGroupId);
		if (mIPServerGroup == null)
		{
			return null;
		}
		if (!string.Equals(mIPServerGroup.ParentId, "C3BAD46C-BBA8-0FB0-1580-244BCFE26ADD", StringComparison.OrdinalIgnoreCase))
		{
			return "AccessControlUnit";
		}
		return "AccessControlSystem";
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
