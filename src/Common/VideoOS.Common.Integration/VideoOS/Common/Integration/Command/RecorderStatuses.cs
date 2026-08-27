using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[Flags]
public enum RecorderStatuses
{
	[EnumMember]
	None = 0,
	[EnumMember]
	RepairingDatabase = 1,
	[EnumMember]
	ArchivingStorageOffline = 2,
	[EnumMember]
	StorageOffline = 4,
	[EnumMember]
	Failover = 8,
	[EnumMember]
	ArchivingStorageUnmounted = 0x10
}
