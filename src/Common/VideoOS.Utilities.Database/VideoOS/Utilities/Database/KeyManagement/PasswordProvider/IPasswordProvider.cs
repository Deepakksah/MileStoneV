namespace VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

public interface IPasswordProvider
{
	IWritingSession StartWritingSession(string accountName);

	string ReadPassword();

	bool HasPassword();

	void DeletePassword();
}
