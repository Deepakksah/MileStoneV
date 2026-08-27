using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagMessage
{
	[DataMember]
	public int ID;

	[DataMember]
	public string MessageTypeName;

	[DataMember]
	public DateTime Created;

	[DataMember]
	public DiagSimpleKVPair[] Data;
}
