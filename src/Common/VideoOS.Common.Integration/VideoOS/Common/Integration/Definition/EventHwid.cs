using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoOS.Common.Integration.Definition;

public class EventHwid : Hwid
{
	public int Index { get; private set; }

	public override string Text
	{
		[DebuggerStepThrough]
		get
		{
			return ToString();
		}
	}

	public EventHwid()
	{
	}

	public EventHwid(string hwid)
	{
		Match match = Regex.Match(hwid, "^event:(\\d+)");
		if (!match.Success)
		{
			throw new ArgumentException("Event Hwid must have the format 'event:{EventIndex}'", "hwid");
		}
		Index = Convert.ToInt32(match.Groups[1].Value, CultureInfo.InvariantCulture);
	}

	public EventHwid(int index)
	{
		Index = index;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "event:{0}", Index);
	}

	public override int GetHashCode()
	{
		return Text.GetHashCode();
	}

	public static bool operator ==(EventHwid left, EventHwid right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(EventHwid left, EventHwid right)
	{
		return !(left == right);
	}

	public bool Equals(EventHwid other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		return Index == other.Index;
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is EventHwid other))
		{
			return false;
		}
		return Equals(other);
	}
}
