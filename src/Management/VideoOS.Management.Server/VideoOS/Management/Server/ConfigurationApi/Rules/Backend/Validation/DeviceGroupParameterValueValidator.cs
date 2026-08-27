using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class DeviceGroupParameterValueValidator : IValueValidator
{
	private readonly IDeviceProxy _deviceProxy;

	public DeviceGroupParameterValueValidator(IDeviceProxy deviceProxy)
	{
		_deviceProxy = deviceProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		DeviceGroup deviceGroup = _deviceProxy.QueryDeviceGroupById(requestContext, itemId);
		if (deviceGroup == null)
		{
			return null;
		}
		return DeviceGroupConfigurationApi.ConvertDeviceTypeToItemType(deviceGroup.DeviceType);
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString(), parameters), expectedType, StringComparison.OrdinalIgnoreCase);
	}
}
