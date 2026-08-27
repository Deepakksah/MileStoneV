using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Crypto;

public class AesSymmetricEncryption : ISymmetricEncryption
{
	private readonly byte[] _key;

	public byte[] Key => _key;

	public AesSymmetricEncryption(byte[] key)
	{
		if (!IsValidKeySize(key))
		{
			throw new ArgumentException("Invalid Aes Key size.");
		}
		_key = key ?? throw new ArgumentNullException("key");
	}

	public string EncryptToBase64String(string value)
	{
		return Convert.ToBase64String(Encrypt(value));
	}

	public byte[] Encrypt(string value)
	{
		return EncryptSequence(Encoding.Unicode.GetBytes(value));
	}

	public byte[] EncryptSequence(byte[] value)
	{
		using AesCng aesCng = new AesCng();
		using MemoryStream memoryStream = new MemoryStream(value);
		using MemoryStream memoryStream2 = new MemoryStream();
		aesCng.GenerateIV();
		byte[] iV = aesCng.IV;
		using (ICryptoTransform transform = aesCng.CreateEncryptor(_key, iV))
		{
			using CryptoStream destination = new CryptoStream(memoryStream2, transform, CryptoStreamMode.Write);
			memoryStream.CopyTo(destination);
		}
		byte[] cipherText = memoryStream2.ToArray();
		return PackOutput(iV, cipherText);
	}

	public static byte[] GenerateKey()
	{
		using AesCng aesCng = new AesCng();
		return aesCng.Key;
	}

	private static byte[] PackOutput(byte[] iv, byte[] cipherText)
	{
		byte[] array = new byte[iv.Length + cipherText.Length];
		Array.Copy(iv, 0, array, 0, iv.Length);
		Array.Copy(cipherText, 0, array, iv.Length, cipherText.Length);
		return array;
	}

	public string DecryptFromBase64String(string encodedValue)
	{
		if (Base64StringValidator.IsValidBase64String(encodedValue))
		{
			return Decrypt(Convert.FromBase64String(encodedValue));
		}
		return encodedValue;
	}

	public byte[] DecryptSequence(byte[] value)
	{
		using AesCng aesCng = new AesCng();
		var (rgbIV, buffer) = UnpackOutput(value, aesCng.BlockSize / 8);
		using ICryptoTransform transform = aesCng.CreateDecryptor(_key, rgbIV);
		using MemoryStream stream = new MemoryStream(buffer);
		using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		using MemoryStream memoryStream = new MemoryStream();
		cryptoStream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

	public string Decrypt(byte[] value)
	{
		using AesCng aesCng = new AesCng();
		var (rgbIV, buffer) = UnpackOutput(value, aesCng.BlockSize / 8);
		using ICryptoTransform transform = aesCng.CreateDecryptor(_key, rgbIV);
		using MemoryStream stream = new MemoryStream(buffer);
		using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
		using StreamReader streamReader = new StreamReader(stream2, Encoding.Unicode);
		return streamReader.ReadToEnd();
	}

	private (byte[] IV, byte[] CipherText) UnpackOutput(byte[] blob, int ivSizeInBytes)
	{
		byte[] array = new byte[ivSizeInBytes];
		int num = blob.Length - ivSizeInBytes;
		byte[] array2 = new byte[num];
		Array.Copy(blob, 0, array, 0, ivSizeInBytes);
		Array.Copy(blob, ivSizeInBytes, array2, 0, num);
		return (IV: array, CipherText: array2);
	}

	private bool IsValidKeySize(byte[] key)
	{
		using AesCng aesCng = new AesCng();
		return aesCng.ValidKeySize(key.Length * 8);
	}
}
