using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[Flags]
public enum RecorderStatusesDto
{
	None = 0,
	RepairingDatabase = 1,
	ArchivingStorageOffline = 2,
	StorageOffline = 4,
	Failover = 8,
	ArchivingStorageUnmounted = 0x10
}
