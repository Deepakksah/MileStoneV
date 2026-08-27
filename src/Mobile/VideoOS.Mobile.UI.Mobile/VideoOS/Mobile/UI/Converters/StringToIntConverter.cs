using System;
using System.Globalization;

namespace VideoOS.Mobile.UI.Converters;

public class StringToIntConverter : ValueConverterBase
{
	public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		int? num = null;
		if (int.TryParse(value.ToString(), out var result))
		{
			num = result;
		}
		return num;
	}

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value ?? string.Empty;
	}
}
