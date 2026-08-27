using System.Windows;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Controls.Popups;

public class PopoverContentTemplateSelector : DataTemplateSelector
{
	public DataTemplate TextTemplate { get; set; }

	public DataTemplate CustomTemplate { get; set; }

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item is string)
		{
			return TextTemplate;
		}
		return CustomTemplate;
	}
}
