using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Definition;

public class DeviceTypeComparer : IComparer<DeviceTypeEnum>
{
	public int Compare(DeviceTypeEnum x, DeviceTypeEnum y)
	{
		int num = DeviceTypeToSortOrder(x);
		int value = DeviceTypeToSortOrder(y);
		return num.CompareTo(value);
	}

	public static int DeviceTypeToSortOrder(DeviceTypeEnum deviceTypeEnum)
	{
		return deviceTypeEnum switch
		{
			DeviceTypeEnum.Camera => 0, 
			DeviceTypeEnum.AudioInput => 1, 
			DeviceTypeEnum.AudioOutput => 2, 
			DeviceTypeEnum.Metadata => 3, 
			DeviceTypeEnum.Input => 4, 
			DeviceTypeEnum.Output => 5, 
			_ => throw new ArgumentException("DeviceTypeToSortOrder", "deviceTypeEnum"), 
		};
	}
}
