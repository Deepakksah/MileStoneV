using System;
using System.Security.Cryptography;

namespace VideoOS.Platform.License;

public class NonFipsLicenseCompatibilityHandler : ILicenseCompatibilityHandler
{
	public Guid RegistrationsObjectId { get; } = new Guid("E125597A-ECAF-40A1-99E6-24BB9D45A09E");

	public Guid LicensesObjectId { get; } = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	public string Encrypt(string value)
	{
		try
		{
			return Convert.ToBase64String(new CryptoHelper(Keys.SymmetricAlgorithmConfigurationXml).EncryptStringData(value));
		}
		catch (Exception ex) when (ex is InvalidOperationException || ex is CryptographicException)
		{
			throw new MIPException("Failed to encrypt data.", ex.InnerException);
		}
	}

	public string Decrypt(string cipherText)
	{
		try
		{
			byte[] encrypted = Convert.FromBase64String(cipherText);
			return new CryptoHelper(Keys.SymmetricAlgorithmConfigurationXml).DecryptStringData(encrypted);
		}
		catch (Exception ex) when (ex is InvalidOperationException || ex is CryptographicException)
		{
			throw new MIPException("Failed to decrypt data.", ex.InnerException);
		}
	}
}
