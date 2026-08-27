using System;

namespace VideoOS.Server;

public interface IDeviceDataDistributionComponent : IDisposable
{
	DeviceDataDistribution[] QueryDeviceDataDistribution(Guid? deviceId, Guid? recorderId, Guid? trackId);

	void DeleteDeviceDataDistribution(Guid recorderId, Guid trackId);
}
