using System;

namespace VideoOS.Management.Common;

public static class DeviceTypeInfo
{
	public static bool IsRecordingDevice(DeviceType deviceType)
	{
		switch (deviceType)
		{
		case DeviceType.Unknown:
		case DeviceType.Input:
		case DeviceType.Output:
			return false;
		case DeviceType.Camera:
		case DeviceType.Microphone:
		case DeviceType.Speaker:
		case DeviceType.Metadata:
			return true;
		default:
			throw new NotSupportedException();
		}
	}
}
