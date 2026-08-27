namespace VideoOS.Mobile.KeyManagement;

public interface IKeyUpdater
{
	void PasswordChanged(string newPassword, string oldPassword);
}
