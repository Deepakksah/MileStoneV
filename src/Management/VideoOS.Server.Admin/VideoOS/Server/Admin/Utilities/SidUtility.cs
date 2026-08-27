using System;
using System.Security.Principal;

namespace VideoOS.Server.Admin.Utilities;

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

	public static bool IsRoleIdentity(string sid)
	{
		if (sid.StartsWith("S", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		Guid result;
		return Guid.TryParse(sid, out result);
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
