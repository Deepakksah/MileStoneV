using VideoOS.Installer.Common.Utilities;

namespace VideoOS.GatewayService.Utilities;

public class EncryptHelperWrapper : IEncryptHelperWrapper
{
	public string DecryptPassword(string encryptedPass, bool unicodePassword)
	{
		return EncryptHelper.DecryptPassword(encryptedPass, unicodePassword);
	}

	public string EncryptPassword(string password, bool unicodePassword)
	{
		return EncryptHelper.EncryptPassword(password, unicodePassword);
	}
}
