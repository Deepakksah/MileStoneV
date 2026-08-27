using System;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public static class BoxTimeConvertor
{
	public static readonly DateTime BaseDateTimeUtc = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DateTime FromBoxTime(ulong boxTime)
	{
		return BaseDateTimeUtc.AddSeconds(boxTime);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong ToBoxTime(this DateTime dateTime)
	{
		return (ulong)(dateTime - BaseDateTimeUtc).TotalSeconds;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TimeSpan FromBoxDuration(ulong boxDuration, ulong timeScale)
	{
		return TimeSpan.FromSeconds((double)boxDuration / (double)timeScale);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ulong ToBoxDuration(this TimeSpan duration, ulong timeScale)
	{
		return (ulong)(duration.TotalSeconds * (double)timeScale);
	}
}
