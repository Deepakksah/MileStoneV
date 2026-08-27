using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.IM.Common.GlobalDefinitions.Utilities;

public static class Extensions
{
	public static string GetResourceKey(this string value)
	{
		if (!value.CanBeLocalized())
		{
			throw new ArgumentException("The resource key cannot be extracted from the string " + value + ".", "value");
		}
		if (value.Length <= Constants.Translations.TranslationPrefix.Length)
		{
			throw new ArgumentException("A resource key should be declared after " + Constants.Translations.TranslationPrefix + ". Value used " + value, "value");
		}
		return value.Substring(Constants.Translations.TranslationPrefix.Length);
	}

	public static bool CanBeLocalized(this string value)
	{
		return value.StartsWith(Constants.Translations.TranslationPrefix);
	}

	public static DateTime ConvertUtcToLocalTime(this DateTime inUtcTime)
	{
		if (inUtcTime.Kind == DateTimeKind.Unspecified)
		{
			inUtcTime = DateTime.SpecifyKind(inUtcTime, DateTimeKind.Utc);
		}
		return inUtcTime.ToLocalTime();
	}

	public static DateTime ConvertLocalToUtcTime(this DateTime inLocalTime)
	{
		if (inLocalTime.Kind == DateTimeKind.Unspecified)
		{
			inLocalTime = DateTime.SpecifyKind(inLocalTime, DateTimeKind.Local);
		}
		return inLocalTime.ToUniversalTime();
	}

	public static DateTime? ConvertUtcToLocalTime(this DateTime? inUtcTime)
	{
		if (!inUtcTime.HasValue)
		{
			return null;
		}
		return inUtcTime.Value.ConvertUtcToLocalTime();
	}

	public static DateTime? ConvertLocalToUtcTime(this DateTime? inLocalTime)
	{
		if (!inLocalTime.HasValue)
		{
			return null;
		}
		return inLocalTime.Value.ConvertLocalToUtcTime();
	}

	public static int RemoveAll<T>(this ObservableCollection<T> coll, Func<T, bool> condition)
	{
		List<T> list = coll.Where(condition).ToList();
		foreach (T item in list)
		{
			coll.Remove(item);
		}
		return list.Count;
	}
}
