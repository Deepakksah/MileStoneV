using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "IntercommunicationFaultType", Namespace = "http://videoos.net/2/CentralServerInterCommunicationCommand")]
public enum IntercommunicationFaultType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	InvalidToken,
	[EnumMember]
	Unauthorized,
	[EnumMember]
	InvalidTimeout,
	[EnumMember]
	InvalidTimeZone,
	[EnumMember]
	InvalidRequest
}
