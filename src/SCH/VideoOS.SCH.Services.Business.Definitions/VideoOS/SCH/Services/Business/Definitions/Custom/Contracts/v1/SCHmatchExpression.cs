using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:messagewatch:1")]
public class SCHmatchExpression
{
	[DataMember]
	public SCHmessageDataItem Data;

	[DataMember]
	public string OperatorName;
}
