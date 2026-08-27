using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public sealed class SecurityTokenDto2
{
	[Required]
	[DataMember]
	public string Token { get; set; }

	[Required]
	[DataMember]
	public IdentityDto Identity { get; set; }

	[Required]
	[DataMember]
	public TimeSpan Expires { get; set; }

	[Required]
	[DataMember]
	public DateTime RegistrationTime { get; set; }

	[DataMember]
	public bool ManagementClientToken { get; set; }

	public static SecurityTokenDto2 Create(SecurityToken securityToken)
	{
		if (securityToken == null)
		{
			throw new ArgumentNullException("securityToken");
		}
		return new SecurityTokenDto2
		{
			Token = securityToken.Value,
			Identity = IdentityDto.Create(securityToken.Identity),
			Expires = securityToken.Expires,
			RegistrationTime = securityToken.RegistrationTime,
			ManagementClientToken = securityToken.ManagementClientToken
		};
	}

	public static SecurityToken CreateSecurityToken(SecurityTokenDto2 securityTokenDto2)
	{
		if (securityTokenDto2 == null)
		{
			throw new ArgumentNullException("securityTokenDto2");
		}
		return new SecurityToken(IdentityDto.CreateIdentity(securityTokenDto2.Identity), securityTokenDto2.Token, securityTokenDto2.RegistrationTime, securityTokenDto2.Expires, securityTokenDto2.ManagementClientToken);
	}
}
