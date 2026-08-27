using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Server.ServiceChannel;

[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "OperationStatusEnum", Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
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
