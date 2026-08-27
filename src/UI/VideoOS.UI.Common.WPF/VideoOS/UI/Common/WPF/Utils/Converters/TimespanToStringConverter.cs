using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class TimespanToStringConverter : MarkupExtension, IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is TimeSpan timeSpan))
		{
			return string.Empty;
		}
		return string.Format(culture, "{1}{0}{2}{0}{3}", culture.DateTimeFormat.TimeSeparator, (int)timeSpan.TotalHours, timeSpan.Minutes.ToString("00", culture), timeSpan.Seconds.ToString("00", culture));
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
