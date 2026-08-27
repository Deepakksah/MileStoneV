using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:messagecreator:1")]
public class CreateMessagesMsgOut
{
	[DataMember]
	public OperationStatus CurrentStatus;

	[DataMember]
	public int NoofSessionMessagesCreated;
}
