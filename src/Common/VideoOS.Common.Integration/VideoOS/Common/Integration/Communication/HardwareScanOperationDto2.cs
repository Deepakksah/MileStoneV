using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum HardwareScanOperationDto2
{
	CancelScan,
	ExpressScan,
	RangeScan,
	ManualScan,
	HostedScan
}
