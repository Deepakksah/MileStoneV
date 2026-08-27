using System;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public class Sequence : TimeInterval, IComparable<Sequence>
{
	public object Tag { get; set; }

	public Sequence()
	{
	}

	public Sequence(Sequence other)
		: base(other)
	{
		Tag = other.Tag;
	}

	public override string ToString()
	{
		return $"({base.BeginTime} - {base.EndTime})";
	}

	public int CompareTo(Sequence other)
	{
		return CompareTo((TimeInterval)other);
	}
}
