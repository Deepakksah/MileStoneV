using System.Security;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface IUserInformation
{
	string UserName { get; set; }

	SecureString Password { get; set; }

	void Dispose();
}
