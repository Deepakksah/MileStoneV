using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class CapabilitiesHelper
{
	public delegate T Convertor<T>(string value);

	public static readonly Convertor<bool> BoolConvertor = (string value) => value == "Yes";

	public static T Extract<T>(Dictionary<string, string> serverParams, string paramName, Convertor<T> convertor, T defaultValue)
	{
		T result = defaultValue;
		if (serverParams.ContainsKey(paramName))
		{
			result = convertor(serverParams[paramName]);
		}
		return result;
	}

	public static bool ExtractBool(Dictionary<string, string> serverParams, string paramName, bool defaultValue)
	{
		return Extract(serverParams, paramName, BoolConvertor, defaultValue);
	}

	public static Guid ExtractGuid(Dictionary<string, string> serverParams, string paramName, Guid defaultValue)
	{
		return Extract(serverParams, paramName, ParamsHelper.GuidConvertion, defaultValue);
	}

	public static void CopyDictionary<TKey, TValue>(Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> destination)
	{
		foreach (KeyValuePair<TKey, TValue> item in source)
		{
			destination[item.Key] = item.Value;
		}
	}
}
