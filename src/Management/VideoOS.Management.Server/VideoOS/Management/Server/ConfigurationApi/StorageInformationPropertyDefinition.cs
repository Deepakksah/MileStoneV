using System;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class StorageInformationPropertyDefinition
{
	public static Property UsedSpaceProperty(int usedSpace)
	{
		return new Property("UsedSpace", usedSpace.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStorageInformationUsedSpace,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageInformationUsedSpace",
			ToolTipTranslationId = "PropertyStorageInformationUsedSpaceToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property LockedUsedSpaceProperty(int lockedUseSpace)
	{
		return new Property("LockedUsedSpace", lockedUseSpace.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStorageInformationLockedUsedSpace,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageInformationLockedUsedSpace",
			ToolTipTranslationId = "PropertyStorageInformationLockedUsedSpaceToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property LastUpdatedProperty(DateTime lastUpdated)
	{
		return new Property("LastUpdated", PropertyTypesHelper.GetValue(lastUpdated))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStorageInformationLastUpdated,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageInformationLastUpdated",
			ToolTipTranslationId = "PropertyStorageInformationLastUpdatedToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property AnyTablesWithDataProperty(TableContainsAnyData tableContainsAnyData)
	{
		return new Property("AnyTablesWithData", tableContainsAnyData.ToString())
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStorageInformationAnyTablesWithData,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageInformationAnyTablesWithData",
			ToolTipTranslationId = "PropertyStorageInformationAnyTablesWithDataToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(TableContainsAnyData))
		};
	}

	public static Property IsAvailableProperty(bool available)
	{
		return new Property("IsAvailable", PropertyTypesHelper.GetValue(available))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStorageInformationIsAvailable,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageInformationIsAvailable",
			ToolTipTranslationId = "PropertyStorageInformationIsAvailableToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property IsMountedProperty(bool mounted)
	{
		return new Property("IsMounted", PropertyTypesHelper.GetValue(mounted))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyStorageInformationIsMounted,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyStorageInformationIsMounted",
			ToolTipTranslationId = "PropertyStorageInformationIsMountedToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}
