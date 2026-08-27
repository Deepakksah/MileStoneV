using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public enum IdentityTypeDto
{
	[EnumMember]
	InvalidIdentity,
	[EnumMember]
	UnknownIdentityType,
	[EnumMember]
	WindowsUser,
	[EnumMember]
	WindowsGroup
}
