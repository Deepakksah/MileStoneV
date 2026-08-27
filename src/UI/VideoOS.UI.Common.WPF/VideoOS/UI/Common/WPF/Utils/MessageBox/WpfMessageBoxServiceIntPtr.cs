using System;
using VideoOS.Common.Definition;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public class WpfMessageBoxServiceIntPtr : IWpfMessageBoxService
{
	private readonly IntPtr _formsOwner;

	public WpfMessageBoxServiceIntPtr(IntPtr formsOwner)
	{
		_formsOwner = formsOwner;
	}

	public WpfMessageBoxResult Show(string text)
	{
		return WpfMessageBox.Show(_formsOwner, text);
	}

	public WpfMessageBoxResult Show(string text, string caption)
	{
		return WpfMessageBox.Show(_formsOwner, text, caption);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button)
	{
		return WpfMessageBox.Show(_formsOwner, text, caption, button);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration)
	{
		return WpfMessageBox.Show(_formsOwner, text, caption, button, configuration);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		return WpfMessageBox.Show(_formsOwner, text, caption, button, image);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image, WpfMessageBoxConfiguration configuration)
	{
		return WpfMessageBox.Show(_formsOwner, text, caption, button, configuration, image);
	}
}
