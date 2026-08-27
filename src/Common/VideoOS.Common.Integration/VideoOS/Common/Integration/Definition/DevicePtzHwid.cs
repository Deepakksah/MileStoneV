using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Common.Integration.Definition;

public class DevicePtzHwid : Hwid
{
	private int type;

	public DeviceTypeEnum Type
	{
		[DebuggerStepThrough]
		get
		{
			return (DeviceTypeEnum)type;
		}
		[DebuggerStepThrough]
		set
		{
			type = (int)value;
		}
	}

	public int Index { get; private set; }

	public override string Text
	{
		[DebuggerStepThrough]
		get
		{
			return ToString();
		}
	}

	public DevicePtzHwid()
	{
	}

	public DevicePtzHwid(string hwid)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		string[] array = hwid.Split(':', '.');
		type = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
		Index = Convert.ToInt32(array[2], CultureInfo.InvariantCulture);
	}

	public DevicePtzHwid(DeviceTypeEnum deviceType, int index)
	{
		type = (int)deviceType;
		Index = index;
	}

	public override string ToString()
	{
		return GetHwidAsText(type, Index);
	}

	public override int GetHashCode()
	{
		return Text.GetHashCode();
	}

	public static bool operator ==(DevicePtzHwid left, DevicePtzHwid right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(DevicePtzHwid left, DevicePtzHwid right)
	{
		return !(left == right);
	}

	public bool Equals(DevicePtzHwid other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		if (Type == other.Type)
		{
			return Index == other.Index;
		}
		return false;
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is DevicePtzHwid other))
		{
			return false;
		}
		return Equals(other);
	}

	public static string GetHwidAsText(DeviceTypeEnum deviceType, int index)
	{
		return GetHwidAsText((int)deviceType, index);
	}

	public static string GetHwidAsText(int deviceType, int index)
	{
		return string.Format(CultureInfo.InvariantCulture, "deviceptz:{0}.{1}", deviceType, index);
	}
}
