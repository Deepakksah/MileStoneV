using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces;

public static class ParametersExtension
{
	public static bool GetValueAsBoolean(this Dictionary<string, string> parameters, string paramName)
	{
		return parameters.GetValueAsBoolean(paramName, fallbackValue: false);
	}

	public static bool GetValueAsBoolean(this Dictionary<string, string> parameters, string paramName, bool fallbackValue)
	{
		return GetValueAsNullableBoolean(parameters, paramName, fallbackValue).Value;
	}

	public static bool? GetValueAsNullableBoolean(this Dictionary<string, string> parameters, string paramName)
	{
		return GetValueAsNullableBoolean(parameters, paramName, null);
	}

	private static bool? GetValueAsNullableBoolean(Dictionary<string, string> parameters, string paramName, bool? fallbackValue)
	{
		if (!parameters.TryGetValue(paramName, out var value))
		{
			return fallbackValue;
		}
		if (!bool.TryParse(value, out var result))
		{
			return fallbackValue;
		}
		return result;
	}

	public static string GetValueOrNull(this Dictionary<string, string> parameters, string paramName)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (!parameters.TryGetValue(paramName, out var value))
		{
			return null;
		}
		return value;
	}
}
