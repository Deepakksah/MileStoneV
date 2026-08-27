using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class BoolToGridLengthConverter : MarkupExtension, IValueConverter
{
	public GridLength FalseResult { get; set; } = GridLength.Auto;

	public GridLength TrueResult { get; set; } = GridLength.Auto;

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
