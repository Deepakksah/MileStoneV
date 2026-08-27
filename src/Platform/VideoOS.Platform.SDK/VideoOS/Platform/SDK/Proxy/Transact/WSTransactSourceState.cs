using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Transact;

[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "WSTransactSourceState", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
public enum WSTransactSourceState
{
	[EnumMember]
	Undefined,
	[EnumMember]
	Disabled,
	[EnumMember]
	Suspended,
	[EnumMember]
	Running
}
