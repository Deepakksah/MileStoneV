using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "StatusFaultType", Namespace = "http://videoos.net/2/MapServerStatusCommand")]
public enum StatusFaultType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	EndpointNotFound,
	[EnumMember]
	InvalidToken,
	[EnumMember]
	InvalidAuthentication,
	[EnumMember]
	InvalidTimeout,
	[EnumMember]
	InvalidVisualizationState,
	[EnumMember]
	InvalidID,
	[EnumMember]
	MarkedForDeletion
}
