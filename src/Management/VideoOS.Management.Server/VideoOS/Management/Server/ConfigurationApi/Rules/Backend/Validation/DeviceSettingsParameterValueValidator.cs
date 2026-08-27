using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class DeviceSettingsParameterValueValidator : IValueValidator
{
	private readonly IDeviceProxy _deviceProxy;

	private readonly IDeviceSettingsProxy _deviceSettingsProxy;

	public DeviceSettingsParameterValueValidator(IDeviceProxy deviceProxy, IDeviceSettingsProxy deviceSettingsProxy)
	{
		_deviceProxy = deviceProxy;
		_deviceSettingsProxy = deviceSettingsProxy;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		List<string> list = new List<string>();
		try
		{
			if (parameters.Any((KeyValuePair<string, RuleParameter> p) => p.Key == "DeviceIds") && !parameters["DeviceIds"].IsEmpty)
			{
				list.AddRange(parameters["DeviceIds"].Values);
			}
			if (parameters.Any((KeyValuePair<string, RuleParameter> p) => p.Key == "DeviceGroupIds") && !parameters["DeviceGroupIds"].IsEmpty)
			{
				string[] values = parameters["DeviceGroupIds"].Values;
				foreach (string deviceGroupId in values)
				{
					list.AddRange(_deviceProxy.QueryDeviceIdsByDeviceGroup(requestContext, deviceGroupId, DeviceTypeSearch.All));
				}
			}
		}
		catch (DeviceQueryByDeviceGroupException)
		{
			return false;
		}
		return list.All((string deviceId) => _deviceSettingsProxy.Validate(requestContext, deviceId, value.ToString()));
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		return null;
	}
}
