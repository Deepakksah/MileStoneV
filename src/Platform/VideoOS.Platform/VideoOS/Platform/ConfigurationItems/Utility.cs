using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

public static class Utility
{
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
}
