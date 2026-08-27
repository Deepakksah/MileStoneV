using System;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class TimeSpanExtensions
{
	public static TimeSpan RoundToMillisecond(this TimeSpan timeSpan, TimeSpanRoundMethod roundMethod)
	{
		return roundMethod switch
		{
			TimeSpanRoundMethod.Down => TimeSpan.FromMilliseconds(Math.Floor(timeSpan.TotalMilliseconds)), 
			TimeSpanRoundMethod.Up => TimeSpan.FromMilliseconds(Math.Ceiling(timeSpan.TotalMilliseconds)), 
			_ => TimeSpan.FromMilliseconds(Math.Round(timeSpan.TotalMilliseconds, MidpointRounding.AwayFromZero)), 
		};
	}

	public static TimeSpan RoundToSecond(this TimeSpan timeSpan, TimeSpanRoundMethod roundMethod)
	{
		return roundMethod switch
		{
			TimeSpanRoundMethod.Down => TimeSpan.FromSeconds(Math.Floor(timeSpan.TotalSeconds)), 
			TimeSpanRoundMethod.Up => TimeSpan.FromSeconds(Math.Ceiling(timeSpan.TotalSeconds)), 
			_ => TimeSpan.FromSeconds(Math.Round(timeSpan.TotalSeconds, MidpointRounding.AwayFromZero)), 
		};
	}

	public static TimeSpan RoundToMinute(this TimeSpan timeSpan, TimeSpanRoundMethod roundMethod)
	{
		return roundMethod switch
		{
			TimeSpanRoundMethod.Down => TimeSpan.FromMinutes(Math.Floor(timeSpan.TotalMinutes)), 
			TimeSpanRoundMethod.Up => TimeSpan.FromMinutes(Math.Ceiling(timeSpan.TotalMinutes)), 
			_ => TimeSpan.FromMinutes(Math.Round(timeSpan.TotalMinutes, MidpointRounding.AwayFromZero)), 
		};
	}

	public static TimeSpan RoundToHour(this TimeSpan timeSpan, TimeSpanRoundMethod roundMethod)
	{
		return roundMethod switch
		{
			TimeSpanRoundMethod.Down => TimeSpan.FromHours(Math.Floor(timeSpan.TotalHours)), 
			TimeSpanRoundMethod.Up => TimeSpan.FromHours(Math.Ceiling(timeSpan.TotalHours)), 
			_ => TimeSpan.FromHours(Math.Round(timeSpan.TotalHours, MidpointRounding.AwayFromZero)), 
		};
	}
}
