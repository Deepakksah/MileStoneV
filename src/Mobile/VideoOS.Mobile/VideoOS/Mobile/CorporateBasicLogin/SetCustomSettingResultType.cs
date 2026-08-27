using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SetCustomSettingResultType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum SetCustomSettingResultType
{
	[EnumMember]
	UnspecifiedError,
	[EnumMember]
	Success,
	[EnumMember]
	ModifiedError
}
