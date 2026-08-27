using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class LockedDevices
{
	[DataMember]
	public string DeviceName { get; private set; }

	[DataMember]
	public string DeviceId { get; private set; }

	[DataMember]
	public string HardwareId { get; private set; }

	public LockedDevices(string id, string name, string hardwareId)
	{
		DeviceId = id;
		DeviceName = name;
		HardwareId = hardwareId;
	}
}
