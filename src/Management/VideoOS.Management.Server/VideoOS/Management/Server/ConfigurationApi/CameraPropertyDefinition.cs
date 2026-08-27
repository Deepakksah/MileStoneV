using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class CameraPropertyDefinition
{
	public static Property RecordingFramerateProperty(double recordingFramerate)
	{
		return new Property("RecordingFramerate", PropertyTypesHelper.GetValue(recordingFramerate))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceRecordingFramerate,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceRecordingFramerate"
		};
	}

	public static Property RecordKeyFramesOnlyProperty(bool recordKeyFramesOnly)
	{
		return new Property("RecordKeyframesOnly", PropertyTypesHelper.GetValue(recordKeyFramesOnly))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceRecordKeyFramesOnly,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceRecordKeyFramesOnly",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property RecordOnRelatedDevicesProperty(bool recordOnRelatedDevices)
	{
		return new Property("RecordOnRelatedDevices", PropertyTypesHelper.GetValue(recordOnRelatedDevices))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordOnRelatedDevices,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordOnRelatedDevices",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property PtzEnabledProperty(bool ptzEnabled)
	{
		return new Property("PtzEnabled", PropertyTypesHelper.GetValue(ptzEnabled))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyPtzEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyPtzEnabled",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}
}
