using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LogicalOperator", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public enum LogicalOperator
{
	[EnumMember]
	And,
	[EnumMember]
	Or
}
