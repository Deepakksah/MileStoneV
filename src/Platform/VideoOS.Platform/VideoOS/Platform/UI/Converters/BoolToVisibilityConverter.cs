using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
{
	public Visibility TrueResult { get; set; }

	public Visibility FalseResult { get; set; } = Visibility.Collapsed;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value != null && (bool)value) ? TrueResult : FalseResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
