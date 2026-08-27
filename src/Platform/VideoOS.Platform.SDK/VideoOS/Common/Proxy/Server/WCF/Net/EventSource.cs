using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
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
