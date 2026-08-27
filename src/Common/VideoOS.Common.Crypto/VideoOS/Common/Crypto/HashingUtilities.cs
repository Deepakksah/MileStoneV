using System;
using System.Security.Cryptography;

namespace VideoOS.Common.Crypto;

public static class HashingUtilities
{
	private static byte[] CreateGuidHash(string guid)
	{
		if (guid == null)
		{
			throw new ArgumentNullException("guid");
		}
		Guid guid2;
		try
		{
			guid2 = new Guid(guid);
		}
		catch (FormatException)
		{
			throw new ArgumentException("The format of 'guid' is invalid.");
		}
		catch (OverflowException)
		{
			throw new ArgumentException("The format of 'guid' is invalid.");
		}
		byte[] buffer = guid2.ToByteArray();
		using HashAlgorithm hashAlgorithm = new SHA256Managed();
		return hashAlgorithm.ComputeHash(buffer);
	}

	public static string CreateGuidHashString(string guid)
	{
		return Convert.ToBase64String(CreateGuidHash(guid));
	}
}
