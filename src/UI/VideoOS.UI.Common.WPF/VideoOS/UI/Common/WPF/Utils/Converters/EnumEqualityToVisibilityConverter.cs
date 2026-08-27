using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class EnumEqualityToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null || parameter == null || !(value is Enum))
		{
			return Visibility.Collapsed;
		}
		if (value.ToString().Equals(parameter))
		{
			return Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if ((Visibility)value == Visibility.Visible)
		{
			return parameter;
		}
		return null;
	}
}
