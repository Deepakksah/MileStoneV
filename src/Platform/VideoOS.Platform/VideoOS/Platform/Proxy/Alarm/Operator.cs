using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Operator", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public enum Operator
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
