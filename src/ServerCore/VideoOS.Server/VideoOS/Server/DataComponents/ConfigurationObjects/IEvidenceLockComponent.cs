using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public interface IEvidenceLockComponent : IDisposable
{
	bool DeviceHasEvidenceLock(string deviceId);

	bool RecorderHasLockedDeviceDataDistribution(Guid id);

	List<Guid> QueryAllEvidenceLockIds();

	List<DeviceEvidenceLockData> QueryDeviceEvidenceLocks(IEnumerable<Guid> evidenceLockIds);

	bool UpdateEvidenceLockSize(Guid evidenceLockId, IEnumerable<DeviceEvidenceLockSize> deviceEvidenceLockSizes);

	IEnumerable<Guid> QueryEvidenceLockIdsWithoutSize();
}
