using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class ObjToTypeStrConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value.GetType().Name;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return DependencyProperty.UnsetValue;
	}
}
