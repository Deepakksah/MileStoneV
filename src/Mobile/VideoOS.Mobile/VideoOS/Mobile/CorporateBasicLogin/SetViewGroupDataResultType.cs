using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SetViewGroupDataResultType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum SetViewGroupDataResultType
{
	[EnumMember]
	UnspecifiedError,
	[EnumMember]
	Success,
	[EnumMember]
	ReadOnlyError,
	[EnumMember]
	ModifiedError
}
