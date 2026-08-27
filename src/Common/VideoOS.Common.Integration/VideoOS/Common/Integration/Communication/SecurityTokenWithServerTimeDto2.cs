using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
[JsonObject(/*Could not decode attribute arguments.*/)]
public sealed class SecurityTokenWithServerTimeDto2
{
	private DateTime ServerTime;

	[DataMember]
	public SecurityTokenDto2 SecurityToken { get; set; }

	public static SecurityTokenWithServerTimeDto2 Create(SecurityToken securityToken)
	{
		return new SecurityTokenWithServerTimeDto2
		{
			SecurityToken = ((securityToken == null) ? null : SecurityTokenDto2.Create(securityToken)),
			ServerTime = DateTime.UtcNow
		};
	}

	public static SecurityToken CreateSecurityToken(SecurityTokenWithServerTimeDto2 securityTokenWithServerTimeDto2)
	{
		if (securityTokenWithServerTimeDto2 == null)
		{
			throw new ArgumentNullException("securityTokenWithServerTimeDto2");
		}
		if (securityTokenWithServerTimeDto2.SecurityToken != null)
		{
			TimeSpan timeSpan = DateTime.UtcNow - securityTokenWithServerTimeDto2.ServerTime;
			securityTokenWithServerTimeDto2.SecurityToken.RegistrationTime += timeSpan;
			return SecurityTokenDto2.CreateSecurityToken(securityTokenWithServerTimeDto2.SecurityToken);
		}
		return null;
	}
}
