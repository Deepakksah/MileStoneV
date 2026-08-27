using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "AuthorizationActionType", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum AuthorizationActionType
{
	[EnumMember]
	None,
	[EnumMember]
	LogOn
}
