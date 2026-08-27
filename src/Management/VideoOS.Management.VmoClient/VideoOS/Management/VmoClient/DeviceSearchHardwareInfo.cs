using System;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchHardwareInfo
{
	public Guid Id { get; }

	public string Name { get; }

	public Guid RecordingServerId { get; }

	public DeviceSearchHardwareInfo(Guid id, string name, Guid recordingServerId)
	{
		Id = id;
		Name = name;
		RecordingServerId = recordingServerId;
	}
}
