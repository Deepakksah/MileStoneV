using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IDeviceTrackDataComponent : IDisposable
{
	Dictionary<Guid, IEnumerable<DeviceTrackData>> QueryDeviceTracksByDeviceIds(Guid[] ids);
}
