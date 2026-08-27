using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FaultType", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server.WebService.Map")]
public enum FaultType
{
	[EnumMember]
	Unknown,
	[EnumMember]
	EndpointNotFound,
	[EnumMember]
	InvalidToken,
	[EnumMember]
	ReferenceExists,
	[EnumMember]
	Timestamp,
	[EnumMember]
	InvalidID,
	[EnumMember]
	InvalidBounds,
	[EnumMember]
	MarkedForDeletion,
	[EnumMember]
	InvalidTimeout,
	[EnumMember]
	InvalidVisualizationState,
	[EnumMember]
	ReloadRequired,
	[EnumMember]
	InsufficientRights,
	[EnumMember]
	InvalidServerID,
	[EnumMember]
	InvalidAuthentication
}
