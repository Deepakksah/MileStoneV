using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:common")]
public class ErrorItem
{
	[DataMember]
	public int ErrorNumber;

	[DataMember]
	public string ErrorDescription;
}
