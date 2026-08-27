using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmFaultType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public enum AlarmFaultType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	DbRead,
	[EnumMember]
	Read,
	[EnumMember]
	DbConnection,
	[EnumMember]
	Serialization,
	[EnumMember]
	DbWrite,
	[EnumMember]
	DbUpdate,
	[EnumMember]
	InvalidAuthentication,
	[EnumMember]
	UnknownSession
}
