using System;

namespace VideoOS.Management.VmoClient;

[Serializable]
public enum ManagementServiceStatus
{
	PendingStatus,
	Running,
	Stopped,
	NotSupported,
	ConnectionFailed
}
