using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmSecurity.Client;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmSecurityFaultType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public enum AlarmSecurityFaultType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	InvalidAuthentication,
	[EnumMember]
	EndpointNotFound
}
