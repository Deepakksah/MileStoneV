using System;
using System.Globalization;

namespace VideoOS.Platform.Util;

public static class TimeConvert
{
	public static string ToGmtString(DateTime dateTime, bool includeMilliSeconds)
	{
		DateTime dateTime2 = ((dateTime.Year > 2000) ? dateTime : DateTime.UtcNow);
		if (dateTime2.Kind != DateTimeKind.Utc)
		{
			dateTime2 = dateTime2.ToUniversalTime();
		}
		DateTime dateTime3 = dateTime2.ToLocalTime();
		TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTime3);
		string text = ((utcOffset.Ticks >= 0) ? "+" : "");
		string text2 = string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}", utcOffset.Hours, Math.Abs(utcOffset.Minutes));
		string text3 = (includeMilliSeconds ? "yyyy-MM-ddTHH:mm:ss.fff" : "yyyy-MM-ddTHH:mm:ss");
		return dateTime3.ToString(text3, CultureInfo.InvariantCulture) + text + text2;
	}

	public static string ToGmtString(DateTime dateTime)
	{
		return ToGmtString(dateTime, includeMilliSeconds: false);
	}
}
