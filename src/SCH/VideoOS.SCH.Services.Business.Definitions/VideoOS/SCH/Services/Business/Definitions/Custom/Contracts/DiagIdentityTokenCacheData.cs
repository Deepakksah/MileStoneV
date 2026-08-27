using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagIdentityTokenCacheData
{
	[DataMember]
	public DateTime LastPurgeTime;

	[DataMember]
	public DiagIdentityTokenCacheEntry[] CachedEntries;
}
