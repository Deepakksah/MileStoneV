using System;
using System.IO;
using System.Security.Cryptography;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public static class SecureFile
{
	private const int AesKeySize = 32;

	private const int AesIvSize = 16;

	public static void Read(string fileName, byte[] encryptionKey, Action<Stream> reader)
	{
		byte[] aesKey = GetAesKey(encryptionKey);
		using FileStream fileStream = new FileStream(fileName, FileMode.Open);
		byte[] array = new byte[16];
		fileStream.Read(array, 0, 16);
		using (Aes aes = GetCryptoProvider(array, aesKey))
		{
			using ICryptoTransform transform = aes.CreateDecryptor();
			using CryptoStream obj = new CryptoStream(fileStream, transform, CryptoStreamMode.Read);
			reader(obj);
		}
		fileStream.Close();
	}

	public static void WriteBytes(string fileName, FileMode mode, byte[] encryptionKey, byte[] buffer)
	{
		Write(fileName, mode, encryptionKey, delegate(Stream stream)
		{
			stream.Write(buffer, 0, buffer.Length);
		});
	}

	public static void Write(string fileName, FileMode mode, byte[] encryptionKey, Action<Stream> writer)
	{
		byte[] aesIv = GetAesIv();
		byte[] aesKey = GetAesKey(encryptionKey);
		try
		{
			using FileStream fileStream = new FileStream(fileName, mode);
			fileStream.Write(aesIv, 0, aesIv.Length);
			using (Aes aes = GetCryptoProvider(aesIv, aesKey))
			{
				using ICryptoTransform transform = aes.CreateEncryptor();
				using CryptoStream obj = new CryptoStream(fileStream, transform, CryptoStreamMode.Write);
				writer(obj);
			}
			fileStream.Close();
		}
		catch (Exception)
		{
			try
			{
				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
			throw;
		}
	}

	private static byte[] GetAesIv()
	{
		byte[] array = new byte[16];
		new RandomGenerator().GetBytes(array);
		return array;
	}

	private static byte[] GetAesKey(byte[] encryptionKey)
	{
		byte[] array = new byte[32];
		Array.Copy(encryptionKey, array, encryptionKey.Length);
		if (encryptionKey.Length < 32)
		{
			Array.Clear(array, encryptionKey.Length, 32 - encryptionKey.Length);
		}
		return array;
	}

	private static Aes GetCryptoProvider(byte[] iv, byte[] key)
	{
		Aes aes = Aes.Create();
		aes.Padding = PaddingMode.ISO10126;
		aes.Mode = CipherMode.CBC;
		aes.KeySize = 256;
		aes.IV = iv;
		aes.Key = key;
		return aes;
	}
}
