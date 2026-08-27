using System;
using System.Globalization;

namespace VideoOS.Mobile.UI.Converters;

public class StringToBooleanConverter : ValueConverterBase
{
	public bool ReverseConversion { get; set; }

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (!ReverseConversion) ? string.IsNullOrWhiteSpace(value as string) : (!string.IsNullOrWhiteSpace(value as string));
	}
}
