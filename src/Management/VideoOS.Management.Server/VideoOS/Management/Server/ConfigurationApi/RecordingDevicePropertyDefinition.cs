using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingDevicePropertyDefinition
{
	public static Property RecordingEnabledProperty(bool recordingEnabled)
	{
		return new Property("RecordingEnabled", recordingEnabled.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceRecordingEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceRecordingEnabled",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property PrebufferEnabledProperty(bool prebufferEnabled)
	{
		return new Property("PrebufferEnabled", prebufferEnabled.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDevicePrebufferEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDevicePrebufferEnabled",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property PrebufferInMemoryProperty(bool prebufferInMemory)
	{
		return new Property("PrebufferInMemory", prebufferInMemory.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDevicePrebufferInMemory,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDevicePrebufferInMemory",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property PrebufferSecondsProperty(int prebufferSeconds)
	{
		return new Property("PrebufferSeconds", PropertyTypesHelper.GetValue(prebufferSeconds))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDevicePrebufferSeconds,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDevicePrebufferSeconds",
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(0, int.MaxValue)
		};
	}

	public static Property EdgeStorageEnabledProperty(bool edgeStorageEnabled)
	{
		return new Property("EdgeStorageEnabled", edgeStorageEnabled.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceEdgeStorageEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceEdgeStorageEnabled",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property EdgeStoragePlaybackEnabledProperty(bool edgeStoragePlaybackEnabled)
	{
		return new Property("EdgeStoragePlaybackEnabled", PropertyTypesHelper.GetValue(edgeStoragePlaybackEnabled))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceEdgeStoragePlaybackEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceEdgeStoragePlaybackEnabled",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property ManualRecordingTimeoutEnabledProperty(bool manualRecordingTimeoutEnabled)
	{
		return new Property("ManualRecordingTimeoutEnabled", PropertyTypesHelper.GetValue(manualRecordingTimeoutEnabled))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceManualRecordingTimeoutEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceManualRecordingTimeoutEnabled",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property ManualRecordingTimeoutMinutesProperty(int manualRecordingTimeoutMinutes)
	{
		return new Property("ManualRecordingTimeoutMinutes", PropertyTypesHelper.GetValue(manualRecordingTimeoutMinutes))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceManualRecordingTimeoutMinutes,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceManualRecordingTimeoutMinutes",
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(0, int.MaxValue)
		};
	}

	public static Property MoveDeviceData(bool moveData)
	{
		return new Property("moveData", PropertyTypesHelper.GetValue(moveData))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMoveDeviceData,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMoveDeviceData",
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property RecordingStoragePathProperty(ConfigurationItemPath path)
	{
		Property property = new Property("RecordingStorage", path.Text);
		property.ValueType = "Path";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyRecordingStoragePath;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyRecordingStoragePath";
		property.ToolTipTranslationId = "PropertyRecordingStoragePathToolTip";
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Storage"
			}
		};
		return property;
	}

	public static Property FailoverSetting(bool failoverLiveOnly, bool disableOnFailover)
	{
		string text = "FullSupport";
		if (failoverLiveOnly)
		{
			text = "LiveOnly";
		}
		if (disableOnFailover)
		{
			text = "Disabled";
		}
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		list.Add(new ValueTypeInfo
		{
			Name = ConfigurationApiResources.PropertyFailoverSettingFull,
			TranslationId = "PropertyFailoverSettingFull",
			Value = "FullSupport"
		});
		list.Add(new ValueTypeInfo
		{
			Name = ConfigurationApiResources.PropertyFailoverSettingLiveOnly,
			TranslationId = "PropertyFailoverSettingLiveOnly",
			Value = "LiveOnly"
		});
		list.Add(new ValueTypeInfo
		{
			Name = ConfigurationApiResources.PropertyFailoverSettingDisabled,
			TranslationId = "PropertyFailoverSettingDisabled",
			Value = "Disabled"
		});
		return new Property("FailoverSetting", text.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceFailoverSetting,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceFailoverSetting",
			ToolTipTranslationId = "PropertyDeviceFailoverSettingToolTip",
			ValueTypeInfos = list.ToArray()
		};
	}
}
