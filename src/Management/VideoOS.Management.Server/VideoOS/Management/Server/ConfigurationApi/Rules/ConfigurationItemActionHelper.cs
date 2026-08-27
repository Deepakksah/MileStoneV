using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules;

public static class ConfigurationItemActionHelper
{
	public static Dictionary<Type, string> SimpleTypesMap => new Dictionary<Type, string>
	{
		{
			typeof(int),
			"Int"
		},
		{
			typeof(double),
			"Double"
		},
		{
			typeof(DateTime),
			"DateTime"
		},
		{
			typeof(string),
			"String"
		}
	};

	public static HashSet<string> PathTypes => new HashSet<string> { "Path", "PathList" };

	public static HashSet<string> RangedValueTypes => new HashSet<string> { "Int", "Double", "DateTime" };

	public static ValueTypeInfo[] GetValueTypeInfos(RuleParameterDefinition parameter)
	{
		parameter.NullCheck("", "GetValueTypeInfos", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemActionHelper.cs", 44);
		if (parameter.ValueType.IsEnum)
		{
			return GeneralPropertyDefinitions.GetEnumValuesFromType(parameter.ValueType);
		}
		string propertyType = GetPropertyType(parameter);
		if (RangedValueTypes.Contains(propertyType) && parameter.MaxValue != null && parameter.MinValue != null)
		{
			return PropertyTypesHelper.GetMinMaxRange(parameter.MinValue.ToString(), parameter.MaxValue.ToString());
		}
		if (PathTypes.Contains(propertyType) && parameter.AllowedItemTypes != null && parameter.AllowedItemTypes.Any())
		{
			return parameter.AllowedItemTypes.Select((string s) => new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = s
			}).ToArray();
		}
		if (parameter.AllowedKeyValuePairs != null && parameter.AllowedKeyValuePairs.Any())
		{
			return parameter.AllowedKeyValuePairs.Select((KeyValuePair<string, string> s) => new ValueTypeInfo
			{
				Name = s.Value,
				Value = s.Key
			}).ToArray();
		}
		return null;
	}

	public static Property GetProperty(RuleParameterDefinition parameter, string propertyName, string propertyValue)
	{
		parameter.NullCheck("", "GetProperty", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemActionHelper.cs", 81);
		return new Property
		{
			Key = propertyName,
			Value = propertyValue,
			ValueType = GetPropertyType(parameter),
			ValueTypeInfos = GetValueTypeInfos(parameter),
			ItemReference = IsItemReference(parameter),
			IsSettable = true,
			DisplayName = propertyName,
			ServerValidation = false,
			TranslationId = string.Empty,
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static bool IsItemReference(RuleParameterDefinition definition)
	{
		return PathTypes.Contains(GetPropertyType(definition));
	}

	public static string GetPropertyType(RuleParameterDefinition definition)
	{
		if (definition != null && !string.IsNullOrEmpty(definition.SpecialPropertyType))
		{
			return definition.SpecialPropertyType;
		}
		if (definition != null && SimpleTypesMap.TryGetValue(definition.ValueType, out var value))
		{
			return value;
		}
		return "String";
	}

	public static bool IsCorrectPosition(bool isStart, ActionPosition position)
	{
		if (position != ActionPosition.None && (!isStart || position != ActionPosition.Start))
		{
			if (!isStart)
			{
				return position == ActionPosition.Stop;
			}
			return false;
		}
		return true;
	}
}
