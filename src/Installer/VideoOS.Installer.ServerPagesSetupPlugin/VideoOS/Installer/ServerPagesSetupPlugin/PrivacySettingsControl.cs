using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public partial class PrivacySettingsControl : UserControl, IComponentConnector
{
	public PrivacySettingsControl()
	{
		InitializeComponent();
	}

	private void hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
		e.Handled = true;
	}
}
