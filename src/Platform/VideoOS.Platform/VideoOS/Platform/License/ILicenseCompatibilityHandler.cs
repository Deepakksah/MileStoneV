using System;

namespace VideoOS.Platform.License;

public interface ILicenseCompatibilityHandler
{
	Guid RegistrationsObjectId { get; }

	Guid LicensesObjectId { get; }

	string Encrypt(string value);

	string Decrypt(string cipherText);
}
