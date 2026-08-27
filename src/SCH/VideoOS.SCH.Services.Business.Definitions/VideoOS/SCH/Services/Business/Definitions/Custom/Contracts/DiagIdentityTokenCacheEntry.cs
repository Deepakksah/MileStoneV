using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagIdentityTokenCacheEntry
{
	[DataMember]
	public bool ValidToken;

	[DataMember]
	public string IdentityToken;

	[DataMember]
	public DiagIdentityTokenInfo TokenInfo;

	[DataMember]
	public bool PendingUpdate;

	[DataMember]
	public DateTime ExpiryTime;

	[DataMember]
	public DateTime LatestIdleTime;
}
