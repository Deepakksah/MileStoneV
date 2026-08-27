using System;

namespace VideoOS.Common.Integration.Media;

public struct TimeSlice
{
	public DateTime BeginTimestamp;

	public DateTime EndTimestamp;

	public static TimeSlice Invalid => new TimeSlice(DateTime.MaxValue, DateTime.MinValue);

	public bool IsValid => BeginTimestamp <= EndTimestamp;

	public TimeSlice(DateTime beginTimestamp, DateTime endTimestamp)
	{
		BeginTimestamp = beginTimestamp;
		EndTimestamp = endTimestamp;
	}

	public static bool operator ==(TimeSlice left, TimeSlice right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(TimeSlice left, TimeSlice right)
	{
		return !left.Equals(right);
	}

	public override int GetHashCode()
	{
		return BeginTimestamp.GetHashCode();
	}

	public bool Equals(TimeSlice other)
	{
		if (BeginTimestamp == other.BeginTimestamp)
		{
			return EndTimestamp == other.EndTimestamp;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		return Equals((TimeSlice)obj);
	}
}
