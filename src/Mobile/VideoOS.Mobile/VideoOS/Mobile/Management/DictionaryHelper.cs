using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Management;

public static class DictionaryHelper
{
	public static string GetProperty(this Dictionary<string, string> properties, string propertyName, int? index = null)
	{
		string result = null;
		string text = (index.HasValue ? index.ToString() : "");
		string key = propertyName + text;
		if (properties != null && properties.ContainsKey(key))
		{
			result = properties[key];
		}
		return result;
	}

	public static bool GetProperty(this Dictionary<string, string> item, string propertyName, out int propertyValue)
	{
		return int.TryParse(item.GetProperty(propertyName), out propertyValue);
	}

	public static bool GetProperty(this Dictionary<string, string> item, string propertyName, out bool propertyValue)
	{
		return bool.TryParse(item.GetProperty(propertyName), out propertyValue);
	}

	public static void SetProperty<T>(this Dictionary<string, string> item, string propertyName, T propertyValue, Func<T, string> convFunc = null)
	{
		if (item != null && propertyValue != null)
		{
			string value = propertyValue.ToString();
			if (convFunc != null)
			{
				value = convFunc(propertyValue);
			}
			item[propertyName + ""] = value;
		}
	}

	public static void SetProperty<T>(this Dictionary<string, string> item, string propertyName, int index, T propertyValue, Func<T, string> convFunc = null)
	{
		if (item != null)
		{
			string value = propertyValue.ToString();
			if (convFunc != null)
			{
				value = convFunc(propertyValue);
			}
			item[propertyName + index] = value;
		}
	}

	public static void InitFromSettings<T>(ref T value, Dictionary<string, string> settings, int? index, string name, Func<string, T> converter)
	{
		try
		{
			if (settings != null)
			{
				string paramPerIndexedValue = GetParamPerIndexedValue(name, index);
				if (settings.ContainsKey(paramPerIndexedValue))
				{
					value = converter(settings[paramPerIndexedValue]);
				}
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	public static string GetParamPerIndexedValue(string name, int? index)
	{
		if (!index.HasValue)
		{
			return name;
		}
		int? num = index;
		return name + num;
	}
}
