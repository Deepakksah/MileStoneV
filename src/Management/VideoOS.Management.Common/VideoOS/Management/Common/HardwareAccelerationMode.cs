using System;

namespace VideoOS.Management.Common;

public static class HardwareAccelerationMode
{
	public enum HardwareAccelerationModeType
	{
		Off,
		Automatic
	}

	public static int ConvertToInt32(HardwareAccelerationModeType value)
	{
		if (Enum.IsDefined(typeof(HardwareAccelerationModeType), value))
		{
			return (int)value;
		}
		return 0;
	}

	public static HardwareAccelerationModeType ConvertFromInt32(int value)
	{
		if (Enum.IsDefined(typeof(HardwareAccelerationModeType), value))
		{
			return (HardwareAccelerationModeType)value;
		}
		return HardwareAccelerationModeType.Off;
	}
}
