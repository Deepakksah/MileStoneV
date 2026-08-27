namespace VideoOS.Mobile.KeyManagement;

public interface IPasswordManager
{
	bool HasPassword();

	string ReadPassword();

	void SetPassword(string password, string accountName);

	bool HasPendingPasswordChange();

	string ReadOldPassword();

	PasswordChangeStatus ChangePassword(string password, string accountName);

	void DeletePassword();

	void RegisterForPasswordChange(IPasswordChangeSubscriber subscriber);

	void UnRegisterForPasswordChange(IPasswordChangeSubscriber subscriber);
}
