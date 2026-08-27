using System;
using System.Globalization;

namespace VideoOS.Mobile.UI.Converters;

public class NullToBooleanConverter : ValueConverterBase
{
	public bool ReverseConversion { get; set; }

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (!ReverseConversion) ? (value == null) : (value != null);
	}
}
