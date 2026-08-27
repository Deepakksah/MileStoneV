using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules;

public static class ConfigurationItemExtensions
{
	public static bool IsPropertyPresent(this ConfigurationItem item, string propertyName)
	{
		item.NullCheck("", "IsPropertyPresent", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 21);
		return item.Properties?.Any((Property x) => x.Key == propertyName) ?? false;
	}

	public static bool IsPropertyValuePresent(this ConfigurationItem item, string propertyName)
	{
		item.NullCheck("", "IsPropertyValuePresent", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 33);
		return !string.IsNullOrEmpty((item.Properties?.FirstOrDefault((Property x) => x.Key == propertyName))?.Value);
	}

	public static bool IsPropertySelected(this ConfigurationItem item, string propertyName)
	{
		item.NullCheck("", "IsPropertySelected", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 46);
		bool result;
		return item.TryGetPropertyValue(propertyName, out result) && result;
	}

	public static bool TryGetProperty(this ConfigurationItem item, string propertyName, out string result)
	{
		item.NullCheck("", "TryGetProperty", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 60);
		Property property = item.Properties?.FirstOrDefault((Property x) => x.Key == propertyName);
		result = property?.Value;
		return property != null;
	}

	public static bool TryGetProperty<T>(this ConfigurationItem item, string propertyName, out T result)
	{
		item.NullCheck("", "TryGetProperty", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 76);
		result = default(T);
		Property property = item.Properties?.FirstOrDefault((Property x) => x.Key == propertyName);
		if (property == null)
		{
			return false;
		}
		if (!string.IsNullOrEmpty(property.Value))
		{
			result = TypeHelper.FromString<T>(property.Value, "Invalid {0}: '{1}'", new string[2] { propertyName, property.Value });
		}
		return true;
	}

	public static bool TryGetPropertyValue(this ConfigurationItem item, string propertyName, out string result)
	{
		item.NullCheck("", "TryGetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 102);
		if (item.TryGetProperty(propertyName, out result))
		{
			return !string.IsNullOrEmpty(result);
		}
		return false;
	}

	public static bool TryGetPropertyValue<T>(this ConfigurationItem item, string propertyName, out T result)
	{
		item.NullCheck("", "TryGetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 117);
		result = default(T);
		if (item.TryGetProperty(propertyName, out var result2) && !string.IsNullOrEmpty(result2))
		{
			try
			{
				result = TypeHelper.FromString<T>(result2, "Invalid {0}: '{1}'", new string[2] { propertyName, result2 });
				return true;
			}
			catch (ArgumentException)
			{
			}
		}
		return false;
	}

	public static bool TryGetPropertyValueIfValid(this ConfigurationItem item, string propertyName, ICollection<string> allowedValues, out string result)
	{
		item.NullCheck("", "TryGetPropertyValueIfValid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 147);
		allowedValues.NullCheck("", "TryGetPropertyValueIfValid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 148);
		if (item.TryGetPropertyValue(propertyName, out result))
		{
			return allowedValues.Contains(result);
		}
		return false;
	}

	public static bool TryGetPropertyValueIfValid<T>(this ConfigurationItem item, string propertyName, ICollection<string> allowedValues, out T result)
	{
		item.NullCheck("", "TryGetPropertyValueIfValid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 165);
		allowedValues.NullCheck("", "TryGetPropertyValueIfValid", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 166);
		if (item.TryGetPropertyValueIfValid(propertyName, allowedValues, out var result2))
		{
			result = TypeHelper.FromString<T>(result2, "Invalid {0}: '{1}'", new string[2] { propertyName, result2 });
			return true;
		}
		result = default(T);
		return false;
	}

	public static string GetPropertyValue(this ConfigurationItem item, string propertyName)
	{
		item.NullCheck("", "GetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 186);
		if (!item.TryGetPropertyValue(propertyName, out var result))
		{
			MissingValueError(propertyName);
		}
		return result;
	}

	public static T GetPropertyValue<T>(this ConfigurationItem item, string propertyName)
	{
		item.NullCheck("", "GetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 208);
		if (!item.TryGetPropertyValue(propertyName, out var result))
		{
			MissingValueError(propertyName);
		}
		return TypeHelper.FromString<T>(result, "Invalid {0}: '{1}'", new string[2] { propertyName, result });
	}

	public static string GetPropertyValue(this ConfigurationItem item, string propertyName, ICollection<string> allowedValues)
	{
		item.NullCheck("", "GetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 227);
		allowedValues.NullCheck("", "GetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 228);
		if (!item.TryGetPropertyValue(propertyName, out var result))
		{
			MissingValueError(propertyName);
		}
		if (!allowedValues.Contains(result))
		{
			InvalidValueError(propertyName, result);
		}
		return result;
	}

	public static T GetPropertyValue<T>(this ConfigurationItem item, string propertyName, ICollection<string> allowedValues)
	{
		item.NullCheck("", "GetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 253);
		allowedValues.NullCheck("", "GetPropertyValue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\ConfigurationItemExtensions.cs", 254);
		string propertyValue = item.GetPropertyValue(propertyName, allowedValues);
		return TypeHelper.FromString<T>(propertyValue, "Invalid {0}: '{1}'", new string[2] { propertyName, propertyValue });
	}

	private static void MissingValueError(string name)
	{
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} is empty", name));
	}

	private static void InvalidValueError(string name, string value)
	{
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid {0}: '{1}'", name, value));
	}
}
