using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class GetServerInfoMsgOut
{
	[DataMember]
	public OperationStatus CurrentStatus;

	[DataMember]
	public DateTime ServerUtcDateTime;

	[DataMember]
	public string ServerTimeZone;

	[DataMember]
	public string ServerName;

	[DataMember]
	public string osVersion;

	[DataMember]
	public string clrRuntimeVersion;

	[DataMember]
	public DiagSimpleKVPair[] InfoDetails;
}
