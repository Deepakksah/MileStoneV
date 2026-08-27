using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Management.Server;

public static class ObjectProperty2ArrayExtensions
{
	public static T GetPropertyOrDefault<T>(this IEnumerable<ObjectProperty2> properties, string propertyName) where T : class
	{
		T value = null;
		if (!properties.TryGetProperty<T>(propertyName, out value))
		{
			return null;
		}
		return value;
	}

	public static bool TryGetProperty<T>(this IEnumerable<ObjectProperty2> properties, string propertyName, out T value) where T : class
	{
		value = null;
		ObjectProperty2 objectProperty = properties.FirstOrDefault((ObjectProperty2 p) => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
		if (objectProperty == null)
		{
			return false;
		}
		if (typeof(T).IsEnum)
		{
			value = (T)Enum.Parse(typeof(T), objectProperty.Value.ToString());
		}
		else
		{
			value = (T)Convert.ChangeType(objectProperty.Value, typeof(T), CultureInfo.InvariantCulture);
		}
		return true;
	}

	public static T? GetPropertyOrNullable<T>(this IEnumerable<ObjectProperty2> properties, string propertyName) where T : struct
	{
		if (!properties.TryGetProperty<T>(propertyName, out T? value))
		{
			return null;
		}
		return value;
	}

	public static bool TryGetProperty<T>(this IEnumerable<ObjectProperty2> properties, string propertyName, out T? value) where T : struct
	{
		value = null;
		ObjectProperty2 objectProperty = properties.FirstOrDefault((ObjectProperty2 p) => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
		if (objectProperty == null)
		{
			return false;
		}
		if (typeof(T).IsEnum)
		{
			value = (T)Enum.Parse(typeof(T), objectProperty.Value.ToString());
		}
		else
		{
			value = (T)Convert.ChangeType(objectProperty.Value, typeof(T), CultureInfo.InvariantCulture);
		}
		return true;
	}
}
