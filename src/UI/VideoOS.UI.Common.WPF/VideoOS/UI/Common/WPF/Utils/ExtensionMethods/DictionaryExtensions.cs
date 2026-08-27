using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class DictionaryExtensions
{
	public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue, bool addDefaultValueToDictionary = false)
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			return value;
		}
		if (addDefaultValueToDictionary)
		{
			dictionary[key] = defaultValue;
		}
		return defaultValue;
	}
}
