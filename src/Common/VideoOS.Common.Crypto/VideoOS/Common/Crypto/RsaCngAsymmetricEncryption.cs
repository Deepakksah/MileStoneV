using System;
using System.Security.Cryptography;

namespace VideoOS.Common.Crypto;

public class RsaCngAsymmetricEncryption : IAsymmetricEncryption, IDisposable
{
	private readonly RSACng _cryptoServiceProvider;

	private readonly RSAEncryptionPadding _encryptionPadding;

	private string _keyInformation;

	private string _publicKey;

	private bool _disposed;

	public string KeyInformationAsBase64
	{
		get
		{
			if (_keyInformation == null)
			{
				_keyInformation = Convert.ToBase64String(_cryptoServiceProvider.Key.Export(CngKeyBlobFormat.GenericPrivateBlob));
			}
			return _keyInformation;
		}
	}

	public string PublicKeyAsBase64
	{
		get
		{
			if (_publicKey == null)
			{
				_publicKey = Convert.ToBase64String(_cryptoServiceProvider.Key.Export(CngKeyBlobFormat.GenericPublicBlob));
			}
			return _publicKey;
		}
	}

	public RsaCngAsymmetricEncryption(RSAEncryptionPadding padding)
	{
		_encryptionPadding = padding ?? throw new ArgumentNullException("padding");
		_cryptoServiceProvider = new RSACng();
	}

	public RsaCngAsymmetricEncryption(CngKey key, RSAEncryptionPadding padding)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		_encryptionPadding = padding ?? throw new ArgumentNullException("padding");
		_cryptoServiceProvider = new RSACng(key);
	}

	public byte[] Encrypt(byte[] data)
	{
		return _cryptoServiceProvider.Encrypt(data, _encryptionPadding);
	}

	public byte[] Decrypt(byte[] data)
	{
		return _cryptoServiceProvider.Decrypt(data, _encryptionPadding);
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_cryptoServiceProvider.Clear();
			_cryptoServiceProvider.Dispose();
			_disposed = true;
		}
	}
}
