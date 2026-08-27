using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
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
