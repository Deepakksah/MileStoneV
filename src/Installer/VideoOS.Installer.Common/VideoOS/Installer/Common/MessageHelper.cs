using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.Common;

public static class MessageHelper
{
	public static void ShowMessageBox(string text, string caption, MessageBoxIcon icon)
	{
		MessageBox.Show(text, caption, MessageBoxButtons.OK, icon);
	}

	public static string GetSystemErrorMessage(int errorNumber, CultureInfo culture)
	{
		return SystemErrorHelper.GetErrorMessage(errorNumber, culture);
	}

	public static DialogResult ShowInstallerMessageBox(string title, string message)
	{
		InstallerMessageBox obj = new InstallerMessageBox
		{
			Message = message,
			Title = title
		};
		DialogResult result = obj.ShowDialog();
		obj.Dispose();
		return result;
	}

	public static DialogResult ShowInstallerMessageBox(IWin32Window parent, string title, string message)
	{
		InstallerMessageBox obj = new InstallerMessageBox
		{
			Message = message,
			Title = title
		};
		DialogResult result = obj.ShowDialog(parent);
		obj.Dispose();
		return result;
	}

	public static DialogResult ShowInstallerMessageBox(string title, string message, InstallerMessageBoxIcon icon)
	{
		InstallerMessageBox obj = new InstallerMessageBox
		{
			Message = message,
			Title = title,
			MessageBoxIcon = icon
		};
		DialogResult result = obj.ShowDialog();
		obj.Dispose();
		return result;
	}

	public static DialogResult ShowInstallerMessageBox(IWin32Window parent, string title, string message, InstallerMessageBoxIcon icon)
	{
		InstallerMessageBox obj = new InstallerMessageBox
		{
			Message = message,
			Title = title,
			MessageBoxIcon = icon
		};
		DialogResult result = obj.ShowDialog(parent);
		obj.Dispose();
		return result;
	}

	public static DialogResult ShowInstallerMessageBox(string title, string message, InstallerMessageBoxIcon icon, InstallerMessageBoxButtons buttons)
	{
		InstallerMessageBox obj = new InstallerMessageBox
		{
			Message = message,
			Title = title,
			MessageBoxIcon = icon,
			Buttons = buttons
		};
		DialogResult result = obj.ShowDialog();
		obj.Dispose();
		return result;
	}

	public static DialogResult ShowInstallerMessageBox(IWin32Window parent, string title, string message, InstallerMessageBoxIcon icon, InstallerMessageBoxButtons buttons)
	{
		InstallerMessageBox obj = new InstallerMessageBox
		{
			Message = message,
			Title = title,
			MessageBoxIcon = icon,
			Buttons = buttons
		};
		DialogResult result = obj.ShowDialog(parent);
		obj.Dispose();
		return result;
	}
}
