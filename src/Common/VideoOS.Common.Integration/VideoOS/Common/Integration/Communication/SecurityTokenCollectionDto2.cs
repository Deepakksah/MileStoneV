using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public sealed class SecurityTokenCollectionDto2
{
	[DataMember]
	private SecurityTokenDto2[] securityTokenDtos;

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

	public static SecurityTokenCollectionDto2 Create(SecurityToken securityToken)
	{
		return Create(new SecurityToken[1] { securityToken });
	}

	public static SecurityTokenCollectionDto2 Create(SecurityToken[] securityTokens)
	{
		SecurityTokenCollectionDto2 securityTokenCollectionDto = new SecurityTokenCollectionDto2();
		List<SecurityTokenDto2> list = new List<SecurityTokenDto2>();
		if (securityTokens != null)
		{
			for (int i = 0; i < securityTokens.Length; i++)
			{
				SecurityTokenDto2 item = SecurityTokenDto2.Create(securityTokens[i]);
				list.Add(item);
			}
		}
		securityTokenCollectionDto.securityTokenDtos = list.ToArray();
		return securityTokenCollectionDto;
	}

	public static SecurityToken[] GetSecurityTokens(SecurityTokenCollectionDto2 securityTokenCollectionDto2)
	{
		if (securityTokenCollectionDto2 == null)
		{
			throw new ArgumentNullException("securityTokenCollectionDto2");
		}
		List<SecurityToken> list = new List<SecurityToken>();
		SecurityTokenDto2[] array = securityTokenCollectionDto2.securityTokenDtos;
		for (int i = 0; i < array.Length; i++)
		{
			SecurityToken item = SecurityTokenDto2.CreateSecurityToken(array[i]);
			list.Add(item);
		}
		return list.ToArray();
	}
}
