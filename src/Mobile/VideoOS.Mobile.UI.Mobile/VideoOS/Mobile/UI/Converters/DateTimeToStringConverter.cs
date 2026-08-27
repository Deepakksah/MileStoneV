using System;
using System.Globalization;
using VideoOS.Mobile.UI.Enums;

namespace VideoOS.Mobile.UI.Converters;

public class DateTimeToStringConverter : ValueConverterBase
{
	public DateTimeFormatType Format { get; set; }

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string result = null;
		if (value is DateTime dateTime)
		{
			result = Format switch
			{
				DateTimeFormatType.ShortTime => dateTime.ToShortTimeString(), 
				DateTimeFormatType.LongTime => dateTime.ToLongTimeString(), 
				DateTimeFormatType.ShortDate => dateTime.ToShortDateString(), 
				DateTimeFormatType.LongDate => dateTime.ToLongDateString(), 
				_ => dateTime.ToString(culture), 
			};
		}
		return result;
	}
}
