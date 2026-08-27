using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class NotifiedDevicesConfiguration
{
	public Dictionary<DeviceState, List<Guid>> Devices { get; }

	public override int GetHashCode()
	{
		if (Devices == null)
		{
			return 0;
		}
		return Devices.GetHashCode();
	}

	public NotifiedDevicesConfiguration()
	{
		Devices = new Dictionary<DeviceState, List<Guid>>();
	}

	public void AddDeviceConfiguration(Guid deviceId, DeviceState state)
	{
		if (!Devices.ContainsKey(state))
		{
			Devices.Add(state, new List<Guid>());
		}
		Devices[state].Add(deviceId);
	}

	public override bool Equals(object other)
	{
		if (!(other is NotifiedDevicesConfiguration notifiedDevicesConfiguration))
		{
			return false;
		}
		foreach (DeviceState enumValue in EnumsHelper.GetEnumValues<DeviceState>())
		{
			Devices.TryGetValue(enumValue, out var value);
			notifiedDevicesConfiguration.Devices.TryGetValue(enumValue, out var value2);
			if ((value != null || value2 != null) && (value == null || value2 == null || !EqualityHelper.Equals(value, value2, null)))
			{
				return false;
			}
		}
		return true;
	}
}
