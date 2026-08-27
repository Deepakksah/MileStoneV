using System;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Common.Crypto;

public static class EncryptionFactory
{
	private const int Aes256KeySize = 32;

	private const int IterationCount = 5000;

	private const string DefaultKey = "6XeDzzmwVl8U4o/cCyJc3D92Uom0QHxMa0Fu5EXFBaM=";

	public static IAsymmetricEncryption CreateAsymmetric(RSAEncryptionPadding padding)
	{
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		return new RsaCngAsymmetricEncryption(padding);
	}

	public static IAsymmetricEncryption CreateAsymmetric(string keys, RSAEncryptionPadding padding, CngKeyBlobFormat blobFormat)
	{
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		if (blobFormat == null)
		{
			throw new ArgumentNullException("blobFormat");
		}
		if (string.IsNullOrEmpty(keys))
		{
			throw new ArgumentNullException("keys");
		}
		return new RsaCngAsymmetricEncryption(CngKey.Import(Convert.FromBase64String(keys), blobFormat), padding);
	}

	public static IAsymmetricEncryption CreateAsymmetric(CngKey cngKey, RSAEncryptionPadding padding)
	{
		if (cngKey == null)
		{
			throw new ArgumentNullException("cngKey");
		}
		if (padding == null)
		{
			throw new ArgumentNullException("padding");
		}
		return new RsaCngAsymmetricEncryption(cngKey, padding);
	}

	public static IAsymmetricEncryption CreateAsymmetricForLicense()
	{
		return CreateAsymmetric(RSAEncryptionPadding.OaepSHA1);
	}

	public static IAsymmetricEncryption CreateAsymmetricForLicense(CngKey cngKey)
	{
		if (cngKey == null)
		{
			throw new ArgumentNullException("cngKey");
		}
		return CreateAsymmetric(cngKey, RSAEncryptionPadding.OaepSHA1);
	}

	public static IAsymmetricEncryption CreateAsymmetricForLicense(string keys, CngKeyBlobFormat blobFormat)
	{
		if (blobFormat == null)
		{
			throw new ArgumentNullException("blobFormat");
		}
		if (string.IsNullOrEmpty(keys))
		{
			throw new ArgumentNullException("keys");
		}
		return CreateAsymmetric(keys, RSAEncryptionPadding.OaepSHA1, blobFormat);
	}

	public static ISymmetricEncryption CreateSymmetric()
	{
		return CreateSymmetric("6XeDzzmwVl8U4o/cCyJc3D92Uom0QHxMa0Fu5EXFBaM=");
	}

	public static ISymmetricEncryption GenerateSymmetric()
	{
		return new AesSymmetricEncryption(AesSymmetricEncryption.GenerateKey());
	}

	public static ISymmetricEncryption CreateSymmetric(string key)
	{
		return new AesSymmetricEncryption(Convert.FromBase64String(key));
	}

	public static ISymmetricEncryption CreateSymmetric(byte[] key)
	{
		return new AesSymmetricEncryption(key);
	}

	public static ISymmetricEncryption CreateSymmetricFromPassword(string password, byte[] salt)
	{
		return new AesSymmetricEncryption(GetKey(Encoding.Unicode.GetBytes(password), salt, 5000, 32));
	}

	private static byte[] GetKey(byte[] password, byte[] salt, int iterationCount, int keySizeInBytes)
	{
		using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterationCount, HashAlgorithmName.SHA1);
		return rfc2898DeriveBytes.GetBytes(keySizeInBytes);
	}
}
