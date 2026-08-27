using System;
using System.Security.Principal;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public static class SidUtility
{
	private const ulong identifierAuthorityResourceManagerAuthority = 9uL;

	private static uint subAuthority0 = 2374562217u;

	public static SecurityIdentifier NewBasicUserSid(Guid domainId)
	{
		uint[] array = new uint[10];
		FillDomainSid(array, domainId);
		array[5] = 1u;
		FillSidRid(array, Guid.NewGuid());
		return new SecurityIdentifier(EncodeSid(9uL, array), 0);
	}

	private static void FillDomainSid(uint[] subAuthorities, Guid domainId)
	{
		subAuthorities[0] = subAuthority0;
		byte[] array = domainId.ToByteArray();
		for (int i = 0; i < 4; i++)
		{
			subAuthorities[i + 1] = ArrayToInt(array, i * 4);
		}
	}

	private static void FillSidRid(uint[] sidArray, Guid ridId)
	{
		byte[] array = ridId.ToByteArray();
		for (int i = 0; i < 4; i++)
		{
			sidArray[i + 6] = ArrayToInt(array, i * 4);
		}
	}

	private static uint ArrayToInt(byte[] array, int index)
	{
		return (uint)(0 | array[index + 3] | (array[index + 2] << 8) | (array[index + 1] << 16) | (array[index] << 24));
	}

	public static uint[] DecodeSid(byte[] binarySid, out int revision, out ulong authority)
	{
		revision = binarySid[0];
		int num = binarySid[1];
		authority = (ulong)((binarySid[2] << 8) + binarySid[3] + (binarySid[4] << 24) + (binarySid[5] << 16) + (binarySid[6] << 8) + binarySid[7]);
		uint[] array = new uint[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (uint)(binarySid[8 + 4 * i] + (binarySid[9 + 4 * i] << 8) + (binarySid[10 + 4 * i] << 16) + (binarySid[11 + 4 * i] << 24));
		}
		return array;
	}

	public static byte[] EncodeSid(ulong authority, uint[] subAuthorities)
	{
		byte[] array = new byte[8 + subAuthorities.Length * 4];
		array[0] = 1;
		array[1] = (byte)subAuthorities.Length;
		for (int i = 0; i < 6; i++)
		{
			array[2 + i] = (byte)((authority >> (5 - i) * 8) & 0xFF);
		}
		for (int j = 0; j < subAuthorities.Length; j++)
		{
			for (int k = 0; k < 4; k++)
			{
				array[8 + 4 * j + k] = (byte)((subAuthorities[j] >> k * 8) & 0xFF);
			}
		}
		return array;
	}

	public static byte[] GetDomainSid(byte[] binarySid)
	{
		int revision;
		ulong authority;
		uint[] array = DecodeSid(binarySid, out revision, out authority);
		if (authority != 5)
		{
			throw new ArgumentException("Domain SID can only be used on Windows SIDs");
		}
		uint[] subAuthorities = new uint[4]
		{
			array[0],
			array[1],
			array[2],
			array[3]
		};
		return EncodeSid(authority, subAuthorities);
	}

	public static bool IsCorporateIdentity(string sid)
	{
		if (sid.StartsWith("S", StringComparison.OrdinalIgnoreCase))
		{
			return IsLegacyBasicUser(sid);
		}
		Guid result;
		return Guid.TryParse(sid, out result);
	}

	public static bool IsCorporateIdentity(IGeminiRequestContext requestContext, string sid, out IdentityType type)
	{
		Guid result;
		if (sid.StartsWith("S", StringComparison.OrdinalIgnoreCase))
		{
			if (IsLegacyBasicUser(sid))
			{
				type = IdentityType.BasicUser;
				return true;
			}
		}
		else if (Guid.TryParse(sid, out result))
		{
			if (IsGuidOfType(requestContext, sid, IdentityType.Role))
			{
				type = IdentityType.Role;
			}
			else
			{
				type = IdentityType.BasicUser;
			}
			return true;
		}
		type = IdentityType.InvalidIdentity;
		return false;
	}

	private static bool IsLegacyBasicUser(string sid)
	{
		SecurityIdentifier securityIdentifier = new SecurityIdentifier(sid);
		byte[] array = new byte[securityIdentifier.BinaryLength];
		securityIdentifier.GetBinaryForm(array, 0);
		int revision;
		ulong authority;
		uint[] array2 = DecodeSid(array, out revision, out authority);
		if (authority == 9)
		{
			return array2[0] == subAuthority0;
		}
		return false;
	}

	private static bool IsGuidOfType(IGeminiRequestContext requestContext, string guid, IdentityType expectedType)
	{
		Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, guid, QueryMembership.None);
		if (identity != null && identity.Type == expectedType)
		{
			return true;
		}
		return false;
	}

	public static SecurityIdentifier ResolveSecurityIdentifier(string sid, ref string domainName, ref string accountName, ref AccountType accountType)
	{
		SecurityIdentifier securityIdentifier = new SecurityIdentifier(sid);
		byte[] array = new byte[securityIdentifier.BinaryLength];
		securityIdentifier.GetBinaryForm(array, 0);
		accountName = NativeMethods.SidToName(array, out domainName, out accountType);
		return securityIdentifier;
	}

	public static bool IsBuiltinAccount(string sid)
	{
		SecurityIdentifier securityIdentifier = new SecurityIdentifier(sid);
		byte[] array = new byte[securityIdentifier.BinaryLength];
		securityIdentifier.GetBinaryForm(array, 0);
		int revision;
		ulong authority;
		uint[] array2 = DecodeSid(array, out revision, out authority);
		if (authority != 5)
		{
			return false;
		}
		if (array2.Length != 0)
		{
			return array2[0] == 32;
		}
		return false;
	}

	public static bool IsNTAccount(string sid)
	{
		SecurityIdentifier securityIdentifier = new SecurityIdentifier(sid);
		byte[] array = new byte[securityIdentifier.BinaryLength];
		securityIdentifier.GetBinaryForm(array, 0);
		int revision;
		ulong authority;
		uint[] array2 = DecodeSid(array, out revision, out authority);
		if (authority != 5)
		{
			return false;
		}
		if (array2.Length != 0)
		{
			return array2[0] == 21;
		}
		return false;
	}
}
