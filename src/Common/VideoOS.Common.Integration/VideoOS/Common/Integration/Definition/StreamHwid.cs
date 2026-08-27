using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Common.Integration.Definition;

public class StreamHwid : Hwid
{
	private int deviceType;

	public int TypeNo => deviceType;

	public DeviceTypeEnum Type
	{
		[DebuggerStepThrough]
		get
		{
			return (DeviceTypeEnum)deviceType;
		}
		[DebuggerStepThrough]
		set
		{
			deviceType = (int)value;
		}
	}

	public int DeviceIndex { get; private set; }

	public int StreamIndex { get; private set; }

	public override string Text
	{
		[DebuggerStepThrough]
		get
		{
			return ToString();
		}
	}

	public StreamHwid()
	{
	}

	public StreamHwid(string hwid)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		string[] array = hwid.Split(':', '.');
		deviceType = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
		DeviceIndex = Convert.ToInt32(array[2], CultureInfo.InvariantCulture);
		StreamIndex = Convert.ToInt32(array[3], CultureInfo.InvariantCulture);
	}

	public StreamHwid(DeviceTypeEnum deviceType, int deviceIndex, int streamIndex)
	{
		this.deviceType = (int)deviceType;
		DeviceIndex = deviceIndex;
		StreamIndex = streamIndex;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "stream:{0}.{1}.{2}", deviceType, DeviceIndex, StreamIndex);
	}

	public override int GetHashCode()
	{
		return Text.GetHashCode();
	}

	public static bool operator ==(StreamHwid left, StreamHwid right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(StreamHwid left, StreamHwid right)
	{
		return !(left == right);
	}

	public bool Equals(StreamHwid other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		if (Type == other.Type && DeviceIndex == other.DeviceIndex)
		{
			return StreamIndex == other.StreamIndex;
		}
		return false;
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is StreamHwid other))
		{
			return false;
		}
		return Equals(other);
	}
}
