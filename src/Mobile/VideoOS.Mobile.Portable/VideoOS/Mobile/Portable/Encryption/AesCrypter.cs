using System;
using System.IO;
using System.Security.Cryptography;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.Encryption;

public class AesCrypter : IAesCrypter
{
	private readonly int _keyLen;

	private readonly byte[] _key;

	private readonly int _ivLen;

	private readonly byte[] _iv;

	private PaddingMode _padding = PaddingMode.PKCS7;

	public EncodingPadding Padding
	{
		get
		{
			return _padding switch
			{
				PaddingMode.PKCS7 => EncodingPadding.Pkcs7, 
				PaddingMode.ISO10126 => EncodingPadding.Iso10126, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}
		set
		{
			_padding = value switch
			{
				EncodingPadding.Pkcs7 => PaddingMode.PKCS7, 
				EncodingPadding.Iso10126 => PaddingMode.ISO10126, 
				_ => PaddingMode.PKCS7, 
			};
		}
	}

	public AesCrypter(byte[] key, byte[] iv)
	{
		_key = key;
		_keyLen = _key.Length;
		_iv = iv;
		_ivLen = _iv.Length;
	}

	public byte[] Encrypt(byte[] input)
	{
		using AesCryptoServiceProvider aesCryptoServiceProvider = GetCryptoProvider();
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, aesCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(input, 0, input.Length);
		cryptoStream.FlushFinalBlock();
		byte[] result = memoryStream.ToArray();
		cryptoStream.Close();
		memoryStream.Close();
		aesCryptoServiceProvider.Dispose();
		return result;
	}

	public byte[] Decrypt(byte[] input)
	{
		using AesCryptoServiceProvider aesCryptoServiceProvider = GetCryptoProvider();
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, aesCryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
		cryptoStream.Write(input, 0, input.Length);
		cryptoStream.FlushFinalBlock();
		byte[] result = memoryStream.ToArray();
		cryptoStream.Close();
		memoryStream.Close();
		return result;
	}

	private AesCryptoServiceProvider GetCryptoProvider()
	{
		return new AesCryptoServiceProvider
		{
			Padding = _padding,
			Mode = CipherMode.CBC,
			KeySize = 8 * _keyLen,
			IV = _iv,
			Key = _key
		};
	}
}
