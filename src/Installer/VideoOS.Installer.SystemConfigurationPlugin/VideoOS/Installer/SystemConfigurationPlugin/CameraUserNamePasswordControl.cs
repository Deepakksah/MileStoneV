using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public partial class CameraUserNamePasswordControl : UserControl, IComponentConnector, IStyleConnector
{
	public CameraUserNamePasswordControl()
	{
		InitializeComponent();
	}

	private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		PasswordBox passwordBox = sender as PasswordBox;
		PasswordBoxAttachedProperties.SetEncryptedPassword(passwordBox, passwordBox.SecurePassword);
	}
}
