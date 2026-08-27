using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:operationinfo:1")]
public class GetConnectedClientsMsgOut
{
	[DataMember]
	public OperationStatus CurrentStatus;

	[DataMember]
	public SCHclient[] Clients;
}
