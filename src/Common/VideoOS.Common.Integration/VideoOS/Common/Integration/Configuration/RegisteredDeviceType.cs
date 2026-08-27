using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Configuration;

public class RegisteredDeviceType
{
	public class BuiltInTypes
	{
		public const string Unknown = "Unknown";

		public const string Camera = "Camera";

		public const string AudioInput = "AudioInput";

		public const string Input = "Input";

		public const string Output = "Output";

		public const string AudioOutput = "AudioOutput";

		public const string Metadata = "Metadata";

		public static string[] GetAllKnownTypes()
		{
			return new string[6] { "Camera", "AudioInput", "AudioOutput", "Metadata", "Input", "Output" };
		}

		private BuiltInTypes()
		{
		}
	}

	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RegisteredDeviceTypesRow _datarow;

	public ConfigurationDataSet.RegisteredDeviceTypesRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public string DeviceType
	{
		get
		{
			return _datarow.DeviceType.Trim();
		}
		set
		{
			_datarow.DeviceType = value;
		}
	}

	public string Description
	{
		get
		{
			return _datarow.Description;
		}
		set
		{
			_datarow.Description = value;
		}
	}

	public int SortOrder
	{
		get
		{
			return _datarow.SortOrder;
		}
		set
		{
			_datarow.SortOrder = value;
		}
	}

	public int PrimaryIconNumber
	{
		get
		{
			return _datarow.PrimaryIconNumber;
		}
		set
		{
			_datarow.PrimaryIconNumber = value;
		}
	}

	public bool IsRecordingDevice
	{
		get
		{
			return _datarow.IsRecordingDevice;
		}
		set
		{
			_datarow.IsRecordingDevice = value;
		}
	}

	public RegisteredDeviceType(ConfigurationManager configurationManager, ConfigurationDataSet.RegisteredDeviceTypesRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
	}

	public static bool IsGeographicalPositionedType(string deviceType)
	{
		if (!IsCamera(deviceType) && !IsInput(deviceType) && !IsOutput(deviceType))
		{
			return IsMicrophone(deviceType);
		}
		return true;
	}

	public static string GetDeviceTypeString(DeviceTypeEnum deviceType)
	{
		return deviceType switch
		{
			DeviceTypeEnum.Camera => "Camera", 
			DeviceTypeEnum.AudioInput => "AudioInput", 
			DeviceTypeEnum.Input => "Input", 
			DeviceTypeEnum.Output => "Output", 
			DeviceTypeEnum.AudioOutput => "AudioOutput", 
			DeviceTypeEnum.Metadata => "Metadata", 
			_ => "Unknown", 
		};
	}

	public static DeviceTypeEnum GetDeviceTypeEnum(string deviceType)
	{
		return deviceType switch
		{
			"Camera" => DeviceTypeEnum.Camera, 
			"AudioInput" => DeviceTypeEnum.AudioInput, 
			"Input" => DeviceTypeEnum.Input, 
			"Output" => DeviceTypeEnum.Output, 
			"AudioOutput" => DeviceTypeEnum.AudioOutput, 
			"Metadata" => DeviceTypeEnum.Metadata, 
			_ => DeviceTypeEnum.Unknown, 
		};
	}

	public static bool IsCamera(string deviceType)
	{
		return string.Equals(deviceType, "Camera", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsOutput(string deviceType)
	{
		return string.Equals(deviceType, "Output", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsInput(string deviceType)
	{
		return string.Equals(deviceType, "Input", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsMicrophone(string deviceType)
	{
		return IsAudioInput(deviceType);
	}

	public static bool IsAudioInput(string deviceType)
	{
		return string.Equals(deviceType, "AudioInput", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsSpeaker(string deviceType)
	{
		return IsAudioOutput(deviceType);
	}

	public static bool IsAudioOutput(string deviceType)
	{
		return string.Equals(deviceType, "AudioOutput", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsAudioDevice(string deviceType)
	{
		if (!IsAudioInput(deviceType))
		{
			return IsAudioOutput(deviceType);
		}
		return true;
	}

	public static bool IsMetadata(string deviceType)
	{
		return string.Equals(deviceType, "Metadata", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsRecordingDeviceType(string deviceType)
	{
		if (!IsAudioInput(deviceType) && !IsAudioOutput(deviceType) && !IsCamera(deviceType))
		{
			return IsMetadata(deviceType);
		}
		return true;
	}
}
