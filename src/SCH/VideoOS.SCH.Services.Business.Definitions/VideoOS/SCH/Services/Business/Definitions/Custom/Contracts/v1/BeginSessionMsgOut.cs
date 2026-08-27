using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:messagewatch:1")]
public class BeginSessionMsgOut
{
	[DataMember]
	public OperationStatus CurrentStatus;

	[DataMember]
	public string SessionID;

	[DataMember]
	public int RecommendedMsgChkIntervalInMs;
}
