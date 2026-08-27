using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.Mobile.UI.Converters;

public class InvertBooleanConverter : ValueConverterBase
{
	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool)
		{
			return !(bool)value;
		}
		return Binding.DoNothing;
	}
}
