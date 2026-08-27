using System;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public sealed class SecurityTokenWithServerTimeDto
{
	[DataMember]
	public SecurityTokenDto SecurityToken;

	private DateTime ServerTime;

	public static SecurityTokenWithServerTimeDto Create(SecurityToken securityToken)
	{
		return new SecurityTokenWithServerTimeDto
		{
			SecurityToken = ((securityToken == null) ? null : SecurityTokenDto.Create(securityToken)),
			ServerTime = DateTime.UtcNow
		};
	}

	public static SecurityToken CreateSecurityToken(SecurityTokenWithServerTimeDto securityTokenWithServerTimeDto)
	{
		if (securityTokenWithServerTimeDto == null)
		{
			throw new ArgumentNullException("securityTokenWithServerTimeDto");
		}
		if (securityTokenWithServerTimeDto.SecurityToken != null)
		{
			TimeSpan timeSpan = DateTime.UtcNow - securityTokenWithServerTimeDto.ServerTime;
			securityTokenWithServerTimeDto.SecurityToken.RegistrationTime += timeSpan;
			return SecurityTokenDto.CreateSecurityToken(securityTokenWithServerTimeDto.SecurityToken);
		}
		return null;
	}
}
