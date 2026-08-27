using System;
using System.Configuration;

namespace VideoOS.Management.Common;

public static class HardwareAccelerationPriorityHelper
{
	public static HardwareAccelerationPriority PriorityConfiguration
	{
		get
		{
			if (Enum.TryParse<HardwareAccelerationPriority>(ConfigurationManager.AppSettings["VideoOS.Management.Common.PriorityConfiguration"], out var result))
			{
				return result;
			}
			return HardwareAccelerationPriority.AutoNvidia;
		}
	}

	public static int ConvertToInt32(HardwareAccelerationPriority value)
	{
		if (Enum.IsDefined(typeof(HardwareAccelerationPriority), value))
		{
			return (int)value;
		}
		return 0;
	}

	public static HardwareAccelerationPriority ConvertFromInt32(int value)
	{
		if (Enum.IsDefined(typeof(HardwareAccelerationPriority), value))
		{
			return (HardwareAccelerationPriority)value;
		}
		return HardwareAccelerationPriority.AutoNvidia;
	}
}
