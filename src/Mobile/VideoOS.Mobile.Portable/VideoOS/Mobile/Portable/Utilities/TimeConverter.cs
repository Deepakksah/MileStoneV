using System;
using System.Globalization;

namespace VideoOS.Mobile.Portable.Utilities;

public static class TimeConverter
{
	public static readonly DateTime Dt1970 = new DateTime(1970, 1, 1);

	public static DateTime FromDouble(double millisec)
	{
		return FromLong((long)millisec);
	}

	public static DateTime FromLong(long millisec)
	{
		return Dt1970 + new TimeSpan(millisec * 10000);
	}

	public static DateTime FromString(string millisec)
	{
		return FromLong(long.Parse(millisec, NumberStyles.Integer));
	}

	public static DateTime FromStringUtc(string millisec)
	{
		return DateTime.SpecifyKind(FromLong(long.Parse(millisec, NumberStyles.Integer)), DateTimeKind.Utc);
	}

	public static double ToDouble(this DateTime time)
	{
		return time.ToLong();
	}

	public static long ToLong(this DateTime time)
	{
		return (time - Dt1970).Ticks / 10000;
	}

	public static string ToMilisecString(this DateTime time)
	{
		return time.ToLong().ToString();
	}
}
