using System;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class DateTimeExtensions
{
	public static DateTime RoundToMillisecond(this DateTime dateTime, DateTimeRoundMethod roundMethod)
	{
		long num = dateTime.Ticks % 10000;
		if (num == 0L)
		{
			return dateTime;
		}
		switch (roundMethod)
		{
		case DateTimeRoundMethod.Down:
			return dateTime.AddTicks(-num);
		case DateTimeRoundMethod.Up:
			return dateTime.AddTicks(10000 - num);
		default:
			if (num < 5000)
			{
				return dateTime.RoundToMillisecond(DateTimeRoundMethod.Down);
			}
			return dateTime.RoundToMillisecond(DateTimeRoundMethod.Up);
		}
	}

	public static DateTime RoundToSecond(this DateTime dateTime, DateTimeRoundMethod roundMethod)
	{
		long num = dateTime.Ticks % 10000000;
		if (num == 0L)
		{
			return dateTime;
		}
		switch (roundMethod)
		{
		case DateTimeRoundMethod.Down:
			return dateTime.AddTicks(-num);
		case DateTimeRoundMethod.Up:
			return dateTime.AddTicks(10000000 - num);
		default:
			if (num < 5000000)
			{
				return dateTime.RoundToSecond(DateTimeRoundMethod.Down);
			}
			return dateTime.RoundToSecond(DateTimeRoundMethod.Up);
		}
	}

	public static DateTime RoundToMinute(this DateTime dateTime, DateTimeRoundMethod roundMethod)
	{
		long num = dateTime.Ticks % 600000000;
		if (num == 0L)
		{
			return dateTime;
		}
		switch (roundMethod)
		{
		case DateTimeRoundMethod.Down:
			return dateTime.AddTicks(-num);
		case DateTimeRoundMethod.Up:
			return dateTime.AddTicks(600000000 - num);
		default:
			if (num < 300000000)
			{
				return dateTime.RoundToMinute(DateTimeRoundMethod.Down);
			}
			return dateTime.RoundToMinute(DateTimeRoundMethod.Up);
		}
	}

	public static DateTime RoundToHour(this DateTime dateTime, DateTimeRoundMethod roundMethod)
	{
		long num = dateTime.Ticks % 36000000000L;
		if (num == 0L)
		{
			return dateTime;
		}
		switch (roundMethod)
		{
		case DateTimeRoundMethod.Down:
			return dateTime.AddTicks(-num);
		case DateTimeRoundMethod.Up:
			return dateTime.AddTicks(36000000000L - num);
		default:
			if (num < 18000000000L)
			{
				return dateTime.RoundToHour(DateTimeRoundMethod.Down);
			}
			return dateTime.RoundToHour(DateTimeRoundMethod.Up);
		}
	}
}
