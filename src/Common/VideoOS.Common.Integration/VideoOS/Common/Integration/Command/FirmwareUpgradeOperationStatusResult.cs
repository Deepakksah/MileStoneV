using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum FirmwareUpgradeOperationStatusResult
{
	[EnumMember]
	None = 0,
	[EnumMember]
	Completed = 11,
	[EnumMember]
	InProgress = 12,
	[EnumMember]
	WarningNotChanged = 13,
	[EnumMember]
	ErrorCannotOpenFile = 21,
	[EnumMember]
	ErrorNetwork = 22,
	[EnumMember]
	ErrorReconnecting = 23,
	[EnumMember]
	ErrorFirmwareRejected = 24,
	[EnumMember]
	ErrorAlreadyRunning = 25,
	[EnumMember]
	ErrorUnsupported = 26,
	[EnumMember]
	ErrorUnknown = 27
}
