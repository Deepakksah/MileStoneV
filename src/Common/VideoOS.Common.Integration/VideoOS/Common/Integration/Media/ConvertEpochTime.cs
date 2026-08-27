using System;

namespace VideoOS.Common.Integration.Media;

public static class ConvertEpochTime
{
	private static long TICKS_TO_EPOCH_1970 = 621355968000000000L;

	public static DateTime EpochTime2DateTime(long msSinceEpochUtc)
	{
		if (msSinceEpochUtc < 0)
		{
			msSinceEpochUtc = 0L;
		}
		return new DateTime(msSinceEpochUtc * 10000 + TICKS_TO_EPOCH_1970, DateTimeKind.Utc);
	}

	public static long DateTime2EpochTime(DateTime utcTime)
	{
		if (utcTime.Ticks <= TICKS_TO_EPOCH_1970)
		{
			return 0L;
		}
		return (utcTime.Ticks - TICKS_TO_EPOCH_1970) / 10000;
	}
}
