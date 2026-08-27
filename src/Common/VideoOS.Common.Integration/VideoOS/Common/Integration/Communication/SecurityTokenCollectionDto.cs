using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public sealed class SecurityTokenCollectionDto
{
	private SecurityTokenDto[] securityTokenDtos;

	private long lastUpdate;

	private long nextUpdate;

	[DataMember]
	public long LastUpdate
	{
		[DebuggerStepThrough]
		get
		{
			return lastUpdate;
		}
		[DebuggerStepThrough]
		set
		{
			lastUpdate = value;
		}
	}

	[DataMember]
	public long NextUpdate
	{
		[DebuggerStepThrough]
		get
		{
			return nextUpdate;
		}
		[DebuggerStepThrough]
		set
		{
			nextUpdate = value;
		}
	}

	public static SecurityTokenCollectionDto Create(SecurityTokenCollectionDto2 securityTokenCollectionDto2)
	{
		SecurityTokenCollectionDto securityTokenCollectionDto3 = Create(SecurityTokenCollectionDto2.GetSecurityTokens(securityTokenCollectionDto2));
		securityTokenCollectionDto3.LastUpdate = securityTokenCollectionDto2.LastUpdate;
		securityTokenCollectionDto3.NextUpdate = securityTokenCollectionDto2.NextUpdate;
		return securityTokenCollectionDto3;
	}

	public static SecurityTokenCollectionDto Create(SecurityToken securityToken)
	{
		return Create(new SecurityToken[1] { securityToken });
	}

	public static SecurityTokenCollectionDto Create(SecurityToken[] securityTokens)
	{
		SecurityTokenCollectionDto securityTokenCollectionDto = new SecurityTokenCollectionDto();
		List<SecurityTokenDto> list = new List<SecurityTokenDto>();
		if (securityTokens != null)
		{
			for (int i = 0; i < securityTokens.Length; i++)
			{
				SecurityTokenDto item = SecurityTokenDto.Create(securityTokens[i]);
				list.Add(item);
			}
		}
		securityTokenCollectionDto.securityTokenDtos = list.ToArray();
		return securityTokenCollectionDto;
	}

	public static SecurityToken[] GetSecurityTokens(SecurityTokenCollectionDto securityTokenCollectionDto)
	{
		if (securityTokenCollectionDto == null)
		{
			throw new ArgumentNullException("securityTokenCollectionDto");
		}
		List<SecurityToken> list = new List<SecurityToken>();
		SecurityTokenDto[] array = securityTokenCollectionDto.securityTokenDtos;
		for (int i = 0; i < array.Length; i++)
		{
			SecurityToken item = SecurityTokenDto.CreateSecurityToken(array[i]);
			list.Add(item);
		}
		return list.ToArray();
	}
}
