using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum NetworkSettingsManagementError
{
	[EnumMember]
	Unsupported,
	[EnumMember]
	UnknownError,
	[EnumMember]
	HardwareNotStarted,
	[EnumMember]
	HardwareReadOnly,
	[EnumMember]
	HardwareBlocked,
	[EnumMember]
	HardwareNotEnabled,
	[EnumMember]
	NoHardwareAccess,
	[EnumMember]
	NoSuitableInterfaceAddress,
	[EnumMember]
	UnknownResult,
	[EnumMember]
	GenericError,
	[EnumMember]
	OtherError
}
