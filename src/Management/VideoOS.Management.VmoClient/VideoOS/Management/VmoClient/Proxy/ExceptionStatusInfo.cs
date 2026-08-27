using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ExceptionStatusInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public enum ExceptionStatusInfo
{
	[EnumMember]
	Inactive,
	[EnumMember]
	InProgress,
	[EnumMember]
	Succeeded,
	[EnumMember]
	Failed,
	[EnumMember]
	Stopping,
	[EnumMember]
	Stopped
}
