using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VideoOS.Management.Common;

[Serializable]
[DataContract]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSAuthorization")]
public enum IdentityType
{
	[EnumMember]
	InvalidIdentity,
	[EnumMember]
	UnknownIdentityType,
	[EnumMember]
	WindowsUser,
	[EnumMember]
	WindowsGroup,
	[EnumMember]
	Role,
	[EnumMember]
	BasicUser
}
