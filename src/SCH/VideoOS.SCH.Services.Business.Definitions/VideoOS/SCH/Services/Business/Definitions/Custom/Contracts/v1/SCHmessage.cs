using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
public class SCHmessage
{
	[DataMember]
	public int MessageID;

	[DataMember]
	public string MsgTypeName;

	[DataMember]
	public DateTime Created;

	[DataMember]
	public SCHmessageDataItem[] DataItems;
}
