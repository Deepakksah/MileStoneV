using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
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
