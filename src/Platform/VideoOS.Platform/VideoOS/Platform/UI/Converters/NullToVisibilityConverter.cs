using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class NullToVisibilityConverter : MarkupExtension, IValueConverter
{
	public Visibility NullResult { get; set; } = Visibility.Collapsed;

	public Visibility NotNullResult { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value == null) ? NullResult : NotNullResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
