using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.Utilities;

public class TimeSelector
{
	public static DateTime GetNearestTime(IEnumerable<DateTime> dateTimes, PlaybackParamsHelper.SeekType seekType, DateTime centralTime)
	{
		if (dateTimes == null || !dateTimes.Any())
		{
			return DateTime.MinValue;
		}
		switch (seekType)
		{
		case PlaybackParamsHelper.SeekType.DatabaseEnd:
			return dateTimes.Last();
		default:
			return dateTimes.First();
		case PlaybackParamsHelper.SeekType.Time:
		case PlaybackParamsHelper.SeekType.TimeOrBefore:
		case PlaybackParamsHelper.SeekType.TimeOrAfter:
		case PlaybackParamsHelper.SeekType.TimeBefore:
		case PlaybackParamsHelper.SeekType.TimeAfter:
		{
			DateTime result = DateTime.MinValue;
			DateTimeAndDiffs[] array = dateTimes.Select((DateTime dateTime) => new DateTimeAndDiffs(dateTime, centralTime - dateTime)).ToArray();
			switch (seekType)
			{
			case PlaybackParamsHelper.SeekType.Time:
			{
				DateTimeAndDiffs[] array2 = array.Select((DateTimeAndDiffs dateTimeAndDiff) => new DateTimeAndDiffs(dateTimeAndDiff.DateTime, TimeSpan.FromTicks(System.Math.Abs(dateTimeAndDiff.TimeSpan.Ticks)))).ToArray();
				if (array2.Length != 0)
				{
					result = array2.MinBy((DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff, new DateTimeAndDiffsTimeSpanComparer()).DateTime;
				}
				break;
			}
			case PlaybackParamsHelper.SeekType.TimeAfter:
				result = SelectFirstNegative(array, (DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff.TimeSpan.Ticks < 0);
				break;
			case PlaybackParamsHelper.SeekType.TimeOrAfter:
				result = SelectFirstNegative(array, (DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff.TimeSpan.Ticks <= 0);
				break;
			case PlaybackParamsHelper.SeekType.TimeBefore:
				result = SelectFirstPositive(array, (DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff.TimeSpan.Ticks > 0);
				break;
			case PlaybackParamsHelper.SeekType.TimeOrBefore:
				result = SelectFirstPositive(array, (DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff.TimeSpan.Ticks >= 0);
				break;
			default:
				Logger.Instance.Log(LogContent.Error, "TimeSelector.GetNearestTime", $"Invalid seekType: {seekType}");
				result = dateTimes.First();
				break;
			}
			return result;
		}
		}
	}

	private static DateTime SelectFirstNegative(DateTimeAndDiffs[] differences, Func<DateTimeAndDiffs, bool> condition)
	{
		DateTime result = DateTime.MinValue;
		DateTimeAndDiffs[] array = differences.Select((DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff).Where(condition).ToArray();
		if (array.Length != 0)
		{
			result = array.MaxBy((DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff, new DateTimeAndDiffsTimeSpanComparer()).DateTime;
		}
		return result;
	}

	private static DateTime SelectFirstPositive(DateTimeAndDiffs[] differences, Func<DateTimeAndDiffs, bool> condition)
	{
		DateTime result = DateTime.MinValue;
		DateTimeAndDiffs[] array = differences.Select((DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff).Where(condition).ToArray();
		if (array.Length != 0)
		{
			result = array.MinBy((DateTimeAndDiffs dateTimeAndDiff) => dateTimeAndDiff, new DateTimeAndDiffsTimeSpanComparer()).DateTime;
		}
		return result;
	}
}
