using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VideoOS.WebService.Common.SecurityService;

[Serializable]
[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
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
