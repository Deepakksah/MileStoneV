namespace VideoOS.Mobile.KeyManagement;

public interface IPasswordChangeSubscriber
{
	void PasswordChanged(string newPassword, string oldPassword);
}
