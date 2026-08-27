using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:common")]
public enum OperationStatusEnum
{
	[EnumMember]
	NotStarted,
	[EnumMember]
	Enqueued,
	[EnumMember]
	InProcess,
	[EnumMember]
	Completed,
	[EnumMember]
	Failed
}
