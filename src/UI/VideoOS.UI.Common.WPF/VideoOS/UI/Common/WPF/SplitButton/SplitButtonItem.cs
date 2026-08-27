using System.Windows;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.SplitButton;

public class SplitButtonItem : DependencyObject
{
	public string Label { get; set; }

	public string ImageName { get; set; }

	public Visibility ImageFromNameVisibility { get; set; }

	public ImageSource ImageSource { get; set; }

	public Visibility ImageFromSourceVisibility { get; set; }

	public object Tag { get; set; }

	public SplitButtonItem(string label, string imageName)
	{
		Label = label;
		ImageName = imageName;
		ImageFromNameVisibility = Visibility.Visible;
		ImageFromSourceVisibility = Visibility.Collapsed;
	}

	public SplitButtonItem(string label, ImageSource imageSource)
	{
		Label = label;
		ImageSource = imageSource;
		ImageFromNameVisibility = Visibility.Collapsed;
		ImageFromSourceVisibility = Visibility.Visible;
	}
}
