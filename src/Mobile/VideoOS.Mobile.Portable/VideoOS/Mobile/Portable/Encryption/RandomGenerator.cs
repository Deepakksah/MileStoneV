using System.Security.Cryptography;

namespace VideoOS.Mobile.Portable.Encryption;

public class RandomGenerator : IRandomGenerator
{
	public void GetBytes(byte[] data)
	{
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		rNGCryptoServiceProvider.GetBytes(data);
	}
}
