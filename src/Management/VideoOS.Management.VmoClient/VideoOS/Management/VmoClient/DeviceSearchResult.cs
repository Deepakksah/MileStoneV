using System.Collections.ObjectModel;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchResult
{
	public Collection<DeviceSearchRecordingServerInfo> RecordingServers { get; } = new Collection<DeviceSearchRecordingServerInfo>();

	public Collection<DeviceSearchHardwareInfo> Hardware { get; } = new Collection<DeviceSearchHardwareInfo>();

	public Collection<DeviceSearchDeviceInfo> Devices { get; } = new Collection<DeviceSearchDeviceInfo>();
}
