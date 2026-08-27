using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.Utilities;

public static class EnumsHelper
{
	public interface IAttributeValue<TResult>
	{
		TResult GetValue();
	}

	public static string GetDescription(Enum value)
	{
		DescriptionAttribute[] array = (DescriptionAttribute[])value.GetType().GetRuntimeField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
		if (array.Length == 0)
		{
			return value.ToString();
		}
		return array[0].Description;
	}

	public static T EnumConverter<T>(string szInput)
	{
		foreach (FieldInfo item in (from f in typeof(T).GetRuntimeFields()
			where f.IsStatic && f.IsPublic
			select f).ToList())
		{
			object[] customAttributes = item.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
			object[] array = customAttributes;
			for (int num = 0; num < array.Length; num++)
			{
				DescriptionAttribute descriptionAttribute = (DescriptionAttribute)array[num];
				if (string.Compare(szInput, descriptionAttribute.Description, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return (T)item.GetValue(null);
				}
			}
			if (!customAttributes.Any() && string.Compare(szInput, item.GetValue(null).ToString(), StringComparison.OrdinalIgnoreCase) == 0)
			{
				return (T)item.GetValue(null);
			}
		}
		return default(T);
	}

	public static TResult GetAttributeValue<TAttribute, TResult>(Enum value) where TAttribute : Attribute, IAttributeValue<TResult>
	{
		TAttribute[] array = (TAttribute[])value.GetType().GetRuntimeField(value.ToString()).GetCustomAttributes(typeof(TAttribute), inherit: false);
		if (array.Length == 0)
		{
			return default(TResult);
		}
		return array[0].GetValue();
	}

	public static IEnumerable<Enum> GetFlags(Enum input)
	{
		foreach (Enum value in Enum.GetValues(input.GetType()))
		{
			if (input.HasFlag(value))
			{
				yield return value;
			}
		}
	}

	public static bool IsSetIn(this Enum mask, int flags)
	{
		int num = Convert.ToInt32(mask);
		return (flags & num) == num;
	}

	public static IEnumerable<T> GetEnumValues<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>();
	}
}
