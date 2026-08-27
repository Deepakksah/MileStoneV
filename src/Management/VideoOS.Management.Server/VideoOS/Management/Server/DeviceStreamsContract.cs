using System;
using System.Collections.ObjectModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class DeviceStreamsContract
{
	public string Id { get; set; }

	public DeviceType DeviceType { get; set; }

	public int DeviceIndex { get; set; }

	public string StreamsXml { get; set; }

	public string HardwareDefinitionDigest { get; set; }

	public DateTime LastModified { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }

	public Collection<DeviceTrackContract> DeviceTracks { get; set; } = new Collection<DeviceTrackContract>();
}
