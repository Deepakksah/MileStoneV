using System;
using System.Threading;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public class SourceQuery : TimeInterval, IComparable<SourceQuery>
{
	public static readonly long PriorityIgnore = long.MaxValue;

	public static readonly long PriorityLowest = 9223372036854775806L;

	private long _priority;

	public ISequenceSource Source { get; set; }

	public long Priority
	{
		get
		{
			return Interlocked.Read(ref _priority);
		}
		set
		{
			Interlocked.Exchange(ref _priority, value);
		}
	}

	public SourceQuery()
	{
	}

	public SourceQuery(TimeInterval interval)
		: base(interval)
	{
	}

	public SourceQuery(SourceQuery other)
		: base(other)
	{
		Source = other.Source;
	}

	public SourceQuery(SourceQueryResult result)
		: base(result)
	{
		Source = result.Source;
	}

	public SourceQuery(DateTime beginTime, TimeSpan timeSpan)
		: base(beginTime, timeSpan)
	{
	}

	public override bool Equals(object obj)
	{
		if (obj is SourceQuery)
		{
			SourceQuery sourceQuery = obj as SourceQuery;
			bool flag = base.Equals(obj);
			if (flag)
			{
				flag = Source.Equals(sourceQuery.Source);
			}
			return flag;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.BeginTime.GetHashCode() ^ base.EndTime.GetHashCode() ^ Source.GetHashCode();
	}

	public override string ToString()
	{
		return $"({base.BeginTime} - {base.EndTime}) {Source}";
	}

	public int CompareTo(SourceQuery other)
	{
		int num = CompareTo((TimeInterval)other);
		if (num == 0)
		{
			num = Source.SourceId.CompareTo(other.Source.SourceId);
			if (num == 0)
			{
				num = Source.InformationId.CompareTo(other.Source.InformationId);
			}
		}
		return num;
	}
}
