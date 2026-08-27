using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagIdentityTokenInfo
{
	[DataMember]
	public string IdentityToken;

	[DataMember]
	public string AccountName;

	[DataMember]
	public string DomainName;

	[DataMember]
	public string Sid;
}
