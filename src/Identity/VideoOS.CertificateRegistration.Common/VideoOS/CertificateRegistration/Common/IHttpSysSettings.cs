using System.Security.Principal;

namespace VideoOS.CertificateRegistration.Common;

public interface IHttpSysSettings
{
	bool EncryptionEnabled { get; set; }

	byte[] CertificateHash { get; set; }

	SecurityIdentifier ServiceUserSid { get; set; }

	void SaveSettings();

	void SaveSettings(bool force);

	void DeleteSettings();
}
