using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmClient;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SearchOperator", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public enum SearchOperator
{
	[EnumMember]
	Equals,
	[EnumMember]
	NotEquals,
	[EnumMember]
	LessThan,
	[EnumMember]
	GreaterThan,
	[EnumMember]
	Contains,
	[EnumMember]
	BeginsWith
}
