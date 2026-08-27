using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class NullToThicknessConverter : MarkupExtension, IValueConverter
{
	public Thickness NullResult { get; set; } = new Thickness(0.0);

	public Thickness NotNullResult { get; set; } = new Thickness(0.0);

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
