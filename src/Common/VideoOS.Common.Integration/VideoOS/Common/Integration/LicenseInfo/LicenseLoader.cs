using System;
using System.IO;
using System.Text;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseLoader : ILicenseLoader
{
	public ILicenseDocument CreateLicenseDocument(string plainTextLicense)
	{
		return new LicenseDocument(plainTextLicense);
	}

	public ILicenseDocument CreateLicenseDocument(byte[] utf8LicenseBytes)
	{
		string plainTextLicense = Encoding.UTF8.GetString(utf8LicenseBytes);
		return CreateLicenseDocument(plainTextLicense);
	}

	public byte[] ConvertLicenseForStorage(byte[] licenseForStorage)
	{
		if (licenseForStorage == null || licenseForStorage.Length == 0)
		{
			return licenseForStorage;
		}
		string s;
		try
		{
			s = LicenseFactory.CreateLicenseImporter().UpdateLicense(licenseForStorage);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("License decrypt failure", LicenseErrorNumber.LicenseDecryptFailure, inner);
		}
		return Encoding.UTF8.GetBytes(s);
	}

	public ILicenseDocument ImportLicenseFile(byte[] externallyEncryptedLicense)
	{
		string xmlLicense;
		try
		{
			xmlLicense = LicenseFactory.CreateLicenseImporter().UpdateLicense(externallyEncryptedLicense);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("License decrypt failure", LicenseErrorNumber.LicenseDecryptFailure, inner);
		}
		return new LicenseDocument(xmlLicense);
	}

	public ILicenseDocument ImportLicenseFile(string filename)
	{
		if (!File.Exists(filename))
		{
			throw new FileNotFoundException("the license file could not be found", filename);
		}
		return ImportLicenseFile(File.ReadAllBytes(filename));
	}
}
