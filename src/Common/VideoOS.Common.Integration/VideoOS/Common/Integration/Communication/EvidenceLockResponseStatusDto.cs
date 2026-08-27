using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[Obsolete("We need to keep class only because it is used in legacy interfaces.")]
public enum EvidenceLockResponseStatusDto
{
	None,
	Success,
	Error,
	Warning,
	FailedDevices
}
