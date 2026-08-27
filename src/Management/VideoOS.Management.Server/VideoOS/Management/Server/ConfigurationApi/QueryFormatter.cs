using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class QueryFormatter
{
	private static Collection<string> textOriented = new Collection<string> { "AES256PKCS7String", "DateTime", "Date", "Enum", "Label", "String", "Time" };

	public static string ConstructSqlSelect(List<Property> properties, string itemType, ItemFilter itemFilter, Dictionary<string, string> keysToTranslate)
	{
		string text = string.Empty;
		bool flag = true;
		if (itemType != itemFilter.ItemType)
		{
			return text;
		}
		if (itemFilter.PropertyFilters == null)
		{
			return text;
		}
		if (itemFilter.EnableFilter != EnableFilter.All)
		{
			text = string.Format(CultureInfo.InvariantCulture, "d.[Enabled]  =  {0}", (itemFilter.EnableFilter == EnableFilter.Enabled) ? "1" : "0");
			flag = false;
		}
		PropertyFilter[] propertyFilters = itemFilter.PropertyFilters;
		foreach (PropertyFilter propertyFilter in propertyFilters)
		{
			string text2 = "";
			if (propertyFilter.Key == "IDRecorder")
			{
				if (!Guid.TryParse(propertyFilter.Value, out var _))
				{
					throw new ArgumentException("Invalid formatted id");
				}
				text2 = " hw.[IDRecorder] = '" + propertyFilter.Value + "' ";
			}
			else if (propertyFilter.Key == "IDHardware")
			{
				if (!Guid.TryParse(propertyFilter.Value, out var _))
				{
					throw new ArgumentException("Invalid formatted id");
				}
				text2 = " hw.[IDHardware] = '" + propertyFilter.Value + "' ";
			}
			else
			{
				Property property = properties.FirstOrDefault((Property p) => p.Key == propertyFilter.Key);
				if (property == null)
				{
					continue;
				}
				string sqlKey = GetSqlKey(keysToTranslate, propertyFilter.Key);
				string sqlOperator = GetSqlOperator(property, propertyFilter.Operator);
				string sqlValue = GetSqlValue(property, propertyFilter);
				text2 = ((!sqlKey.StartsWith("d.", StringComparison.Ordinal) && !sqlKey.StartsWith("rs.", StringComparison.Ordinal)) ? string.Format(CultureInfo.InvariantCulture, "d.[{0}] {1} {2}", sqlKey, sqlOperator, sqlValue) : string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", sqlKey, sqlOperator, sqlValue));
			}
			if (flag)
			{
				flag = false;
			}
			else
			{
				text += " AND ";
			}
			text += text2;
		}
		return text;
	}

	private static string GetSqlKey(Dictionary<string, string> keysToTranslate, string key)
	{
		if (keysToTranslate.ContainsKey(key))
		{
			return keysToTranslate[key];
		}
		ValidateKey(key);
		return key;
	}

	private static string GetSqlValue(Property property, PropertyFilter propertyFilter)
	{
		if (IsTextOriented(property.ValueType))
		{
			string text = propertyFilter.Value.Replace("'", "''");
			text = text.Replace("{", "{{");
			text = text.Replace("}", "}}");
			if (property.ValueType == "DateTime")
			{
				text = FixMillisecondsToMatchSql(text);
			}
			if (propertyFilter.Operator == Operator.Contains || propertyFilter.Operator == Operator.BeginsWith)
			{
				text = text.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
				return string.Format(CultureInfo.InvariantCulture, "N'%{0}%'", text);
			}
			return string.Format(CultureInfo.InvariantCulture, "N'{0}'", text);
		}
		if (property.ValueType == "Tick")
		{
			if (bool.TryParse(propertyFilter.Value, out var result))
			{
				if (!result)
				{
					return "0";
				}
				return "1";
			}
			throw new ArgumentException("Invalid value for boolean operation");
		}
		if (property.ValueType == "Int" || property.ValueType == "Double" || property.ValueType == "Slider" || property.ValueType == "Progress")
		{
			ValidateIntDouble(propertyFilter.Value);
		}
		else
		{
			ValidateValue(propertyFilter.Value);
		}
		return propertyFilter.Value;
	}

	private static string FixMillisecondsToMatchSql(string datetimeValue)
	{
		int num = datetimeValue.LastIndexOf('.');
		if (num == -1)
		{
			return datetimeValue;
		}
		int num2 = datetimeValue.LastIndexOf('+');
		if (num2 == -1)
		{
			num2 = datetimeValue.Length;
		}
		num += 4;
		if (num2 > num)
		{
			datetimeValue = datetimeValue.Remove(num, num2 - num);
		}
		return datetimeValue;
	}

	private static string GetSqlOperator(Property property, Operator opr)
	{
		switch (opr)
		{
		case Operator.Contains:
		case Operator.BeginsWith:
			if (!IsTextOriented(property.ValueType))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Operator {0} cannot be used on field of ValueType:{1}", opr, property.ValueType));
			}
			return " LIKE ";
		case Operator.Equals:
			return " = ";
		case Operator.GreaterThan:
			return " > ";
		case Operator.LessThan:
			return " < ";
		case Operator.NotEquals:
			return " != ";
		default:
			throw new ArgumentException("Unknown operator");
		}
	}

	private static bool IsTextOriented(string valueType)
	{
		return textOriented.Contains(valueType);
	}

	private static void ValidateValue(string value)
	{
		if (!new Regex("^[ :+\\-.a-zA-Z0-9]*$").IsMatch(value))
		{
			throw new ArgumentException("Invalid text value");
		}
	}

	private static void ValidateIntDouble(string value)
	{
		if (!new Regex("^[+\\-.0-9]*$").IsMatch(value))
		{
			throw new ArgumentException("Invalid number value");
		}
	}

	private static void ValidateKey(string value)
	{
		if (!new Regex("^[.a-zA-Z0-9]*$").IsMatch(value))
		{
			throw new ArgumentException("Invalid Key");
		}
	}
}
