using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class StringNullOrEmptyToVisibilityConverter : MarkupExtension, IValueConverter
{
	public Visibility NullOrEmptyResult { get; set; } = Visibility.Collapsed;

	public Visibility NotNullOrEmptyResult { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.IsNullOrEmpty((string)value) ? NullOrEmptyResult : NotNullOrEmptyResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
