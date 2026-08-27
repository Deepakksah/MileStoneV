using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class InvertedBooleanConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool)
		{
			return !(bool)value;
		}
		throw new ArgumentException();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool)
		{
			return !(bool)value;
		}
		throw new ArgumentException();
	}
}
