using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum UserHandlingError
{
	[EnumMember]
	OperationUnsupported,
	[EnumMember]
	HardwareError,
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
	UnknownResult,
	[EnumMember]
	GenericError,
	[EnumMember]
	UserManagementHardwareBlocked,
	[EnumMember]
	UserNameClashError,
	[EnumMember]
	UserNotCreatedError,
	[EnumMember]
	AccessDenied,
	[EnumMember]
	DeleteCurrentUserNotAllowed,
	[EnumMember]
	UserNameNotRecognized,
	[EnumMember]
	UserFixed,
	[EnumMember]
	UserNameTooShort,
	[EnumMember]
	UserNameTooLong,
	[EnumMember]
	PasswordTooWeak,
	[EnumMember]
	PasswordTooLong,
	[EnumMember]
	TooManyUsers,
	[EnumMember]
	CredentialsUpdateError,
	[EnumMember]
	NetworkError,
	[EnumMember]
	InvalidCharacter,
	[EnumMember]
	UserLevelAnonymousNotAllowed,
	[EnumMember]
	NotInFactoryDefaultState
}
