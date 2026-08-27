using System;
using System.Globalization;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Utils;

public static class DateTimeFormatter
{
	public static string ToTimezonedShortDate(DateTime dateTime, CultureInfo culture)
	{
		return TimeZoneInfo.ConvertTime(dateTime, ClientControl.Instance.SelectedTimeZoneInfo).ToString(culture.DateTimeFormat.ShortDatePattern, culture);
	}

	public static string ToTimezonedLongTime(DateTime dateTime, CultureInfo culture)
	{
		return TimeZoneInfo.ConvertTime(dateTime, ClientControl.Instance.SelectedTimeZoneInfo).ToString(culture.DateTimeFormat.LongTimePattern, culture);
	}

	public static string ToTimezonedShortDateLongTime(DateTime dateTime, CultureInfo culture)
	{
		return ToTimezonedShortDate(dateTime, culture) + " " + ToTimezonedLongTime(dateTime, culture);
	}

	public static string ToTimezonedFullDateTime(DateTime dateTime, CultureInfo culture)
	{
		return TimeZoneInfo.ConvertTime(dateTime, ClientControl.Instance.SelectedTimeZoneInfo).ToString(culture.DateTimeFormat.FullDateTimePattern, culture);
	}
}
