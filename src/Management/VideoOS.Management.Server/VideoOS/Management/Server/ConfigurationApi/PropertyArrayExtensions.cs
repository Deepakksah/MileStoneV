using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class PropertyArrayExtensions
{
	private static Dictionary<string, Type[]> ValueTypeMap = new Dictionary<string, Type[]>
	{
		{
			"DateTime",
			new Type[1] { typeof(DateTime) }
		},
		{
			"Button",
			new Type[1] { typeof(string) }
		},
		{
			"Double",
			new Type[1] { typeof(double) }
		},
		{
			"Int",
			new Type[2]
			{
				typeof(int),
				typeof(long)
			}
		},
		{
			"Label",
			new Type[1] { typeof(string) }
		},
		{
			"Path",
			new Type[1] { typeof(string) }
		},
		{
			"Progress",
			new Type[1] { typeof(int) }
		},
		{
			"Separator",
			new Type[1] { typeof(string) }
		},
		{
			"Slider",
			new Type[1] { typeof(int) }
		},
		{
			"String",
			new Type[1] { typeof(string) }
		},
		{
			"Tick",
			new Type[1] { typeof(bool) }
		}
	};

	public static bool TryGetPropertyValue(this IEnumerable<Property> properties, string propertyName, out string value)
	{
		value = null;
		Property property = properties.FirstOrDefault((Property prop) => prop.Key.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
		if (property == null)
		{
			return false;
		}
		if (property.Value == null)
		{
			return false;
		}
		value = property.Value;
		return true;
	}

	public static bool TryGetPropertyValue(this IEnumerable<Property> properties, string propertyName, out string[] values)
	{
		values = null;
		Property property = properties.FirstOrDefault((Property prop) => prop.Key.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
		if (property?.ValueArray == null)
		{
			return false;
		}
		values = property.ValueArray.ToArray();
		return true;
	}

	public static bool TryGetPropertyValue(this IEnumerable<Property> properties, string propertyName, out int[] values)
	{
		values = null;
		Property property = properties.FirstOrDefault((Property prop) => prop.Key.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
		if (property?.ValueArray == null)
		{
			return false;
		}
		values = property.ValueArray.Select((string v) => ConvertPropertyValue<int>(v, "Int", propertyName).Value).ToArray();
		return true;
	}

	public static Property GetProperty(this IEnumerable<Property> properties, string propertyName)
	{
		return properties.FirstOrDefault((Property prop) => prop.Key.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
	}

	public static bool TryGetPropertyValue<T>(this IEnumerable<Property> properties, string propertyName, out T? value) where T : struct
	{
		value = null;
		Property property = properties.FirstOrDefault((Property prop) => prop.Key.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
		if (property == null)
		{
			return false;
		}
		if (property.Value == null)
		{
			return false;
		}
		value = ConvertPropertyValue<T>(property.Value, property.ValueType, property.Key);
		return true;
	}

	private static T? ConvertPropertyValue<T>(string propertyValue, string propertyValueType, string propertyName) where T : struct
	{
		if (string.IsNullOrEmpty(propertyValue) && propertyValueType == "DateTime")
		{
			return null;
		}
		if (typeof(T).IsEnum)
		{
			try
			{
				return (T)Enum.Parse(typeof(T), propertyValue);
			}
			catch (ArgumentException innerException)
			{
				throw new ArgumentException("Unable to cast property value to the desired type.", propertyName, innerException);
			}
			catch (OverflowException innerException2)
			{
				throw new ArgumentException("Unable to cast property value to the desired type.", propertyName, innerException2);
			}
		}
		try
		{
			return (T)Convert.ChangeType(propertyValue, typeof(T), CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException innerException3)
		{
			throw new ArgumentException("Unable to cast property value to the desired type.", propertyName, innerException3);
		}
		catch (FormatException innerException4)
		{
			throw new ArgumentException("Unable to cast property value to the desired type.", propertyName, innerException4);
		}
		catch (OverflowException innerException5)
		{
			throw new ArgumentException("Unable to cast property value to the desired type.", propertyName, innerException5);
		}
	}

	public static void SetValue(this IEnumerable<Property> properties, string key, string value, bool ignoreSettable)
	{
		ValidateValue(properties, key, ignoreSettable).Value = value;
	}

	public static void SetValue(this IEnumerable<Property> properties, string key, string value)
	{
		properties.SetValue(key, value, ignoreSettable: false);
	}

	public static void SetValue<T>(this IEnumerable<Property> properties, string key, T value, bool ignoreSettable)
	{
		Property property = ValidateValue(properties, key, ignoreSettable);
		ValidateValueType<T>(property);
		property.Value = PropertyTypesHelper.GetValue(value);
	}

	public static void SetValue<T>(this IEnumerable<Property> properties, string key, T value) where T : struct
	{
		properties.SetValue(key, value, ignoreSettable: false);
	}

	private static Property ValidateValue(IEnumerable<Property> properties, string key, bool ignoreSettable)
	{
		Property obj = properties.FirstOrDefault((Property prop) => prop.Key.Equals(key, StringComparison.OrdinalIgnoreCase)) ?? throw new KeyNotFoundException("key");
		if (!obj.IsSettable && !ignoreSettable)
		{
			throw new ArgumentException("value is not settable");
		}
		return obj;
	}

	private static void ValidateValueType<T>(Property property)
	{
		if (property.ValueType == "Enum" && !typeof(T).IsEnum)
		{
			throw new ArgumentException("value is not a valid type");
		}
		if (ValueTypeMap.TryGetValue(property.ValueType, out var value) && !value.Contains(typeof(T)))
		{
			throw new ArgumentException("value is not a valid type");
		}
	}
}
