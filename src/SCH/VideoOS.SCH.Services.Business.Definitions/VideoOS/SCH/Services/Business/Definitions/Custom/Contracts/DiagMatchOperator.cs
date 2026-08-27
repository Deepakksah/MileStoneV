using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagMatchOperator
{
	[DataMember]
	public string Name;

	[DataMember]
	public string Description;
}
