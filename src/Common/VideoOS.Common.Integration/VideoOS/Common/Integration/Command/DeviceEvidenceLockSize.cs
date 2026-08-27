using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DeviceEvidenceLockSize
{
	[DataMember]
	public Guid DeviceId { get; private set; }

	[DataMember]
	public long Size { get; private set; }

	public DeviceEvidenceLockSize(Guid deviceId, long size)
	{
		DeviceId = deviceId;
		Size = size;
	}
}
