using System;

namespace VideoOS.Management.Common;

public static class DeviceTypeUtilities
{
	public static bool IsRecordingDevice(DeviceType deviceType)
	{
		if ((uint)deviceType <= 1u || deviceType == DeviceType.Speaker || deviceType == DeviceType.Metadata)
		{
			return true;
		}
		return false;
	}

	public static bool IsAudioDevice(DeviceType deviceType)
	{
		if (deviceType == DeviceType.Microphone || deviceType == DeviceType.Speaker)
		{
			return true;
		}
		return false;
	}

	public static string ResolveDeviceEntityNameFromDeviceType(DeviceType deviceType)
	{
		return deviceType switch
		{
			DeviceType.Camera => "CameraDevice", 
			DeviceType.Microphone => "MicrophoneDevice", 
			DeviceType.Speaker => "SpeakerDevice", 
			DeviceType.Input => "InputDevice", 
			DeviceType.Output => "OutputDevice", 
			DeviceType.Metadata => "MetadataDevice", 
			DeviceType.Unknown => throw new ArgumentException("The device type 'Unknown' is not a valid argument"), 
			_ => throw new NotImplementedException("The implementation of this function was not updated after defining a new device type."), 
		};
	}

	public static DeviceType ResolveDeviceTypeFromDeviceEntityName(string entityName)
	{
		return entityName switch
		{
			"CameraDevice" => DeviceType.Camera, 
			"MicrophoneDevice" => DeviceType.Microphone, 
			"SpeakerDevice" => DeviceType.Speaker, 
			"InputDevice" => DeviceType.Input, 
			"OutputDevice" => DeviceType.Output, 
			"MetadataDevice" => DeviceType.Metadata, 
			_ => throw new ArgumentException("The implementation of this function was not updated after defining a new device group entity name.", "entityName"), 
		};
	}

	public static string ResolveDeviceGroupEntityNameFromDeviceType(DeviceType deviceType)
	{
		return deviceType switch
		{
			DeviceType.Camera => "CameraDeviceGroup", 
			DeviceType.Microphone => "MicrophoneDeviceGroup", 
			DeviceType.Speaker => "SpeakerDeviceGroup", 
			DeviceType.Input => "InputDeviceGroup", 
			DeviceType.Output => "OutputDeviceGroup", 
			DeviceType.Metadata => "MetadataDeviceGroup", 
			DeviceType.Unknown => throw new ArgumentException("The device type 'Unknown' is not a valid argument"), 
			_ => throw new NotImplementedException("The implementation of this function was not updated after defining a new device type."), 
		};
	}

	public static DeviceType ResolveDeviceGroupDeviceTypeFromEntityName(string entityName)
	{
		return entityName switch
		{
			"CameraDeviceGroup" => DeviceType.Camera, 
			"MicrophoneDeviceGroup" => DeviceType.Microphone, 
			"SpeakerDeviceGroup" => DeviceType.Speaker, 
			"InputDeviceGroup" => DeviceType.Input, 
			"OutputDeviceGroup" => DeviceType.Output, 
			"MetadataDeviceGroup" => DeviceType.Metadata, 
			_ => throw new NotImplementedException("The implementation of this function was not updated after defining a new device group entity name."), 
		};
	}
}
