using System;
using System.Numerics;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.Encryption;

public class DHEncryption
{
	private readonly DHKeyGenerator _keygen = new DHKeyGenerator();

	private readonly DHKeyPair _keypair;

	private PBE _pbe;

	private readonly EncodingPadding _padding;

	private readonly PrimeLength _primeLength;

	public DHEncryption(EncodingPadding padding = EncodingPadding.Pkcs7, PrimeLength primeLength = PrimeLength.Prime1024)
	{
		_padding = padding;
		_primeLength = primeLength;
		_keygen.Initialize(primeLength);
		_keypair = _keygen.GenerateKeyPair();
	}

	public DHEncryption(string publicKey, string privateKey, EncodingPadding padding = EncodingPadding.Pkcs7, PrimeLength primeLength = PrimeLength.Prime1024)
	{
		_padding = padding;
		_primeLength = primeLength;
		_keygen.Initialize(primeLength);
		_keypair = new DHKeyPair(new BigInteger(Convert.FromBase64String(privateKey)), new BigInteger(Convert.FromBase64String(publicKey)));
	}

	public string GetPublicKey()
	{
		return Convert.ToBase64String(_keypair.GetPublicKey().ToByteArray());
	}

	public string GetPrivateKey()
	{
		return Convert.ToBase64String(_keypair.GetPrivateKey().ToByteArray());
	}

	public void GenerateSecretKey(string publicKey)
	{
		if (publicKey.Length > DhPublicKeyHelper.GetMaxPublicKeyLength(_primeLength))
		{
			throw new DHEncryptionException("Other public key length is bigger that max possible size");
		}
		BigInteger publicKey2 = new BigInteger(Convert.FromBase64String(publicKey));
		_pbe = new PBE(_keygen.GetSecretKeyBytes(PBE.KeybytesSize, publicKey2, _keypair.GetPrivateKey()))
		{
			Padding = _padding
		};
	}

	public byte[] GetSecretKey()
	{
		return _pbe.GetSecretKey();
	}

	public byte[] GetSecretKeyFull()
	{
		return _keygen.GetFullSecretKeyBytes();
	}

	public string Encrypt(string text)
	{
		return _pbe.GetEncryptedValue(text);
	}

	public string Decrypt(string text)
	{
		return _pbe.GetDecryptedValue(text);
	}
}
