using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class PtzPresetParameterValueValidator : IValueValidator
{
	private readonly IDeviceProxy _deviceProxy;

	public PtzPresetParameterValueValidator(IDeviceProxy deviceProxy)
	{
		_deviceProxy = deviceProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		string value = parameters["DeviceId"].Value;
		try
		{
			bool? flag = _deviceProxy.QueryDevicePtzPresets(requestContext, value)?.Any((PtzPreset p) => p.Id == new Guid(itemId));
			if (!flag.HasValue || flag == false)
			{
				return null;
			}
		}
		catch (DeviceNotFoundException)
		{
			return null;
		}
		return "PtzPreset";
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
