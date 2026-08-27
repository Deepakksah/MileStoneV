using System;
using System.Collections.Generic;
using System.Reflection;
using VideoOS.Mobile.Management;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Mappings;

public static class MappingHelper
{
	public static string GetValue(this Mapping mapping, Dictionary<string, string> settings, string keyPrefix = null)
	{
		string text = mapping.GetNonMappedValue(settings, keyPrefix);
		try
		{
			if (mapping.Mapper != null)
			{
				text = mapping.Mapper(settings, 0, text);
			}
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return text;
	}

	public static string GetNonMappedValue(this Mapping mapping, Dictionary<string, string> settings, string keyPrefix = null)
	{
		string key = (keyPrefix ?? string.Empty) + mapping.Key;
		return settings.GetValueOrDefault(key, string.Empty);
	}

	public static string GetIndexedValue(this Mapping mapping, Dictionary<string, string> settings, int index)
	{
		string text = settings[DictionaryHelper.GetParamPerIndexedValue(mapping.Key, index)];
		try
		{
			if (mapping.Mapper != null)
			{
				text = mapping.Mapper(settings, index, text);
			}
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
		return text;
	}
}
