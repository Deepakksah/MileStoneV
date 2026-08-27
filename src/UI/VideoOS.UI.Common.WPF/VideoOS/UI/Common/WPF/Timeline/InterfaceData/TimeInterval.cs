using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public class TimeInterval : IComparable<TimeInterval>
{
	public static readonly TimeInterval Empty = new TimeInterval(DateTime.MinValue, TimeSpan.MinValue);

	public DateTime BeginTime { get; set; }

	public TimeSpan TimeSpan { get; set; }

	public DateTime MiddleTime => BeginTime.AddMilliseconds(TimeSpan.TotalMilliseconds / 2.0);

	public DateTime EndTime
	{
		get
		{
			if (TimeSpan == TimeSpan.MaxValue)
			{
				return DateTime.MaxValue;
			}
			return BeginTime.Add(TimeSpan);
		}
		set
		{
			TimeSpan = value - BeginTime;
		}
	}

	public TimeSpan Radius
	{
		get
		{
			return TimeSpan.FromTicks(TimeSpan.Ticks / 2);
		}
		set
		{
			DateTime middleTime = MiddleTime;
			BeginTime = middleTime - value;
			EndTime = middleTime + value;
		}
	}

	public TimeInterval()
	{
	}

	public TimeInterval(DateTime beginTime, TimeSpan timeSpan)
	{
		if (beginTime.Year == 9999)
		{
			Debugger.Break();
		}
		BeginTime = beginTime;
		TimeSpan = timeSpan;
	}

	public TimeInterval(DateTime beginTime, TimeSpan timeSpan, TimeSpan offset)
	{
		if (beginTime.Year == 9999)
		{
			Debugger.Break();
		}
		BeginTime = beginTime.Add(offset);
		TimeSpan = timeSpan;
	}

	public TimeInterval(DateTime beginTime, DateTime endTime)
	{
		if (beginTime.Year == 9999)
		{
			Debugger.Break();
		}
		BeginTime = beginTime;
		EndTime = endTime;
	}

	public TimeInterval(TimeInterval other)
		: this(other.BeginTime, other.TimeSpan)
	{
	}

	public TimeInterval(DateTime beginTime, double timeSpan)
		: this(beginTime, TimeSpan.FromMilliseconds(timeSpan))
	{
	}

	public TimeInterval(DateTime beginTime, double timeSpan, double offset)
		: this(beginTime, TimeSpan.FromMilliseconds(timeSpan), TimeSpan.FromMilliseconds(offset))
	{
	}

	public TimeInterval(DateTime beginTime, TimeSpan timeSpan, double offset)
		: this(beginTime, timeSpan, TimeSpan.FromMilliseconds(offset))
	{
	}

	public TimeInterval(long beginTimeTicks, double timeSpanMs)
		: this(new DateTime(beginTimeTicks, DateTimeKind.Utc), TimeSpan.FromMilliseconds(timeSpanMs))
	{
	}

	public TimeInterval(long beginTimeTicks, long endTimeTicks)
		: this(new DateTime(beginTimeTicks, DateTimeKind.Utc), new DateTime(endTimeTicks, DateTimeKind.Utc))
	{
	}

	public IEnumerable<TimeInterval> Substract(TimeInterval other)
	{
		if (!IntersectsWith(other))
		{
			yield return this;
		}
		else if (other.EndTime > BeginTime)
		{
			if (BeginTime < other.BeginTime)
			{
				yield return new TimeInterval
				{
					BeginTime = BeginTime,
					EndTime = other.BeginTime
				};
			}
			if (other.EndTime < EndTime)
			{
				yield return new TimeInterval
				{
					BeginTime = other.EndTime,
					EndTime = EndTime
				};
			}
		}
	}

	public IEnumerable<TimeInterval> Substract(IEnumerable<TimeInterval> intervalList)
	{
		DateTime dateTime = BeginTime;
		foreach (TimeInterval interval in intervalList)
		{
			if (IntersectsWith(interval))
			{
				if (interval.BeginTime >= dateTime)
				{
					if (!(interval.BeginTime < EndTime))
					{
						break;
					}
					if (interval.BeginTime > dateTime)
					{
						yield return new TimeInterval
						{
							BeginTime = dateTime,
							EndTime = interval.BeginTime
						};
					}
					dateTime = interval.EndTime;
				}
				else
				{
					dateTime = ((EndTime < interval.EndTime) ? EndTime : interval.EndTime);
				}
			}
			if (dateTime >= EndTime)
			{
				break;
			}
		}
		if (dateTime < EndTime)
		{
			yield return new TimeInterval
			{
				BeginTime = dateTime,
				EndTime = EndTime
			};
		}
	}

	public TimeInterval IntersectWith(TimeInterval other)
	{
		return this.IntersectWith(other, (TimeInterval t) => new TimeInterval(t));
	}

	public bool IntersectsWith(DateTime beginTime, DateTime endTime)
	{
		return IntersectsWith(beginTime, endTime, beginInclusive: false, endInclusive: false);
	}

	public bool IntersectsWith(DateTime beginTime, DateTime endTime, bool beginInclusive, bool endInclusive)
	{
		if (beginInclusive ? (beginTime <= EndTime) : (beginTime < EndTime))
		{
			if (!endInclusive)
			{
				return endTime > BeginTime;
			}
			return endTime >= BeginTime;
		}
		return false;
	}

	public bool IntersectsWith(TimeInterval interval)
	{
		return IntersectsWith(interval.BeginTime, interval.EndTime);
	}

	public bool IntersectsWith(DateTime beginTime, TimeSpan timeSpan)
	{
		return IntersectsWith(beginTime, beginTime.Add(timeSpan));
	}

	public int CompareTo(TimeInterval other)
	{
		int num = BeginTime.CompareTo(other.BeginTime);
		if (num == 0)
		{
			return TimeSpan.CompareTo(other.TimeSpan);
		}
		return num;
	}

	public override bool Equals(object obj)
	{
		if (obj is TimeInterval)
		{
			TimeInterval timeInterval = obj as TimeInterval;
			if (BeginTime.Equals(timeInterval.BeginTime))
			{
				return EndTime.Equals(timeInterval.EndTime);
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return BeginTime.GetHashCode();
	}

	public override string ToString()
	{
		return $"({BeginTime} - {EndTime})";
	}

	public void ToUniversalTime()
	{
		BeginTime = BeginTime.ToUniversalTime();
	}

	public void ToLocalTime()
	{
		BeginTime = BeginTime.ToLocalTime();
	}
}
