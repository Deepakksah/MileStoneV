using System;
using System.Windows;
using System.Windows.Markup;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public partial class ConfirmationDialog : Window, IComponentConnector
{
	public ConfirmationDialog()
	{
		InitializeComponent();
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		IconHelper.RemoveIcon(this);
	}

	private void ButtonOK_Click(object sender, RoutedEventArgs e)
	{
		base.DialogResult = true;
	}

	private void ButtonCancel_Click(object sender, RoutedEventArgs e)
	{
		base.DialogResult = false;
	}
}
