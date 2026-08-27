using System;
using System.ComponentModel;
using System.Globalization;

namespace VideoOS.Management.Server.ConfigurationApi.Rules;

public static class TypeHelper
{
	public static T FromString<T>(string value, string errorMessageFormat, params string[] args)
	{
		try
		{
			return (!string.IsNullOrEmpty(value)) ? ((T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(null, CultureInfo.InvariantCulture, value)) : default(T);
		}
		catch (NotSupportedException)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Conversion to {0} not supported", typeof(T).Name));
		}
		catch
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, errorMessageFormat, args));
		}
	}

	public static string ToString<T>(T value)
	{
		if (object.Equals(value, null))
		{
			return "null";
		}
		return value.ToString();
	}
}
