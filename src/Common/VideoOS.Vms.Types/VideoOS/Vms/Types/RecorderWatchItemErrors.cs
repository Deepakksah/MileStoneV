using System;

namespace VideoOS.Vms.Types;

[Serializable]
[Flags]
public enum RecorderWatchItemErrors
{
	None = 0,
	SslCommunication = 1,
	Authorization = 2
}
