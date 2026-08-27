using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:messagewatch:1")]
public class SCHwatchItem
{
	[DataMember]
	public string SessionID;

	[DataMember]
	public string MsgTypeName;

	[DataMember]
	public SCHmatchExpression[] MatchExpressions;
}
