using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util.Security;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IdentityType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
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
