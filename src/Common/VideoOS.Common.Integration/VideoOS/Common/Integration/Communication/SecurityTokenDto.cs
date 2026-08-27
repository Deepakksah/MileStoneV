using System;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public sealed class SecurityTokenDto
{
	[DataMember]
	public string Token;

	[DataMember]
	public IdentityDto Identity;

	[DataMember]
	public TimeSpan Expires;

	[DataMember]
	public DateTime RegistrationTime;

	public static SecurityTokenDto Create(SecurityTokenDto2 securityTokenDto2)
	{
		if (securityTokenDto2 == null)
		{
			throw new ArgumentNullException("securityTokenDto2");
		}
		return new SecurityTokenDto
		{
			Token = securityTokenDto2.Token,
			Identity = securityTokenDto2.Identity,
			Expires = securityTokenDto2.Expires,
			RegistrationTime = securityTokenDto2.RegistrationTime
		};
	}

	public static SecurityTokenDto Create(SecurityToken securityToken)
	{
		if (securityToken == null)
		{
			throw new ArgumentNullException("securityToken");
		}
		return new SecurityTokenDto
		{
			Token = securityToken.Value,
			Identity = IdentityDto.Create(securityToken.Identity),
			Expires = securityToken.Expires,
			RegistrationTime = securityToken.RegistrationTime
		};
	}

	public static SecurityToken CreateSecurityToken(SecurityTokenDto securityTokenDto)
	{
		if (securityTokenDto == null)
		{
			throw new ArgumentNullException("securityTokenDto");
		}
		return new SecurityToken(IdentityDto.CreateIdentity(securityTokenDto.Identity), securityTokenDto.Token, securityTokenDto.RegistrationTime, securityTokenDto.Expires);
	}
}
