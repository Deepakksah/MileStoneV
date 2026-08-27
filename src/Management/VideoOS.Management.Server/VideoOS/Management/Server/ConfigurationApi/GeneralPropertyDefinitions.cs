using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class GeneralPropertyDefinitions
{
	public static Property GetIdPropertyGuid(string id)
	{
		return GetIdPropertyGuid(new Guid(id));
	}

	public static Property GetIdPropertyGuid(Guid id)
	{
		return GetIdPropertyGuid(id, uiVisible: false);
	}

	public static Property GetIdPropertyGuid(Guid id, bool uiVisible, bool isSettable = false)
	{
		return GetProperty("String", "Id", ConfigurationApiResources.PropertyGeneralId, PropertyTypesHelper.GetValue(id), "PropertyGeneralId", isSettable, (!uiVisible) ? 1 : 2);
	}

	public static Property GetIdPropertyInt(string id)
	{
		return GetIdPropertyInt(Convert.ToInt32(id, CultureInfo.InvariantCulture));
	}

	public static Property GetIdPropertyInt(int id)
	{
		return GetIdPropertyInt(id, uiVisible: false);
	}

	public static Property GetIdPropertyInt(int id, bool uiVisible, bool isSettable = false)
	{
		return GetProperty("Int", "Id", ConfigurationApiResources.PropertyGeneralId, PropertyTypesHelper.GetValue(id), "PropertyGeneralId", isSettable, (!uiVisible) ? 1 : 2);
	}

	public static Property GetNameProperty(string value, bool isSettable)
	{
		return GetStringProperty("Name", value, ConfigurationApiResources.PropertyGeneralName, "PropertyGeneralName", isSettable);
	}

	public static Property GetVersionProperty(string value)
	{
		return GetStringProperty("Version", value, ConfigurationApiResources.PropertyGeneralVersion, "PropertyGeneralVersion", isSettable: false);
	}

	public static Property GetNameProperty(string name)
	{
		return GetNameProperty(name, isSettable: true);
	}

	public static Property GetDescriptionProperty(string description, bool isSettable)
	{
		return GetProperty("String", "Description", ConfigurationApiResources.PropertyGeneralDescription, description, "PropertyGeneralDescription", isSettable, 0);
	}

	public static Property GetDescriptionProperty(string description)
	{
		return GetDescriptionProperty(description, isSettable: true);
	}

	public static Property GetLastModifiedProperty(DateTime lastModified)
	{
		return GetProperty("DateTime", "LastModified", ConfigurationApiResources.PropertyGeneralLastModified, PropertyTypesHelper.GetValue(lastModified), "PropertyGeneralLastModified", isSettable: false, 0);
	}

	public static Property GetCreatedDateProperty(DateTime createdDate)
	{
		return GetProperty("DateTime", "CreatedDate", ConfigurationApiResources.PropertyGeneralCreatedDate, PropertyTypesHelper.GetValue(createdDate), "PropertyGeneralCreatedDate", isSettable: false, 0);
	}

	public static Property GetTimeProperty(string name, string displayName, string value, string translationId, bool isSettable)
	{
		return GetProperty("Time", name, displayName, value, translationId, isSettable, UIImportance.Default);
	}

	public static Property GetIntProperty(string name, string displayName, string value, string translationId, bool isSettable)
	{
		return GetProperty("Int", name, displayName, value, translationId, isSettable, UIImportance.Default);
	}

	public static Property GetStringProperty(string name, string value, string displayName, string translationId, bool isSettable)
	{
		return GetProperty("String", name, displayName, value, translationId, isSettable, 2);
	}

	public static Property GetTickProperty(string name, string displayName, string value, string translationId, bool isSettable)
	{
		return GetProperty("Tick", name, displayName, value, translationId, isSettable, UIImportance.Default);
	}

	public static Property GetPathProperty(string name, string displayName, string value, IEnumerable<string> itemTypes, string translationId, bool isSettable)
	{
		Property property = GetProperty("Path", name, displayName, value, translationId, isSettable, 2);
		property.Key = name;
		property.ItemReference = true;
		property.ValueTypeInfos = itemTypes.Select((string s) => new ValueTypeInfo
		{
			Name = "PathItemType",
			Value = s
		}).ToArray();
		return property;
	}

	public static EnablePropertyInfo GetEnabledProperty(bool enabled, bool uiFollowsEnabled)
	{
		return new EnablePropertyInfo
		{
			Enabled = enabled,
			DisplayName = ConfigurationApiResources.PropertyGeneralEnabled,
			UIToFollowEnabled = uiFollowsEnabled,
			TranslationId = "PropertyGeneralEnabled"
		};
	}

	public static Property GetChannelProperty(int index, int sort)
	{
		return new Property("Channel", index.ToString(CultureInfo.InvariantCulture))
		{
			DisplayName = ConfigurationApiResources.PropertyDeviceChannel,
			ValueType = "Int",
			TranslationId = "PropertyDeviceChannel",
			SortKey = sort,
			UIImportance = UIImportance.Default,
			IsSettable = false
		};
	}

	public static Property GetGisPointProperty(string gisPoint)
	{
		return new Property("GisPoint", gisPoint)
		{
			DisplayName = ConfigurationApiResources.PropertyDeviceGisPoint,
			ValueType = "String",
			TranslationId = "PropertyDeviceGisPoint",
			UIImportance = UIImportance.Default,
			IsSettable = true,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyDeviceGisPointToolTip"
		};
	}

	public static Property GetShortNameProperty(string shortName)
	{
		return new Property("ShortName", shortName)
		{
			DisplayName = ConfigurationApiResources.PropertyDeviceShortName,
			ValueType = "String",
			TranslationId = "PropertyDeviceShortName",
			UIImportance = UIImportance.Default,
			IsSettable = true,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyDeviceShortNameTooltip"
		};
	}

	public static Property GetIconProperty(int icon)
	{
		return new Property("Icon", icon.ToString(CultureInfo.InvariantCulture))
		{
			DisplayName = ConfigurationApiResources.PropertyDeviceIcon,
			ValueType = "Int",
			TranslationId = "PropertyDeviceIcon",
			UIImportance = UIImportance.Default,
			IsSettable = true,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyDeviceIconTooltip",
			ValueTypeInfos = PropertyTypesHelper.GetIntegerRange(0, int.MaxValue)
		};
	}

	public static Property GetCoverageDirectionProperty(double coverageDirection)
	{
		return new Property("CoverageDirection", coverageDirection.ToString(CultureInfo.InvariantCulture))
		{
			DisplayName = ConfigurationApiResources.PropertyDeviceCoverageDirection,
			ValueType = "Double",
			TranslationId = "PropertyDeviceCoverageDirection",
			UIImportance = UIImportance.Default,
			IsSettable = true,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyDeviceCoverageDirectionTooltip"
		};
	}

	public static Property GetCoverageDepthProperty(double coverageDepth)
	{
		return new Property("CoverageDepth", coverageDepth.ToString(CultureInfo.InvariantCulture))
		{
			DisplayName = ConfigurationApiResources.PropertyDeviceCoverageDepth,
			ValueType = "Double",
			TranslationId = "PropertyDeviceCoverageDepth",
			UIImportance = UIImportance.Default,
			IsSettable = true,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyDeviceCoverageDepthTooltip",
			ValueTypeInfos = PropertyTypesHelper.GetDoubleRange(0.0, double.MaxValue)
		};
	}

	public static Property GetCoverageFieldOfViewProperty(double coverageFieldOfView)
	{
		return new Property("CoverageFieldOfView", coverageFieldOfView.ToString(CultureInfo.InvariantCulture))
		{
			DisplayName = ConfigurationApiResources.PropertyDeviceCoverageFieldOfView,
			ValueType = "Double",
			TranslationId = "PropertyDeviceCoverageFieldOfView",
			UIImportance = UIImportance.Default,
			IsSettable = true,
			ServerValidation = false,
			ToolTipTranslationId = "PropertyDeviceCoverageFieldOfViewTooltip"
		};
	}

	public static Property GetListProperty(string semicolonSeparatedItems, string name, string displayName, string translationId, bool isSettable)
	{
		return new Property(name, semicolonSeparatedItems)
		{
			ValueType = "Separator",
			DisplayName = displayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = translationId,
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			IsSettable = isSettable
		};
	}

	public static Property GetListProperty(string semicolonSeparatedItems, string name, string displayName, string translationId, bool isSettable, IDictionary<string, string> valueTypeInfos)
	{
		return GetListProperty(semicolonSeparatedItems, name, displayName, translationId, isSettable, valueTypeInfos, serverValidation: false);
	}

	public static Property GetListProperty(string semicolonSeparatedItems, string name, string displayName, string translationId, bool isSettable, IDictionary<string, string> valueTypeInfos, bool serverValidation)
	{
		return new Property(name, semicolonSeparatedItems)
		{
			ValueType = "Separator",
			DisplayName = displayName,
			ItemReference = false,
			ServerValidation = serverValidation,
			TranslationId = translationId,
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			IsSettable = isSettable,
			ValueTypeInfos = (from pair in valueTypeInfos
				where pair.Value != null
				select new ValueTypeInfo
				{
					Name = pair.Value,
					Value = pair.Key
				}).ToArray()
		};
	}

	public static Property GetPathListProperty(string name, string value, string displayName, string translationId, IEnumerable<string> itemTypes)
	{
		return new Property(name, value)
		{
			ValueType = "PathList",
			IsSettable = true,
			DisplayName = displayName,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = translationId,
			UIImportance = UIImportance.Summary,
			SortKey = 100,
			ValueTypeInfos = itemTypes.Select((string s) => new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = s
			}).ToArray()
		};
	}

	public static string ConvertFromItemTypeToChildItemType(string parentItemType)
	{
		string result = parentItemType;
		switch (parentItemType)
		{
		case "CameraFolder":
			result = "Camera";
			break;
		case "MicrophoneFolder":
			result = "Microphone";
			break;
		case "SpeakerFolder":
			result = "Speaker";
			break;
		case "InputEventFolder":
			result = "InputEvent";
			break;
		case "OutputFolder":
			result = "Output";
			break;
		case "MetadataFolder":
			result = "Metadata";
			break;
		case "RuleFolder":
			result = "Rule";
			break;
		}
		return result;
	}

	public static Property GetPathSelectionProperty(string itemType, string propertyName, string displayName)
	{
		string value = ConvertFromItemTypeToChildItemType(itemType);
		Property property = new Property(propertyName, "");
		property.ValueType = "Path";
		property.IsSettable = true;
		property.DisplayName = displayName;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = value
			}
		};
		return property;
	}

	public static Property GetEnumProperty(string propertyName, string value, string displayName, string translationId, string tooltipTranslationId, IDictionary<string, string> members)
	{
		return GetEnumProperty(propertyName, value, displayName, translationId, tooltipTranslationId, members, serverValidation: false);
	}

	public static Property GetEnumProperty(string propertyName, string value, string displayName, string translationId, string tooltipTranslationId, IDictionary<string, string> members, bool serverValidation)
	{
		return new Property(propertyName, value)
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = displayName,
			ItemReference = false,
			ServerValidation = serverValidation,
			TranslationId = translationId,
			ToolTipTranslationId = tooltipTranslationId,
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = GetEnumValues(members)
		};
	}

	public static ValueTypeInfo[] GetEnumValuesFromType(Type enumType)
	{
		enumType.NullCheck("", "GetEnumValuesFromType", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\PropertyHandling\\GeneralPropertyDefinitions.cs", 429);
		if (!enumType.IsEnum)
		{
			throw new ArgumentException("Not an enum type: " + enumType.FullName);
		}
		return GetEnumValues((from object s in Enum.GetValues(enumType)
			select s.ToString()).ToDictionary((string s) => s, (string s) => s));
	}

	public static ValueTypeInfo[] GetEnumValues(IDictionary<string, string> members)
	{
		members.NullCheck("", "GetEnumValues", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\PropertyHandling\\GeneralPropertyDefinitions.cs", 440);
		return (from pair in members
			where pair.Value != null
			select new ValueTypeInfo
			{
				TranslationId = "",
				Value = pair.Key,
				Name = pair.Value
			}).ToArray();
	}

	private static Property GetProperty(string valueType, string name, string displayName, string value, string translationId, bool isSettable, int uiImportance)
	{
		return new Property(name, value)
		{
			ValueType = valueType,
			IsSettable = isSettable,
			DisplayName = displayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = translationId,
			UIImportance = uiImportance,
			SortKey = 100
		};
	}
}
