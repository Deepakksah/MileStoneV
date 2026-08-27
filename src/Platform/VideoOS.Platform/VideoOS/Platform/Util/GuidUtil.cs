using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Platform.Util;

public static class GuidUtil
{
	public static Guid Create(Guid namespaceId, IList<Guid> guidIds)
	{
		if (guidIds == null || !guidIds.Any())
		{
			throw new ArgumentNullException("guidIds");
		}
		byte[] array = new byte[guidIds.Count * 16];
		for (int i = 0; i < guidIds.Count; i++)
		{
			guidIds[i].ToByteArray().CopyTo(array, i * 16);
		}
		return BytesToGuid(namespaceId, array);
	}

	public static Guid Create(Guid namespaceId, params string[] names)
	{
		if (names == null || names.Length == 0)
		{
			throw new ArgumentNullException("names");
		}
		string s = string.Join(":", names);
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		return BytesToGuid(namespaceId, bytes);
	}

	private static Guid BytesToGuid(Guid namespaceId, byte[] bytes)
	{
		byte[] array = namespaceId.ToByteArray();
		SwapByteOrder(array);
		byte[] hash;
		using (HashAlgorithm hashAlgorithm = SHA1.Create())
		{
			hashAlgorithm.TransformBlock(array, 0, array.Length, null, 0);
			hashAlgorithm.TransformFinalBlock(bytes, 0, bytes.Length);
			hash = hashAlgorithm.Hash;
		}
		byte[] array2 = new byte[16];
		Array.Copy(hash, 0, array2, 0, 16);
		array2[6] = (byte)((array2[6] & 0xF) | 0x50);
		array2[8] = (byte)((array2[8] & 0x3F) | 0x80);
		SwapByteOrder(array2);
		return new Guid(array2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void SwapByteOrder(byte[] byteArray)
	{
		fixed (byte* ptr = &byteArray[0])
		{
			byte b = *ptr;
			*ptr = ptr[3];
			ptr[3] = b;
			b = ptr[1];
			ptr[1] = ptr[2];
			ptr[2] = b;
			b = ptr[4];
			ptr[4] = ptr[5];
			ptr[5] = b;
			b = ptr[6];
			ptr[6] = ptr[7];
			ptr[7] = b;
		}
	}
}
