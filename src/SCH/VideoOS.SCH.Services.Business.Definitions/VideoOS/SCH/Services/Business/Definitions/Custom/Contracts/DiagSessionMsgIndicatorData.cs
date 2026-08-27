using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagSessionMsgIndicatorData
{
	[DataMember]
	public int TotalMsgCount;

	[DataMember]
	public DateTime LatestMsgCreateTime;

	[DataMember]
	public string[] SessionIdsWithMessages;
}
