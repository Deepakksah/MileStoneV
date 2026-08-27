using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IOState", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public enum IOState
{
	[EnumMember]
	Activated,
	[EnumMember]
	Deactivated,
	[EnumMember]
	NotAvailable
}
