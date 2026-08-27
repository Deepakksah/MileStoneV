using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Definition;

public class HardwareHwid : Hwid
{
	private string hwid;

	public override string Text
	{
		[DebuggerStepThrough]
		get
		{
			return ToString();
		}
	}

	public HardwareHwid(string hwid)
	{
		this.hwid = hwid;
	}

	public override string ToString()
	{
		return hwid;
	}

	public override int GetHashCode()
	{
		return hwid.GetHashCode();
	}

	public static bool operator ==(HardwareHwid left, HardwareHwid right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(HardwareHwid left, HardwareHwid right)
	{
		return !(left == right);
	}

	public bool Equals(HardwareHwid other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		return string.Equals(hwid, other.hwid, StringComparison.Ordinal);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is HardwareHwid other))
		{
			return false;
		}
		return Equals(other);
	}
}
