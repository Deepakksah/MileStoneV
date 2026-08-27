using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagSimpleKVPair
{
	[DataMember]
	public string Key;

	[DataMember]
	public string Value;
}
