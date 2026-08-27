using System.Security.Cryptography;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Encryption;

public class RandomGeneratorUtility
{
	public static byte[] GetBytes(int length)
	{
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		byte[] array = new byte[length];
		rNGCryptoServiceProvider.GetBytes(array);
		return array;
	}

	public static string GetBase64UrlEncodedRandom(int length)
	{
		return StringHelper.GetBase64urlEncodedString(GetBytes(length));
	}
}
