using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Reporting.Web.Plugin.Base;

public static class EnumUtility
{
	public static bool IsFlaggedEnum(Type enumType)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		if (!enumType.IsEnum)
		{
			return false;
		}
		object[] customAttributes = enumType.GetCustomAttributes(inherit: true);
		for (int i = 0; i < customAttributes.Length; i++)
		{
			if (customAttributes[i].ToString() == "System.FlagsAttribute")
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsValidFlaggedEnum(Enum valueToValidate)
	{
		if (valueToValidate == null)
		{
			throw new ArgumentNullException("valueToValidate");
		}
		int num = 0;
		if (Enum.GetUnderlyingType(valueToValidate.GetType()) != typeof(int))
		{
			return false;
		}
		foreach (int value in Enum.GetValues(valueToValidate.GetType()))
		{
			num |= value;
		}
		int num3 = ~num;
		if ((Convert.ToInt32(valueToValidate, CultureInfo.InvariantCulture) & num3) > 0)
		{
			return false;
		}
		return true;
	}

	public static bool IsValidEnum(Enum valueToValidate)
	{
		if (valueToValidate == null)
		{
			throw new ArgumentNullException("valueToValidate");
		}
		if (IsFlaggedEnum(valueToValidate.GetType()))
		{
			return IsValidFlaggedEnum(valueToValidate);
		}
		if (Enum.GetName(valueToValidate.GetType(), valueToValidate) == null)
		{
			return false;
		}
		return true;
	}

	private static List<string> TrimAll(List<string> stringList)
	{
		List<string> list = new List<string>();
		foreach (string @string in stringList)
		{
			list.Add(@string.Trim());
		}
		return list;
	}

	public static bool TryParseEnum(Type enumType, string enumCsvList, out Enum enumerator)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		if (!enumType.IsEnum)
		{
			throw new ArgumentOutOfRangeException("enumType", "Input parameter 'enumType' does not represent an enum.");
		}
		if (enumCsvList == null || enumCsvList.Length == 0)
		{
			enumerator = null;
			return false;
		}
		List<string> source = new List<string>(Enum.GetNames(enumType));
		List<string> stringList = new List<string>(enumCsvList.Split(','));
		stringList = TrimAll(stringList);
		if (stringList.Count > 1 && !IsFlaggedEnum(enumType))
		{
			throw new ArgumentOutOfRangeException("enumCsvList", "Multiple values in 'enumCSVList' requires that enumType has the FlagsAttribute.");
		}
		foreach (string item in stringList)
		{
			if (!source.Contains(item, StringComparer.OrdinalIgnoreCase))
			{
				enumerator = null;
				return false;
			}
		}
		enumerator = (Enum)Enum.Parse(enumType, string.Join(", ", stringList.ToArray()), ignoreCase: true);
		return true;
	}

	public static bool IsDefined(Type enumType, string value)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		if (!enumType.IsEnum)
		{
			throw new ArgumentOutOfRangeException("enumType", "Input parameter 'enumType' does not represent an enum.");
		}
		if (new List<string>(Enum.GetNames(enumType)).Contains(value, StringComparer.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}
}
