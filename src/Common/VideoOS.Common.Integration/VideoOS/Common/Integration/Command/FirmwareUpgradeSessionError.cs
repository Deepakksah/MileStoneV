using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum FirmwareUpgradeSessionError
{
	[EnumMember]
	HardwareNotStarted,
	[EnumMember]
	HardwareReadOnly,
	[EnumMember]
	HardwareBlocked,
	[EnumMember]
	HardwareNotEnabled,
	[EnumMember]
	UnknownError,
	[EnumMember]
	SessionAlreadyExists,
	[EnumMember]
	SessionNotFound,
	[EnumMember]
	OperationTimeout
}
