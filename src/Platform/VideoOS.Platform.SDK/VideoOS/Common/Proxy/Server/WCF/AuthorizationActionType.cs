using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AuthorizationActionType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum AuthorizationActionType
{
	[EnumMember]
	None,
	[EnumMember]
	LogOn
}
