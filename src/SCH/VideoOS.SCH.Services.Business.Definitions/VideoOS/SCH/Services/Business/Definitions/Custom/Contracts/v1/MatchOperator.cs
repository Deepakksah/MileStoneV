using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
public class MatchOperator
{
	[DataMember]
	public string Name;

	[DataMember]
	public string Description;
}
