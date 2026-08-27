using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
public class SCHclient
{
	[DataMember]
	public string ClientName;

	[DataMember]
	public string[] MemberGroups;
}
