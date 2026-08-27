using System;
using System.Globalization;

namespace VideoOS.Utilities.DateTimeHelper;

public class DateTimeParser
{
	public static DateTime ParseAndConvertToUtc(string input, TimeZoneInfo sourceTimeZone)
	{
		return ParseAndConvert(input, sourceTimeZone, TimeZoneInfo.Utc);
	}

	public static DateTime ParseAndConvertToLocal(string input, TimeZoneInfo sourceTimeZone)
	{
		return ParseAndConvert(input, sourceTimeZone, TimeZoneInfo.Local);
	}

	public static DateTime ParseAndConvert(string input, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
	{
		return TimeZoneInfo.ConvertTime(DateTime.Parse(input), sourceTimeZone, destinationTimeZone);
	}

	public static bool TryParseAndConvertToUtc(string input, TimeZoneInfo sourceTimeZone, out DateTime utcTime)
	{
		return TryParseAndConvert(input, sourceTimeZone, TimeZoneInfo.Utc, out utcTime);
	}

	public static bool TryParseAndConvertToLocal(string input, TimeZoneInfo sourceTimeZone, out DateTime localTime)
	{
		return TryParseAndConvert(input, sourceTimeZone, TimeZoneInfo.Local, out localTime);
	}

	public static bool TryParseAndConvert(string input, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone, out DateTime convertedTime)
	{
		try
		{
			_ = CultureInfo.CurrentCulture;
			DateTime result2;
			bool result = DateTime.TryParse(input, out result2);
			convertedTime = TimeZoneInfo.ConvertTime(result2, sourceTimeZone, destinationTimeZone);
			return result;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			convertedTime = DateTime.MinValue;
			return false;
		}
	}
}
