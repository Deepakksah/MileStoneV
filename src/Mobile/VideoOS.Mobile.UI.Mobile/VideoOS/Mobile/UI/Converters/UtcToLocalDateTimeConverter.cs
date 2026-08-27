using System;
using System.Globalization;

namespace VideoOS.Mobile.UI.Converters;

public class UtcToLocalDateTimeConverter : ValueConverterBase
{
	public bool ReverseConversion { get; set; }

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is DateTime dateTime)
		{
			return (!ReverseConversion) ? dateTime.ToLocalTime() : dateTime.ToUniversalTime();
		}
		return null;
	}
}
