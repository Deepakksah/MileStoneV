using VideoOS.Common.Crypto;

namespace VideoOS.Common.Integration.Licensing;

public interface ILicenseFileHelper
{
	string GetContentFromInitialLicenseFile(byte[] licenseFile);

	string GetContentFromActivatedLicenseFile(byte[] licenseFile, IAsymmetricEncryption asymmetricEncryption);

	byte[] CreateLicenseRequestFile(string requestJsonPayload, string lrqPublicKey, string sitePublicKey);
}
