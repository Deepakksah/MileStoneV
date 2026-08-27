using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class HardwareParameterValueValidator : IValueValidator
{
	private readonly IHardwareProxy _hardwareProxy;

	public HardwareParameterValueValidator(IHardwareProxy hardwareProxy)
	{
		_hardwareProxy = hardwareProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		if (_hardwareProxy.QueryHardwareById(requestContext, itemId) == null)
		{
			return null;
		}
		return "Hardware";
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		return GetItemType(requestContext, value.ToString(), null) == expectedType;
	}
}
