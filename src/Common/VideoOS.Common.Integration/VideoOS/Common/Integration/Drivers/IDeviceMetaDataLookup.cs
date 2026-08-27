using System;

namespace VideoOS.Common.Integration.Drivers;

public interface IDeviceMetaDataLookup
{
	string DeviceDisplayName(Guid deviceID);
}
