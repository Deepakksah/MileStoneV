using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.SharedComponent.ClientProfile;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ClientProfilePropertyDefinitions
{
	public const string IsDefaultProfileKey = "IsDefaultProfile";

	public const string DisabledViewLayoutsKey = "DisabledViewLayouts";

	public static Property IsDefaultProfileProperty(bool isDefault)
	{
		return new Property("IsDefaultProfile", PropertyTypesHelper.GetValue(isDefault))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyIsDefaultProfile,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyIsDefaultProfile",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property GenericEnumProperty(ClientProfileSettingSpecification spec, string defaultValue)
	{
		return new Property(spec.ID, defaultValue ?? "")
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = spec.LocalizedName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = string.Empty,
			UIImportance = UIImportance.Default,
			SortKey = 1,
			ValueTypeInfos = ConvertControlEnum(spec.Control)
		};
	}

	public static Property GenericStringProperty(ClientProfileSettingSpecification spec, string defaultValue)
	{
		return new Property(spec.ID, defaultValue ?? "")
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = spec.LocalizedName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = string.Empty,
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property GenericProperty(ClientProfileSettingSpecification spec, string valueType, ValueTypeInfo[] valueTypeInfos, string defaultValue)
	{
		return new Property(spec.ID, defaultValue ?? "")
		{
			ValueType = valueType,
			IsSettable = true,
			DisplayName = spec.LocalizedName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = string.Empty,
			UIImportance = UIImportance.Default,
			SortKey = 1,
			ValueTypeInfos = valueTypeInfos
		};
	}

	public static Property GenericImageProperty(ClientProfileSettingSpecification spec, string defaultValue)
	{
		Property property = new Property(spec.ID, defaultValue ?? "");
		property.ValueType = "String";
		property.IsSettable = true;
		property.DisplayName = spec.LocalizedName;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = string.Empty;
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ImageAsBase64",
				Value = ""
			}
		};
		return property;
	}

	public static Property LockedProperty(ClientProfileSettingSpecification spec, bool defaultLocked)
	{
		return new Property(spec.ID + "Locked", PropertyTypesHelper.GetValue(defaultLocked))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = string.Format(CultureInfo.InvariantCulture, "{0} {1}", spec.LocalizedName, ConfigurationApiResources.ClientProfileLockedPropertyPostfix),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = string.Empty,
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static Property ViewLayoutProperty(string pathList)
	{
		Property property = new Property("DisabledViewLayouts", pathList);
		property.ValueType = "PathList";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyClientProfileDisabledViewLayouts;
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyClientProfileDisabledViewLayouts";
		property.ToolTipTranslationId = "PropertyClientProfileDisabledViewLayoutsToolTip";
		property.Value = pathList;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Layout"
			}
		};
		return property;
	}

	private static ValueTypeInfo[] ConvertControlEnum(ClientProfileControlSpecification control)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (ClientProfileControlValueSpecification element in control.Elements)
		{
			list.Add(new ValueTypeInfo
			{
				Name = element.LocalizedName,
				Value = element.ID
			});
		}
		return list.ToArray();
	}
}
