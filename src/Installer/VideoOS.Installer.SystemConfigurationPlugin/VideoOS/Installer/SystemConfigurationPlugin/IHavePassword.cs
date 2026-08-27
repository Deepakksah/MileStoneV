using System.Windows.Controls;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface IHavePassword
{
	PasswordBox PasswordBox { get; }

	PasswordBox PasswordRepeatBox { get; }
}
