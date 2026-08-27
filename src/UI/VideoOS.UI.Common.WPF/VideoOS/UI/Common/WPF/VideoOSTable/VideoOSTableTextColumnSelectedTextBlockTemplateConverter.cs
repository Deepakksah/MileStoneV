using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.VideoOSTable;

public class VideoOSTableTextColumnSelectedTextBlockTemplateConverter : MarkupExtension, IValueConverter
{
	public DataTemplate SelectedTemplate { get; set; }

	public DataTemplate DeselectedTemplate { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is bool))
		{
			return null;
		}
		if ((bool)value)
		{
			return SelectedTemplate;
		}
		return DeselectedTemplate;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
