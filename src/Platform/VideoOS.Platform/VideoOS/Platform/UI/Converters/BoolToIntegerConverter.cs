using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class BoolToIntegerConverter : MarkupExtension, IValueConverter
{
	public int FalseResult { get; set; }

	public int TrueResult { get; set; } = 1;

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
