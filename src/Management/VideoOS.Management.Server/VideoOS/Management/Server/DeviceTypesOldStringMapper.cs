using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public static class DeviceTypesOldStringMapper
{
	public static string MapToDbString(DeviceTypesOld value)
	{
		return value switch
		{
			DeviceTypesOld.Camera => "Camera", 
			DeviceTypesOld.Input => "Input", 
			DeviceTypesOld.Output => "Output", 
			DeviceTypesOld.Microphone => "AudioInput", 
			DeviceTypesOld.Speaker => "AudioOutput", 
			_ => string.Empty, 
		};
	}

	public static DeviceTypesOld[] MapToDeviceTypesArray(string[] value)
	{
		return value.Select((string s) => MapToDeviceTypes(s)).ToArray();
	}

	public static DeviceTypesOld MapToDeviceTypes(string value)
	{
		return value switch
		{
			"Camera" => DeviceTypesOld.Camera, 
			"Input" => DeviceTypesOld.Input, 
			"Output" => DeviceTypesOld.Output, 
			"AudioInput" => DeviceTypesOld.Microphone, 
			"AudioOutput" => DeviceTypesOld.Speaker, 
			_ => DeviceTypesOld.Unknown, 
		};
	}

	public static DeviceType[] MapToDeviceTypeArray(string[] value)
	{
		return value.Select((string s) => MapToDeviceType(s)).ToArray();
	}

	public static DeviceType MapToDeviceType(string value)
	{
		return value switch
		{
			"Camera" => DeviceType.Camera, 
			"Input" => DeviceType.Input, 
			"Output" => DeviceType.Output, 
			"AudioInput" => DeviceType.Microphone, 
			"AudioOutput" => DeviceType.Speaker, 
			_ => DeviceType.Unknown, 
		};
	}
}
