using System;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public static class DeviceConfigurationFactory
{
	private const string _cameraType = "camera";

	private const string _microphoneType = "microphone";

	private const string _speakerType = "speaker";

	private const string _metadataType = "metadata";

	private const string _inputType = "inputevent";

	private const string _outputType = "output";

	public static string GetItemType(string type)
	{
		return type switch
		{
			"camera" => "Camera", 
			"microphone" => "Microphone", 
			"speaker" => "Speaker", 
			"metadata" => "Metadata", 
			"inputevent" => "InputEvent", 
			"output" => "Output", 
			_ => throw new ArgumentException("Invalid input type"), 
		};
	}

	public static DeviceConfiguration GetDevice(string type)
	{
		return type switch
		{
			"camera" => new CameraConfiguration(), 
			"microphone" => new MicrophoneConfiguration(), 
			"speaker" => new SpeakerConfiguration(), 
			"metadata" => new MetadataConfiguration(), 
			"inputevent" => new InputConfiguration(), 
			"output" => new OutputConfiguration(), 
			_ => throw new ArgumentException("Invalid input type"), 
		};
	}

	public static string GetType(DeviceConfiguration device)
	{
		if (device is CameraConfiguration)
		{
			return "camera";
		}
		if (device is MicrophoneConfiguration)
		{
			return "microphone";
		}
		if (device is SpeakerConfiguration)
		{
			return "speaker";
		}
		if (device is MetadataConfiguration)
		{
			return "metadata";
		}
		if (device is InputConfiguration)
		{
			return "inputevent";
		}
		if (device is OutputConfiguration)
		{
			return "output";
		}
		throw new ArgumentException("Invalid input type");
	}
}
