namespace VideoOS.Utilities.Database.KeyManagement;

public interface IEncryptionKeyUpdater : IEncryptionKeyReader
{
	void ChangePassword(string newPassword, string accountName);
}
