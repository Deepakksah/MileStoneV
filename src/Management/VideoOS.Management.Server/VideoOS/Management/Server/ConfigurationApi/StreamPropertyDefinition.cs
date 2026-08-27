using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class StreamPropertyDefinition
{
	public static Property StreamReferenceIdProperty(Guid streamRefId, ValueTypeInfo[] referenceStreamIdTypeInfo)
	{
		return new Property("StreamReferenceId", PropertyTypesHelper.GetValue(streamRefId))
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStreamReferenceId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStreamReferenceId",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = referenceStreamIdTypeInfo
		};
	}

	public static Property LiveModeProperty(StreamLiveMode value)
	{
		return new Property("LiveMode", value.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStreamLiveMode,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStreamLiveMode",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(StreamLiveMode))
		};
	}

	public static Property LiveDefaultProperty(bool value)
	{
		return new Property("LiveDefault", value.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStreamLiveDefault,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStreamLiveDefault",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property RecordToProperty(Guid? trackDefinitionId, ValueTypeInfo[] valueTypeInfos)
	{
		return new Property("RecordTo", trackDefinitionId?.ToString() ?? string.Empty)
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStreamRecordTo,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStreamRecordTo",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = valueTypeInfos
		};
	}

	public static Property UseEdgeProperty(bool value)
	{
		return new Property("UseEdge", value.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStreamUseEdge,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStreamUseEdge",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property DefaultPlaybackProperty(bool value)
	{
		return new Property("DefaultPlayback", value.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyStreamDefaultPlayback,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStreamDefaultPlayback",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetBoolValueTypeInfos()
		};
	}

	public static Property EdgeTrackIdProperty(Guid edgeTrackId)
	{
		string value = ((edgeTrackId == Guid.Empty) ? "" : edgeTrackId.ToString());
		return new Property("EdgeTrackId", value)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStreamEdgeTrackId,
			TranslationId = "PropertyStreamEdgeTrackId",
			SortKey = 100
		};
	}
}
