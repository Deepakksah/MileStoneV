using System.Security;

namespace VideoOS.Utilities.Security;

public interface IPasswordValidator
{
	PasswordValidationResult ValidateNewPassword(SecureString passwordEntry1, SecureString passwordEntry2);

	PasswordValidationResult ValidatePassword(SecureString password);
}
