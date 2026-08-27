using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:common")]
public class OperationStatus
{
	[DataMember]
	public OperationStatusEnum Status;

	[DataMember]
	public ErrorItem[] Errors;
}
