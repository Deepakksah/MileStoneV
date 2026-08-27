using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class DeviceFilterValidator : ISpecialValidator<Device>
{
	private readonly IDictionary<string, Func<Device, bool>> _filterValidators;

	public DeviceFilterValidator()
	{
		_filterValidators = new Dictionary<string, Func<Device, bool>>
		{
			{
				"Ptz",
				(Device device) => device.SettingUtilities.IsPtzEnabled
			},
			{
				"EdgeStorage",
				(Device device) => device.SettingUtilities.IsEdgeStorageSupported
			}
		};
	}

	public bool Validate(Device model, IDictionary<string, RuleParameter> parameters)
	{
		if (parameters == null)
		{
			return true;
		}
		if (!parameters.TryGetValue("__Filters", out var value) || value.IsEmpty)
		{
			return true;
		}
		return value.Values.All((string filterName) => !_filterValidators.TryGetValue(filterName, out var value2) || value2(model));
	}
}
