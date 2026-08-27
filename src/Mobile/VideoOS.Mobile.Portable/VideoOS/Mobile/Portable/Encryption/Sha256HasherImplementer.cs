using System.Security.Cryptography;
using System.Text;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.Encryption;

public class Sha256HasherImplementer : ISha256Hasher
{
	private readonly SHA256 _sha256Encoder = SHA256.Create();

	private readonly object _sha256LockObject = new object();

	public byte[] ComputeHash(byte[] buffer)
	{
		lock (_sha256LockObject)
		{
			return _sha256Encoder.ComputeHash(buffer);
		}
	}

	public string ComputeHash(string value)
	{
		lock (_sha256LockObject)
		{
			return StringHelper.GetBase64urlEncodedString(_sha256Encoder.ComputeHash(Encoding.ASCII.GetBytes(value)));
		}
	}
}
