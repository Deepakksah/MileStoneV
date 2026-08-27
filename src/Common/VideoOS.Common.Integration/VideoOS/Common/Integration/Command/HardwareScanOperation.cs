using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum HardwareScanOperation
{
	[EnumMember]
	CancelScan,
	[EnumMember]
	ExpressScan,
	[EnumMember]
	RangeScan,
	[EnumMember]
	ManualScan,
	[EnumMember]
	HostedScan
}
