using System.Windows;
using VideoOS.Common.Definition;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public class WpfMessageBoxServiceWindow : IWpfMessageBoxService
{
	private readonly Window _window;

	public WpfMessageBoxServiceWindow(Window window)
	{
		_window = window;
	}

	public WpfMessageBoxResult Show(string text)
	{
		return WpfMessageBox.Show(_window, text);
	}

	public WpfMessageBoxResult Show(string text, string caption)
	{
		return WpfMessageBox.Show(_window, text, caption);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button)
	{
		return WpfMessageBox.Show(_window, text, caption, button);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration)
	{
		return WpfMessageBox.Show(_window, text, caption, button, configuration);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		return WpfMessageBox.Show(_window, text, caption, button, image);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image, WpfMessageBoxConfiguration configuration)
	{
		return WpfMessageBox.Show(_window, text, caption, button, configuration, image);
	}
}
