using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class BoolToDoubleConverter : MarkupExtension, IValueConverter
{
	public double FalseResult { get; set; }

	public double TrueResult { get; set; } = 1.0;

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
