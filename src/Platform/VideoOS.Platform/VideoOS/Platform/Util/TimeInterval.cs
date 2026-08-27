using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace VideoOS.Platform.Util;

[DebuggerDisplay("{StartTime} - {EndTime}")]
public struct TimeInterval : IEquatable<TimeInterval>
{
	public static readonly TimeInterval Zero;

	private readonly DateTime _startTime;

	private readonly TimeSpan _duration;

	public DateTime StartTime => _startTime;

	public TimeSpan Duration => _duration;

	public DateTime EndTime => _startTime + _duration;

	public DateTimeKind Kind => _startTime.Kind;

	public TimeInterval(DateTime startTime, TimeSpan duration)
	{
		_startTime = startTime;
		_duration = duration;
	}

	public TimeInterval(DateTime startTime, DateTime endTime)
	{
		if (startTime.Kind != endTime.Kind)
		{
			throw new ArgumentException("Both start and end time must have the same DateTime.Kind");
		}
		_startTime = startTime;
		_duration = endTime - startTime;
	}

	public TimeInterval ToUniversalTime()
	{
		return new TimeInterval(StartTime.ToUniversalTime(), Duration);
	}

	public TimeInterval ToLocalTime()
	{
		return new TimeInterval(StartTime.ToLocalTime(), Duration);
	}

	public bool OverlapsWith(TimeInterval interval)
	{
		return OverlapsWith(interval.StartTime, interval.EndTime);
	}

	public bool OverlapsWith(DateTime startTime, DateTime endTime)
	{
		if (startTime.Kind != endTime.Kind)
		{
			throw new ArgumentException("Both start and end time must have the same DateTime.Kind");
		}
		if (startTime < EndTime)
		{
			return endTime > StartTime;
		}
		return false;
	}

	public bool OverlapsWith(DateTime startTime, TimeSpan timeSpan)
	{
		return OverlapsWith(startTime, startTime.Add(timeSpan));
	}

	public static bool operator !=(TimeInterval first, TimeInterval second)
	{
		return !first.Equals(second);
	}

	public static bool operator ==(TimeInterval first, TimeInterval second)
	{
		return first.Equals(second);
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", StartTime, EndTime);
	}

	public bool Equals(TimeInterval other)
	{
		if (StartTime == other.StartTime)
		{
			return Duration == other.Duration;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is TimeInterval other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return StartTime.GetHashCode() ^ Duration.GetHashCode();
	}

	public static IEnumerable<TimeInterval> Minimize(IEnumerable<TimeInterval> intervals)
	{
		IOrderedEnumerable<TimeInterval> orderedEnumerable = intervals.OrderBy((TimeInterval interval) => interval.StartTime);
		DateTime? dateTime = null;
		DateTime? dateTime2 = null;
		foreach (TimeInterval currentInterval in orderedEnumerable)
		{
			if (!dateTime.HasValue)
			{
				dateTime = currentInterval.StartTime;
			}
			if (!dateTime2.HasValue)
			{
				dateTime2 = currentInterval.EndTime;
			}
			if (dateTime2 >= currentInterval.StartTime)
			{
				if (dateTime2 < currentInterval.EndTime)
				{
					dateTime2 = currentInterval.EndTime;
				}
			}
			else
			{
				yield return new TimeInterval(dateTime.Value, dateTime2.Value);
				dateTime = currentInterval.StartTime;
				dateTime2 = currentInterval.EndTime;
			}
		}
		if (dateTime.HasValue && dateTime2.HasValue)
		{
			yield return new TimeInterval(dateTime.Value, dateTime2.Value);
		}
	}

	static TimeInterval()
	{
		Zero = new TimeInterval(DateTime.MinValue, TimeSpan.Zero);
	}
}
