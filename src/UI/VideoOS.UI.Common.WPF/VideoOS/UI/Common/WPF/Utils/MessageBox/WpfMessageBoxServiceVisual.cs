using System.Windows.Media;
using VideoOS.Common.Definition;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public class WpfMessageBoxServiceVisual : IWpfMessageBoxService
{
	private readonly Visual _visual;

	public WpfMessageBoxServiceVisual(Visual visual)
	{
		_visual = visual;
	}

	public WpfMessageBoxResult Show(string text)
	{
		return WpfMessageBox.Show(_visual, text);
	}

	public WpfMessageBoxResult Show(string text, string caption)
	{
		return WpfMessageBox.Show(_visual, text, caption);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button)
	{
		return WpfMessageBox.Show(_visual, text, caption, button);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration)
	{
		return WpfMessageBox.Show(_visual, text, caption, button, configuration);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image)
	{
		return WpfMessageBox.Show(_visual, text, caption, button, image);
	}

	public WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image, WpfMessageBoxConfiguration configuration)
	{
		return WpfMessageBox.Show(_visual, text, caption, button, configuration, image);
	}
}
