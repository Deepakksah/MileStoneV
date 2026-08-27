using System;
using System.Security.Cryptography;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration.Licensing;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseImporter : ILicenseImporter
{
	public string UpdateLicense(byte[] newLicense)
	{
		if (newLicense == null)
		{
			throw new ArgumentNullException("newLicense");
		}
		string result = null;
		try
		{
			LicenseFileHelper.ContentType contentType = LicenseFileHelper.ContentType.Unknown;
			contentType = LicenseFileHelper.DetermineLicenseType(newLicense);
			LicenseFileHelper licenseFileHelper = new LicenseFileHelper();
			switch (contentType)
			{
			case LicenseFileHelper.ContentType.Unknown:
			case LicenseFileHelper.ContentType.Request:
				throw new LicenseException("Invalid license", LicenseErrorNumber.InvalidLicenseFile);
			case LicenseFileHelper.ContentType.Activated:
			{
				using (IAsymmetricEncryption asymmetricEncryption = EncryptionFactory.CreateAsymmetricForLicense(Keys.SiteSpecificPrivatePublicKey, CngKeyBlobFormat.GenericPrivateBlob))
				{
					result = licenseFileHelper.GetContentFromActivatedLicenseFile(newLicense, asymmetricEncryption);
				}
				break;
			}
			case LicenseFileHelper.ContentType.Initial:
				result = licenseFileHelper.GetContentFromInitialLicenseFile(newLicense);
				break;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("License decrypt failure", LicenseErrorNumber.LicenseDecryptFailure, inner);
		}
		return result;
	}
}
