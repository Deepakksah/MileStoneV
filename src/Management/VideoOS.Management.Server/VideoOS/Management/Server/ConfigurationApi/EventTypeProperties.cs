using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class EventTypeProperties
{
	public const string DescriptionKey = "Description";

	public const string GeneratorTypeKey = "GeneratorType";

	public const string GeneratorSubtypeKey = "GeneratorSubtype";

	public const string GeneratorIDKey = "GeneratorID";

	public const string GeneratorNameKey = "GeneratorName";

	public const string GeneratorGroupIdKey = "GeneratorGroupId";

	public const string GeneratorGroupNameKey = "GeneratorGroupName";

	public const string OccursGloballyKey = "OccursGlobally";

	public const string BuiltInKey = "BuiltIn";

	public const string CounterEventIDKey = "CounterEventID";

	public const string SourceArrayKey = "SourceArray";

	public const string SourceFilterArrayKey = "SourceFilterArray";

	public const string StateGroupIdKey = "StateGroupId";

	public const string StateKey = "State";

	public static Property GetDescriptionProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("Description", text, ConfigurationApiResources.PropertyGeneralDescription, "PropertyGeneralDescription", isSettable: false);
	}

	public static Property GetGeneratorTypeProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("GeneratorType", text, ConfigurationApiResources.PropertyEventDefinitionGeneratorType, "PropertyEventDefinitionGeneratorType", isSettable: false);
	}

	public static Property GetGeneratorSubtypeProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("GeneratorSubtype", text, ConfigurationApiResources.PropertyEventDefinitionGeneratorSubtype, "PropertyEventDefinitionGeneratorSubtype", isSettable: false);
	}

	public static Property GetGeneratorIDProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("GeneratorID", text, ConfigurationApiResources.PropertyEventDefinitionGeneratorID, "PropertyEventDefinitionGeneratorID", isSettable: false);
	}

	public static Property GetGeneratorNameProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("GeneratorName", text, ConfigurationApiResources.PropertyEventDefinitionGeneratorName, "PropertyEventDefinitionGeneratorName", isSettable: false);
	}

	public static Property GetGeneratorGroupIdProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("GeneratorGroupId", text, ConfigurationApiResources.PropertyEventDefinitionGeneratorGroupId, "PropertyEventDefinitionGeneratorGroupId", isSettable: false);
	}

	public static Property GetGeneratorGroupNameProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("GeneratorGroupName", text, ConfigurationApiResources.PropertyEventDefinitionGeneratorGroupName, "PropertyEventDefinitionGeneratorGroupName", isSettable: false);
	}

	public static Property GetOccursGloballyProperty(bool text)
	{
		return GeneralPropertyDefinitions.GetTickProperty("OccursGlobally", ConfigurationApiResources.PropertyEventDefinitionOccursGlobally, text.ToString(), "PropertyEventDefinitionOccursGlobally", isSettable: false);
	}

	public static Property GetBuiltInProperty(bool text)
	{
		return GeneralPropertyDefinitions.GetTickProperty("BuiltIn", ConfigurationApiResources.PropertyEventDefinitionBuiltIn, text.ToString(), "PropertyEventDefinitionBuiltIn", isSettable: false);
	}

	public static Property GetCounterEventIDProperty(string text)
	{
		return GeneralPropertyDefinitions.GetStringProperty("CounterEventID", text, ConfigurationApiResources.PropertyEventDefinitionCounterEventID, "PropertyEventDefinitionCounterEventID", isSettable: false);
	}

	public static Property GetSourceArrayProperty(string[] sources)
	{
		Property property = new Property("SourceArray", sources);
		property.ValueType = "Array";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyEventDefinitionSources;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyEventDefinitionSources";
		property.ToolTipTranslationId = "PropertyEventDefinitionSourcesTooltip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}

	public static Property GetSourceFilterArrayProperty(string[] filters)
	{
		Property property = new Property("SourceFilterArray", filters);
		property.ValueType = "Array";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyEventDefinitionSourceFilters;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyEventDefinitionSourceFilters";
		property.ToolTipTranslationId = "PropertyEventDefinitionSourceFiltersTooltip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}

	public static Property GetStateGroupId(Guid? id)
	{
		string value = id?.ToString() ?? string.Empty;
		Property stringProperty = GeneralPropertyDefinitions.GetStringProperty("StateGroupId", value, ConfigurationApiResources.PropertyStateGroupId, "PropertyStateGroupId", isSettable: false);
		stringProperty.ToolTipTranslationId = "PropertyStateGroupIdTooltip";
		return stringProperty;
	}

	public static Property GetState(string state)
	{
		Property stringProperty = GeneralPropertyDefinitions.GetStringProperty("State", state, ConfigurationApiResources.PropertyStateGroupState, "PropertyStateGroupState", isSettable: false);
		stringProperty.ToolTipTranslationId = "PropertyStateGroupStateTooltip";
		return stringProperty;
	}
}
