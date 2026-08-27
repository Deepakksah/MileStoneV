using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class LayoutParameterValueValidator : IValueValidator
{
	private readonly ILayoutProxy _layoutProxy;

	private static readonly Guid CurrentLayoutId = new Guid("00000000-0000-0000-0000-000000000000");

	public LayoutParameterValueValidator(ILayoutProxy layoutProxy)
	{
		_layoutProxy = layoutProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		if (new Guid(itemId) != CurrentLayoutId && _layoutProxy.QueryLayout(requestContext, itemId) == null)
		{
			return null;
		}
		return "Layout";
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters = null)
	{
		object obj = CurrentLayoutId;
		if (value.Equals(obj))
		{
			return true;
		}
		if (expectedType == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString()), expectedType, StringComparison.OrdinalIgnoreCase);
	}
}
