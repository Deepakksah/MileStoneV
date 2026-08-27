using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public static class DatabaseDeviceTypeStrings
{
	public const string Unknown = "Unknown";

	public const string Camera = "Camera";

	public const string Microphone = "AudioInput";

	public const string Metadata = "Metadata";

	public const string Speaker = "AudioOutput";

	public const string Input = "Input";

	public const string Output = "Output";

	public static string[] GetDeviceTypeStrings(DeviceType[] deviceTypes)
	{
		return deviceTypes.Select((DeviceType d) => GetDeviceTypeString(d)).ToArray();
	}

	public static string GetDeviceTypeString(DeviceType deviceType)
	{
		return deviceType switch
		{
			DeviceType.Camera => "Camera", 
			DeviceType.Microphone => "AudioInput", 
			DeviceType.Metadata => "Metadata", 
			DeviceType.Input => "Input", 
			DeviceType.Output => "Output", 
			DeviceType.Speaker => "AudioOutput", 
			_ => "Unknown", 
		};
	}

	public static string GetDeviceTypeString(DeviceTypeSearch deviceType)
	{
		return deviceType switch
		{
			DeviceTypeSearch.Camera => "Camera", 
			DeviceTypeSearch.Microphone => "AudioInput", 
			DeviceTypeSearch.Metadata => "Metadata", 
			DeviceTypeSearch.Input => "Input", 
			DeviceTypeSearch.Output => "Output", 
			DeviceTypeSearch.Speaker => "AudioOutput", 
			_ => "Unknown", 
		};
	}

	public static DeviceType[] GetDeviceTypeEnums(string[] deviceTypes)
	{
		return deviceTypes.Select((string s) => GetDeviceTypeEnum(s)).ToArray();
	}

	public static DeviceType GetDeviceTypeEnum(string deviceType)
	{
		return deviceType switch
		{
			"Camera" => DeviceType.Camera, 
			"AudioInput" => DeviceType.Microphone, 
			"Metadata" => DeviceType.Metadata, 
			"Input" => DeviceType.Input, 
			"Output" => DeviceType.Output, 
			"AudioOutput" => DeviceType.Speaker, 
			_ => DeviceType.Unknown, 
		};
	}
}
