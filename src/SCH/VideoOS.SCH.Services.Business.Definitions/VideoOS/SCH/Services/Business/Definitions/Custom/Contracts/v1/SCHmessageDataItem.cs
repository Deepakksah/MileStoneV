using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.v1;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:main:common:1")]
public class SCHmessageDataItem
{
	[DataMember]
	public string Key;

	[DataMember]
	public string Value;
}
