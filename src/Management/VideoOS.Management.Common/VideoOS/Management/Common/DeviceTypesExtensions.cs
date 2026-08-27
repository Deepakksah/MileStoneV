using System;

namespace VideoOS.Management.Common;

public static class DeviceTypesExtensions
{
	private enum BuiltInDeviceType
	{
		Unknown = -1,
		Camera = 0,
		AudioInput = 1,
		Input = 2,
		Output = 3,
		AudioOutput = 4,
		Metadata = 8
	}

	public static DeviceType ToDeviceType(this DeviceTypesOld value)
	{
		return value switch
		{
			DeviceTypesOld.Unknown => DeviceType.Unknown, 
			DeviceTypesOld.Camera => DeviceType.Camera, 
			DeviceTypesOld.Input => DeviceType.Input, 
			DeviceTypesOld.Output => DeviceType.Output, 
			DeviceTypesOld.Microphone => DeviceType.Microphone, 
			DeviceTypesOld.Speaker => DeviceType.Speaker, 
			_ => throw new ArgumentException("The value is not supported", "value"), 
		};
	}

	public static DeviceTypesOld ToDeviceType(this DeviceType value)
	{
		return value switch
		{
			DeviceType.Camera => DeviceTypesOld.Camera, 
			DeviceType.Input => DeviceTypesOld.Input, 
			DeviceType.Output => DeviceTypesOld.Output, 
			DeviceType.Microphone => DeviceTypesOld.Microphone, 
			DeviceType.Speaker => DeviceTypesOld.Speaker, 
			_ => DeviceTypesOld.Unknown, 
		};
	}

	public static string ToBuiltInDeviceType(this DeviceType value)
	{
		BuiltInDeviceType builtInDeviceType = (BuiltInDeviceType)value;
		if (!Enum.IsDefined(typeof(BuiltInDeviceType), builtInDeviceType))
		{
			builtInDeviceType = BuiltInDeviceType.Unknown;
		}
		return builtInDeviceType.ToString();
	}

	public static DeviceType FromBuiltInDeviceType(string value)
	{
		if (!Enum.TryParse<BuiltInDeviceType>(value, out var result))
		{
			result = BuiltInDeviceType.Unknown;
		}
		DeviceType deviceType = (DeviceType)result;
		if (!Enum.IsDefined(typeof(DeviceType), deviceType))
		{
			deviceType = DeviceType.Unknown;
		}
		return deviceType;
	}
}
