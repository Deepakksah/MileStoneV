namespace VideoOS.GatewayService.Utilities;

public interface IEncryptHelperWrapper
{
	string EncryptPassword(string password, bool unicodePassword);

	string DecryptPassword(string encryptedPass, bool unicodePassword);
}
