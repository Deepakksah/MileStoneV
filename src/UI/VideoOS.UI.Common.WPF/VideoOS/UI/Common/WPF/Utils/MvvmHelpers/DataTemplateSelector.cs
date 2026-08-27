using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

[ContentProperty("DataTemplateSelectorOptions")]
public class DataTemplateSelector : System.Windows.Controls.DataTemplateSelector
{
	public ObservableCollection<DataTemplateSelectorOption> DataTemplateSelectorOptions { get; private set; }

	public DataTemplateSelector()
	{
		DataTemplateSelectorOptions = new ObservableCollection<DataTemplateSelectorOption>();
	}

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		foreach (DataTemplateSelectorOption dataTemplateSelectorOption in DataTemplateSelectorOptions)
		{
			dataTemplateSelectorOption.DataContext = item;
			if (dataTemplateSelectorOption.DoesMatch)
			{
				return dataTemplateSelectorOption.DataTemplate;
			}
		}
		return base.SelectTemplate(item, container);
	}
}
