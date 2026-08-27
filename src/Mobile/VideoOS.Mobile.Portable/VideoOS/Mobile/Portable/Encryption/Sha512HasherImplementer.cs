using System.Security.Cryptography;

namespace VideoOS.Mobile.Portable.Encryption;

public class Sha512HasherImplementer : ISha512Hasher
{
	private readonly SHA512 _sha512Encoder = SHA512.Create();

	private readonly object _sha512LockObject = new object();

	public byte[] ComputeHash(byte[] buffer)
	{
		lock (_sha512LockObject)
		{
			return _sha512Encoder.ComputeHash(buffer);
		}
	}
}
