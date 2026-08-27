using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagMatchExpression
{
	[DataMember]
	public DiagSimpleKVPair Data;

	[DataMember]
	public string OperatorName;
}
