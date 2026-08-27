using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class UserDefinedEventValueValidator : IValueValidator
{
	private readonly IUserDefinedEventProxy _userDefinedEventProxy;

	private readonly ISiteProxy _siteProxy;

	public UserDefinedEventValueValidator(IUserDefinedEventProxy userDefinedEventProxy, ISiteProxy siteProxy)
	{
		_userDefinedEventProxy = userDefinedEventProxy;
		_siteProxy = siteProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		if (_userDefinedEventProxy.QueryUserDefinedEvent(requestContext, itemId) == null)
		{
			return null;
		}
		return "UserDefinedEvent";
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		if (parameters != null && parameters.TryGetValue("SiteId", out var value2))
		{
			Site site = _siteProxy.QueryHomeSite(requestContext);
			if (value2.Value.Equals(site?.Id, StringComparison.OrdinalIgnoreCase))
			{
				return string.Equals(GetItemType(requestContext, value.ToString()), expectedType, StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}
		return false;
	}
}
