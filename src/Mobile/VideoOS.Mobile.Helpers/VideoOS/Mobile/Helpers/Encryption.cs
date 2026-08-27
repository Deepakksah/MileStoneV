using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Mobile.Helpers;

public static class Encryption
{
	public enum Operation
	{
		Encrypt,
		Decrypt
	}

	public class PBE
	{
		private const int KEY_SIZE = 32;

		private const int IV_SIZE = 16;

		private const int KEYBYTES_SIZE = 48;

		private byte[] IV = new byte[16];

		private byte[] Key = new byte[32];

		public PBE(byte[] keyBytes)
		{
			if (keyBytes.Length < 48)
			{
				throw new CryptographicException("Invalid keyBytes size!");
			}
			Array.Copy(keyBytes, 0, IV, 0, 16);
			Array.Copy(keyBytes, 16, Key, 0, 32);
		}

		public string GetEncryptedValue(string inputValue)
		{
			using Aes aes = GetCryptoProvider();
			using MemoryStream memoryStream = new MemoryStream();
			using ICryptoTransform transform = aes.CreateEncryptor();
			using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			byte[] bytes = new UTF8Encoding().GetBytes(inputValue);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] inArray = memoryStream.ToArray();
			cryptoStream.Close();
			memoryStream.Close();
			return Convert.ToBase64String(inArray);
		}

		public string GetDecryptedValue(string inputValue)
		{
			using Aes aes = GetCryptoProvider();
			byte[] array = Convert.FromBase64String(inputValue);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.FlushFinalBlock();
			cryptoStream.Close();
			return new UTF8Encoding().GetString(memoryStream.ToArray());
		}

		private Aes GetCryptoProvider()
		{
			Aes aes = Aes.Create();
			aes.Padding = PaddingMode.PKCS7;
			aes.Mode = CipherMode.CBC;
			aes.KeySize = 256;
			aes.IV = IV;
			aes.Key = Key;
			return aes;
		}
	}

	public static string GetValue(string value, Operation operation)
	{
		string result = null;
		try
		{
			string s = "D47A8F8140F84AC28B374B596ED3E6B4419EFE15EF1D1850";
			PBE pBE = new PBE(Encoding.UTF8.GetBytes(s));
			switch (operation)
			{
			case Operation.Decrypt:
				result = pBE.GetDecryptedValue(value);
				break;
			case Operation.Encrypt:
				result = pBE.GetEncryptedValue(value);
				break;
			}
		}
		catch (Exception)
		{
		}
		return result;
	}
}
