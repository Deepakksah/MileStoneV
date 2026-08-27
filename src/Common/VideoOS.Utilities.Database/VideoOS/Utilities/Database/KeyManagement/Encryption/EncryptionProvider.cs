using System;
using System.Security.Cryptography;
using VideoOS.Common.Crypto;

namespace VideoOS.Utilities.Database.KeyManagement.Encryption;

public class EncryptionProvider : IEncryptionProvider
{
	private const int SaltSizeInBytes = 8;

	public byte[] EncryptKey(string password, byte[] key)
	{
		byte[] array = new byte[8];
		using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
		{
			rNGCryptoServiceProvider.GetBytes(array, 0, 8);
		}
		byte[] array2;
		try
		{
			EncryptionFactory.CreateSymmetric(key);
			array2 = EncryptionFactory.CreateSymmetricFromPassword(password, array).EncryptSequence(key);
		}
		catch (Exception ex) when (ex is CryptographicException || ex is ArgumentException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.KeyEncryptionOperationFailed, "Failed to encrypt the key with the provided password.", ex);
		}
		byte[] array3 = new byte[8 + array2.Length];
		Array.Copy(array, array3, 8);
		Array.Copy(array2, 0, array3, 8, array2.Length);
		return array3;
	}

	public byte[] DecryptKey(string password, byte[] saltedEncryptedKey)
	{
		byte[] array = new byte[8];
		Array.Copy(saltedEncryptedKey, 0, array, 0, 8);
		int num = saltedEncryptedKey.Length - 8;
		byte[] array2 = new byte[num];
		Array.Copy(saltedEncryptedKey, 8, array2, 0, num);
		try
		{
			byte[] array3 = EncryptionFactory.CreateSymmetricFromPassword(password, array).DecryptSequence(array2);
			EncryptionFactory.CreateSymmetric(array3);
			return array3;
		}
		catch (Exception ex) when (ex is CryptographicException || ex is ArgumentException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.KeyEncryptionOperationFailed, "Failed to decrypt the key with the provided password.", ex);
		}
	}

	public byte[] GenerateKey()
	{
		return EncryptionFactory.GenerateSymmetric().Key;
	}
}
