using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class BoolToThicknessConverter : MarkupExtension, IValueConverter
{
	public Thickness FalseResult { get; set; } = new Thickness(0.0);

	public Thickness TrueResult { get; set; } = new Thickness(0.0);

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
