using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class GenericEventProperties
{
	public static Property ExpressionProperty(string val, bool isSettable)
	{
		return new Property(GenericEventConfigurationApi.ExpressionKey, val)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGenericEventExpression,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventExpression",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property ExpressionTypeProperty(string val, bool isSettable)
	{
		Property property = new Property(GenericEventConfigurationApi.ExpressionTypeKey, val)
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGenericEventExpressiontype,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventExpressiontype",
			UIImportance = UIImportance.Summary,
			SortKey = 101
		};
		property.ValueTypeInfos = new ValueTypeInfo[3]
		{
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.PropertyGenericEventExpressionTypeSearch,
				TranslationId = "PropertyGenericEventExpressionTypeSearch",
				Value = "0"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.PropertyGenericEventExpressionTypeMatch,
				TranslationId = "PropertyGenericEventExpressionTypeMatch",
				Value = "1"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.PropertyGenericEventExpressionTypeRegularExpression,
				TranslationId = "PropertyGenericEventExpressionTypeRegularExpression",
				Value = "2"
			}
		};
		return property;
	}

	public static Property DataSourceProperty(string val, bool isSettable, List<GenericEventDataSource> datasources)
	{
		Property property = new Property(GenericEventConfigurationApi.DataSourceKey, val)
		{
			ValueType = "Enum",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSource,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSource",
			UIImportance = UIImportance.Summary,
			SortKey = 101
		};
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (GenericEventDataSource datasource in datasources)
		{
			list.Add(new ValueTypeInfo
			{
				Name = datasource.Name,
				Value = ConfigurationItemPath.CreateItemPath("GenericEventDataSource", datasource.Id.ToString()).Text
			});
		}
		property.ValueTypeInfos = list.ToArray();
		return property;
	}

	public static Property PriorityProperty(string val, bool isSettable)
	{
		Property property = new Property(GenericEventConfigurationApi.PriorityKey, val)
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyGenericEventPriority,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventPriority",
			UIImportance = UIImportance.Summary,
			SortKey = 103
		};
		property.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "MinValue",
				Value = "0"
			},
			new ValueTypeInfo
			{
				Name = "MaxValue",
				Value = "999999"
			}
		};
		return property;
	}

	public static Property ShadowIdProperty(string val)
	{
		return new Property(GenericEventConfigurationApi.ShadowIdKey, val)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyGenericEventShadowId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventShadowId",
			ToolTipTranslationId = "PropertyGenericEventShadowIdTooltip",
			UIImportance = UIImportance.Hidden,
			SortKey = 104
		};
	}
}
