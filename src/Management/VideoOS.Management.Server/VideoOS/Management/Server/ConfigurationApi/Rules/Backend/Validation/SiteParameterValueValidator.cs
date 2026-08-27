using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class SiteParameterValueValidator : IValueValidator
{
	private readonly ISiteProxy _siteProxy;

	public SiteParameterValueValidator(ISiteProxy siteProxy)
	{
		_siteProxy = siteProxy;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters = null)
	{
		if (expectedType == null)
		{
			return false;
		}
		return GetItemType(requestContext, value.ToString()) == expectedType;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		if (_siteProxy.QuerySite(requestContext, itemId) != null)
		{
			return "Site";
		}
		return null;
	}
}
