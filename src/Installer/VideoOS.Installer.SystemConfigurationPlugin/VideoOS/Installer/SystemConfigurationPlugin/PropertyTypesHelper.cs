using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Installer.SystemConfigurationPlugin;

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

	public static ValueTypeInfo[] GetDoubleRange(double min, double max)
	{
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo
		{
			Name = "MinValue",
			TranslationId = "",
			Value = min.ToString("R", CultureInfo.InvariantCulture)
		};
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo
		{
			Name = "MaxValue",
			TranslationId = "",
			Value = max.ToString("R", CultureInfo.InvariantCulture)
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
		return value.ToString("o", CultureInfo.InvariantCulture);
	}
}
