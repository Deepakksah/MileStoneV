namespace VideoOS.Utilities.Database.KeyManagement;

public interface IEncryptionKeyReader
{
	void VerifyAndSavePassword(string password, string accountName);

	bool VerifyPassword(string password);

	byte[] GetUnencryptedKey(string password);

	byte[] GetUnencryptedKey();

	byte[] GetKey();

	KeyManagementState GetKeyManagementState();
}
