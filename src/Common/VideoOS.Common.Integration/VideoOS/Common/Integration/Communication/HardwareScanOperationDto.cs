using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[Obsolete("Do not use in Corporate 5.0")]
public enum HardwareScanOperationDto
{
	BeginScan,
	CancelScan,
	StopScan
}
