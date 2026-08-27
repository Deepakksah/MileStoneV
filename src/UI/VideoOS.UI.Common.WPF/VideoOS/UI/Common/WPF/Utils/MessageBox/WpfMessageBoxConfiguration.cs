using System.Windows;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Utils.MessageBox;

public class WpfMessageBoxConfiguration
{
	public ImageSource Icon { get; set; }

	public string OkButtonText { get; set; } = string.Empty;

	public string CancelButtonText { get; set; } = string.Empty;

	public string YesButtonText { get; set; } = string.Empty;

	public string NoButtonText { get; set; } = string.Empty;

	public string RemoveButtonText { get; set; } = string.Empty;

	public Style OkButtonStyle { get; set; }

	public Style CancelButtonStyle { get; set; }

	public Style YesButtonStyle { get; set; }

	public Style NoButtonStyle { get; set; }

	public Style RemoveButtonStyle { get; set; }
}
