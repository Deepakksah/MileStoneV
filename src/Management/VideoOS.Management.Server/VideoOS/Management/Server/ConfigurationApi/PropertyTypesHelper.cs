using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class PropertyTypesHelper
{
	public static ValueTypeInfo[] GetPropertyValueInfoItems(Type enumType)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		if (!enumType.IsEnum)
		{
			throw new ArgumentException("The parametere must be an enumeration type.", "enumType");
		}
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (object value in Enum.GetValues(enumType))
		{
			ValueTypeInfo valueTypeInfo = new ValueTypeInfo();
			valueTypeInfo.Name = value.ToString();
			valueTypeInfo.Value = value.ToString();
			valueTypeInfo.TranslationId = string.Format(CultureInfo.InvariantCulture, "ValueTypeEnum{0}{1}", enumType.Name, value.ToString());
			list.Add(valueTypeInfo);
		}
		return list.ToArray();
	}

	public static ValueTypeInfo[] GetIntegerRange(int min, int max)
	{
		return GetMinMaxRange(min.ToString(CultureInfo.InvariantCulture), max.ToString(CultureInfo.InvariantCulture));
	}

	public static ValueTypeInfo[] GetDoubleRange(double min, double max)
	{
		return GetMinMaxRange(min.ToString("R", CultureInfo.InvariantCulture), max.ToString("R", CultureInfo.InvariantCulture));
	}

	public static ValueTypeInfo[] GetMinMaxRange(string min, string max)
	{
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo
		{
			Name = "MinValue",
			TranslationId = "",
			Value = min
		};
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo
		{
			Name = "MaxValue",
			TranslationId = "",
			Value = max
		};
		return new ValueTypeInfo[2] { valueTypeInfo, valueTypeInfo2 };
	}

	public static string GetValue<T>(T value)
	{
		if (value is DateTime)
		{
			return GetValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
		}
		if (value is Guid)
		{
			return GetValue((Guid)Convert.ChangeType(value, typeof(Guid), CultureInfo.InvariantCulture));
		}
		if (value is int)
		{
			return GetValue(Convert.ToInt32(value, CultureInfo.InvariantCulture));
		}
		if (value is long)
		{
			return GetValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
		}
		if (value is bool)
		{
			return GetValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
		}
		if (value is double)
		{
			return GetValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
		}
		return value.ToString();
	}

	public static string GetValue(Guid value)
	{
		return value.ToString().ToUpperInvariant();
	}

	public static string GetValue(long value)
	{
		return value.ToString(CultureInfo.InvariantCulture);
	}

	public static string GetValue(int value)
	{
		return value.ToString(CultureInfo.InvariantCulture);
	}

	public static string GetValue(bool value)
	{
		return value.ToString();
	}

	public static string GetValue(double value)
	{
		return value.ToString("R", CultureInfo.InvariantCulture);
	}

	public static string GetValue(DateTime value)
	{
		return value.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
	}

	public static string GetValue(Version value)
	{
		return value.ToString();
	}

	public static Property GetMembersPathAsEnumsProperty(string propertyName, string displayName, string translationId, string toolTiptranslationId, ConfigurationItem[] members)
	{
		return GetMembersAsEnumsProperty(propertyName, displayName, translationId, toolTiptranslationId, members, (ConfigurationItem m) => m.Path);
	}

	public static Property GetMembersPropertyAsEnumsProperty(string propertyName, string displayName, string translationId, string toolTiptranslationId, ConfigurationItem[] members, string valuePropertyName)
	{
		return GetMembersAsEnumsProperty(propertyName, displayName, translationId, toolTiptranslationId, members, (ConfigurationItem m) => m.Properties.GetProperty(valuePropertyName)?.Value);
	}

	public static Property GetMembersAsEnumsProperty(string propertyName, string displayName, string translationId, string toolTiptranslationId, ConfigurationItem[] members, Func<ConfigurationItem, string> memberValueFunc)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(members.Length);
		foreach (ConfigurationItem configurationItem in members)
		{
			string text = memberValueFunc(configurationItem);
			if (text != null)
			{
				dictionary[text] = configurationItem.DisplayName;
			}
		}
		return GeneralPropertyDefinitions.GetEnumProperty(propertyName, "", displayName, translationId, toolTiptranslationId, dictionary);
	}

	public static ValueTypeInfo[] GetIntegerRange(int min, long max)
	{
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo
		{
			Name = "MinValue",
			TranslationId = "",
			Value = min.ToString(CultureInfo.InvariantCulture)
		};
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo
		{
			Name = "MaxValue",
			TranslationId = "",
			Value = max.ToString(CultureInfo.InvariantCulture)
		};
		return new ValueTypeInfo[2] { valueTypeInfo, valueTypeInfo2 };
	}

	public static ValueTypeInfo[] GetBoolValueTypeInfos()
	{
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo
		{
			Name = "BoolValue",
			Value = "true"
		};
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo
		{
			Name = "BoolValue",
			Value = "false"
		};
		return new ValueTypeInfo[2] { valueTypeInfo, valueTypeInfo2 };
	}
}
