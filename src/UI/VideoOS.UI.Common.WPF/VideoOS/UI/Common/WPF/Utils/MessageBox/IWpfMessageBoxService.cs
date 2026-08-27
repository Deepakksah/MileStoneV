using VideoOS.Common.Definition;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public interface IWpfMessageBoxService
{
	WpfMessageBoxResult Show(string text);

	WpfMessageBoxResult Show(string text, string caption);

	WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button);

	WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxConfiguration configuration);

	WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image);

	WpfMessageBoxResult Show(string text, string caption, WpfMessageBoxButton button, WpfMessageBoxImage image, WpfMessageBoxConfiguration configuration);
}
