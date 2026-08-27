using System;

namespace VideoOS.Platform.License;

public class LicenseCompatibilityHandler : ILicenseCompatibilityHandler
{
	public Guid RegistrationsObjectId { get; } = new Guid("AD5EFDCE-3B21-47D5-A6A8-657E43A3E638");

	public Guid LicensesObjectId { get; } = new Guid("B9AFA95A-652B-40A4-981B-F3C7D39BFFDC");

	public string Encrypt(string value)
	{
		return value;
	}

	public string Decrypt(string cipherText)
	{
		return cipherText;
	}
}
