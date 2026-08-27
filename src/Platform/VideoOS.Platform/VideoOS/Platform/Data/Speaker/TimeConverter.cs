using System;
using System.Globalization;

namespace VideoOS.Platform.Data.Speaker;

public class TimeConverter
{
	protected static readonly DateTime _dt1970;

	static TimeConverter()
	{
		_dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}

	public static DateTime FromDouble(double milesec)
	{
		return (_dt1970 + new TimeSpan((long)milesec * 10000)).ToLocalTime();
	}

	public static DateTime FromString(string milesecString)
	{
		return (_dt1970 + new TimeSpan(long.Parse(milesecString, CultureInfo.InvariantCulture) * 10000)).ToLocalTime();
	}

	public static double ToDouble(DateTime time)
	{
		time = time.ToUniversalTime();
		return (time - _dt1970).Ticks / 10000;
	}
}
