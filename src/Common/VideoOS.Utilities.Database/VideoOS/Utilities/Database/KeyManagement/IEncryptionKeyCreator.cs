namespace VideoOS.Utilities.Database.KeyManagement;

public interface IEncryptionKeyCreator : IEncryptionKeyReader
{
	void InsertPasswordAndKey(string newPassword, string accountName);
}
