using System;

namespace VideoOS.Management.VmoClient;

public class DeviceSearchRecordingServerInfo
{
	public Guid Id { get; }

	public string Name { get; }

	public DeviceSearchRecordingServerInfo(Guid id, string name)
	{
		Id = id;
		Name = name;
	}
}
