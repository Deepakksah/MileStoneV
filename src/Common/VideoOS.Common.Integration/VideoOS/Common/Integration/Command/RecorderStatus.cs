using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[Obsolete("Replaced by RecorderStatuses in version 12.2")]
[DataContract]
public enum RecorderStatus
{
	[EnumMember]
	NoStatus,
	[EnumMember]
	Starting,
	[EnumMember]
	RepairingDatabase,
	[EnumMember]
	DiskFull,
	[EnumMember]
	Failover
}
