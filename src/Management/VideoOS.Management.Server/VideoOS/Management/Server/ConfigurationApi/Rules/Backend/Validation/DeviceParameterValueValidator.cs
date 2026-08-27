using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class DeviceParameterValueValidator : IValueValidator
{
	private readonly IDeviceProxy _deviceProxy;

	private readonly ISpecialValidator<Device> _filtersValidator;

	public DeviceParameterValueValidator(IDeviceProxy deviceProxy, ISpecialValidator<Device> deviceFilterValidator)
	{
		_deviceProxy = deviceProxy;
		_filtersValidator = deviceFilterValidator;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		return GetDeviceWithItemType(requestContext, itemId)?.Item1;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		Tuple<string, Device> deviceWithItemType = GetDeviceWithItemType(requestContext, value.ToString());
		if (deviceWithItemType == null)
		{
			return false;
		}
		if (!string.Equals(deviceWithItemType.Item1, expectedType, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return _filtersValidator.Validate(deviceWithItemType.Item2, parameters);
	}

	private Tuple<string, Device> GetDeviceWithItemType(IGeminiRequestContext requestContext, string itemId)
	{
		Device device = _deviceProxy.QueryDeviceById(requestContext, itemId);
		if (device == null)
		{
			return null;
		}
		return new Tuple<string, Device>(DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == device.DeviceType).ItemType, device);
	}
}
