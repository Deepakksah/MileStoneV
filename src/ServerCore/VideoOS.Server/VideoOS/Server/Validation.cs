using System;
using System.Text.RegularExpressions;

namespace VideoOS.Server;

public static class Validation
{
	public static void CheckName(string name, string parameterName)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException(parameterName);
		}
	}

	public static void CheckVersionString(string value, string parameterName)
	{
		if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, "^(\\d+\\.){3}(\\d+)$"))
		{
			throw new ArgumentException("Invalid value", parameterName);
		}
	}
}
