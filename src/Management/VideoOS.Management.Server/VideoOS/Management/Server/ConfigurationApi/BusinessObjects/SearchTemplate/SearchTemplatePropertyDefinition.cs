using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.SearchTemplate;

public static class SearchTemplatePropertyDefinition
{
	public static Property StringProperty(string value, bool isSettable)
	{
		return new Property("Id", value)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGeneralId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGeneralId"
		};
	}

	public static Property SearchQueryProperty(string searchQuery)
	{
		return new Property("SearchQuery", searchQuery)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySearchQuery,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySearchQuery"
		};
	}

	public static Property AvailabilityProperty(SearchTemplateAvailability availability, bool isSettable)
	{
		return new Property("Availability", availability.ToString())
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertySearchAvailability,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySearchAvailability",
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(SearchTemplateAvailability))
		};
	}

	public static Property CountProperty(int count)
	{
		return new Property("Count", count.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySearchCount,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySearchCount"
		};
	}

	public static Property StartIndexProperty(int startIndex)
	{
		return new Property("StartIndex", startIndex.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySearchStartIndex,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySearchStartIndex"
		};
	}

	public static Property KeywordFilterProperty(string nameFilter)
	{
		return new Property("KeywordFilter", nameFilter)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySearchKeywordFilter,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySearchKeywordFilter"
		};
	}

	public static Property PrivateOnlyProperty(bool privateOnly)
	{
		return new Property("PrivateOnly", privateOnly.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySearchPrivateOnly,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySearchPrivateOnly"
		};
	}

	public static Property IncludesScopeItemsProperty(bool includesScopeItems, bool isSettable)
	{
		return new Property("IncludesScopeItems", includesScopeItems.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertySearchIncludesScopeItems,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertySearchIncludesScopeItems"
		};
	}
}
