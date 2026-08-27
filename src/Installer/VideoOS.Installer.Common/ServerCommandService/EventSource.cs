using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace ServerCommandService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventSource", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public enum EventSource
{
	[EnumMember]
	Undefined,
	[EnumMember]
	Device,
	[EnumMember]
	External,
	[EnumMember]
	Hardware,
	[EnumMember]
	Recorder,
	[EnumMember]
	Server,
	[EnumMember]
	System,
	[EnumMember]
	Timer
}
