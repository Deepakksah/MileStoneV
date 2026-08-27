using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using VideoOS.Platform.UI.Controls;

namespace VideoOS.Common.Help.UI;

public partial class HelpNotAvailableDialog : VideoOSDialog, IComponentConnector
{
	public static void ShowDialog(Window ownerWindow, string helpUrl)
	{
		HelpNotAvailableDialog helpNotAvailableDialog = new HelpNotAvailableDialog(helpUrl);
		helpNotAvailableDialog.Owner = ownerWindow;
		helpNotAvailableDialog.ShowDialog();
	}

	public static void ShowDialog(IntPtr ownerHandle, string helpUrl)
	{
		HelpNotAvailableDialog helpNotAvailableDialog = new HelpNotAvailableDialog(helpUrl);
		helpNotAvailableDialog.WindowStyle = WindowStyle.ToolWindow;
		new WindowInteropHelper(helpNotAvailableDialog).Owner = ownerHandle;
		helpNotAvailableDialog.ShowDialog();
	}

	public HelpNotAvailableDialog(string helpUrl)
	{
		InitializeComponent();
		base.DataContext = new HelpNotAvailableDialogViewModel(helpUrl);
	}

	private void VideoOSDialogActionButtonBase_OnClick(object sender, EventArgs e)
	{
		Close();
	}
}
