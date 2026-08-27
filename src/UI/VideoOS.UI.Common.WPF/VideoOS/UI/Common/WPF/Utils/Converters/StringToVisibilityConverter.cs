using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class StringToVisibilityConverter : IValueConverter
{
	public Visibility EmptyVisibility { get; set; } = Visibility.Collapsed;

	public Visibility NonEmptyVisibility { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!string.IsNullOrEmpty(value as string))
		{
			return NonEmptyVisibility;
		}
		return EmptyVisibility;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
