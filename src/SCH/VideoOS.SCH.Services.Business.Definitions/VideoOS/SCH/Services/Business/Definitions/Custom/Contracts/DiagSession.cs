using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagSession
{
	[DataMember]
	public string ID;

	[DataMember]
	public int[] MessageIDs;

	[DataMember]
	public string ClientName;

	[DataMember]
	public string ClientUserName;

	[DataMember]
	public DateTime ExpiryTime;
}
