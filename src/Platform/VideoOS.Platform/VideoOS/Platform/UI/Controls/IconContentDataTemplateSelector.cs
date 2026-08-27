using System.Windows;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Controls;

public class IconContentDataTemplateSelector : DataTemplateSelector
{
	public DataTemplate BitmapTemplate { get; set; }

	public DataTemplate ResourceTemplate { get; set; }

	public DataTemplate UriTemplate { get; set; }

	public DataTemplate PathTemplate { get; set; }

	public DataTemplate MultiPathTemplate { get; set; }

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item is VideoOSIconBitmapSource)
		{
			return BitmapTemplate;
		}
		if (item is VideoOSIconResourceSource)
		{
			return ResourceTemplate;
		}
		if (item is VideoOSIconUriSource)
		{
			return UriTemplate;
		}
		if (item is VideoOSIconPathSource)
		{
			return PathTemplate;
		}
		if (item is VideoOSIconMultiPathSource)
		{
			return MultiPathTemplate;
		}
		return null;
	}
}
