using System;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Common.Integration.Definition;

public class MetadataItemHwid : Hwid
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

	public int MetadataItemIndex { get; private set; }

	public override string Text
	{
		[DebuggerStepThrough]
		get
		{
			return ToString();
		}
	}

	public MetadataItemHwid()
	{
	}

	public MetadataItemHwid(string hwid)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		string[] array = hwid.Split(':', '.');
		deviceType = Convert.ToInt32(array[1], CultureInfo.InvariantCulture);
		DeviceIndex = Convert.ToInt32(array[2], CultureInfo.InvariantCulture);
		MetadataItemIndex = Convert.ToInt32(array[3], CultureInfo.InvariantCulture);
	}

	public MetadataItemHwid(DeviceTypeEnum deviceType, int deviceIndex, int metadataItemIndex)
	{
		this.deviceType = (int)deviceType;
		DeviceIndex = deviceIndex;
		MetadataItemIndex = metadataItemIndex;
	}

	public override string ToString()
	{
		return GetHwidAsText(deviceType, DeviceIndex, MetadataItemIndex);
	}

	public override int GetHashCode()
	{
		return Text.GetHashCode();
	}

	public static bool operator ==(MetadataItemHwid left, MetadataItemHwid right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(MetadataItemHwid left, MetadataItemHwid right)
	{
		return !(left == right);
	}

	public bool Equals(MetadataItemHwid other)
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
			return MetadataItemIndex == other.MetadataItemIndex;
		}
		return false;
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is MetadataItemHwid other))
		{
			return false;
		}
		return Equals(other);
	}

	public static string GetHwidAsText(DeviceTypeEnum deviceType, int deviceIndex, int itemIndex)
	{
		return GetHwidAsText((int)deviceType, deviceIndex, itemIndex);
	}

	public static string GetHwidAsText(int deviceType, int deviceIndex, int itemIndex)
	{
		return string.Format(CultureInfo.InvariantCulture, "metadataitem:{0}.{1}.{2}", deviceType, deviceIndex, itemIndex);
	}
}
