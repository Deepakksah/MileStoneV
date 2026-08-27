using System;
using VideoOS.Common.Crypto;

namespace VideoOS.Common.Integration;

public sealed class CorporateEncryptionSymmetric : ISymmetricEncryption
{
	private string _key;

	public byte[] Key => Convert.FromBase64String(_key);

	public CorporateEncryptionSymmetric(string key)
	{
		_key = key;
	}

	public string Decrypt(byte[] value)
	{
		if (value != null && value.Length != 0)
		{
			return EncryptionFactory.CreateSymmetric(_key).Decrypt(value);
		}
		return string.Empty;
	}

	public string DecryptFromBase64String(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			return EncryptionFactory.CreateSymmetric(_key).DecryptFromBase64String(value);
		}
		return value;
	}

	public byte[] DecryptSequence(byte[] value)
	{
		if (value != null && value.Length != 0)
		{
			return EncryptionFactory.CreateSymmetric(_key).DecryptSequence(value);
		}
		return value;
	}

	public byte[] Encrypt(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			return EncryptionFactory.CreateSymmetric(_key).Encrypt(value);
		}
		return Array.Empty<byte>();
	}

	public byte[] EncryptSequence(byte[] value)
	{
		if (value != null && value.Length != 0)
		{
			return EncryptionFactory.CreateSymmetric(_key).EncryptSequence(value);
		}
		return value;
	}

	public string EncryptToBase64String(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			return EncryptionFactory.CreateSymmetric(_key).EncryptToBase64String(value);
		}
		return value;
	}
}
