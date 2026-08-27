using System;
using System.Reflection;

namespace VideoOS.Platform.UI.Extensions;

public static class ObjectExtensionMethods
{
	public static TPropertyType ResolvePropertyValue<TPropertyType>(this object obj, string propertyName, bool throwOnError = true, TPropertyType defaultValue = default(TPropertyType), BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
	{
		if (obj == null && throwOnError)
		{
			throw new ArgumentNullException("obj");
		}
		if (string.IsNullOrWhiteSpace(propertyName) && throwOnError)
		{
			throw new ArgumentException("Invalid property name", "propertyName");
		}
		if (obj != null && !string.IsNullOrWhiteSpace(propertyName))
		{
			PropertyInfo property = obj.GetType().GetProperty(propertyName, bindingFlags);
			if (property == null)
			{
				if (throwOnError)
				{
					throw new ArgumentException(FormattableString.Invariant($"Property not present on type '{typeof(TPropertyType).Name}'"), propertyName);
				}
			}
			else
			{
				if (typeof(TPropertyType).IsAssignableFrom(property.PropertyType))
				{
					return (TPropertyType)property.GetValue(obj);
				}
				if (throwOnError)
				{
					throw new ArgumentException(FormattableString.Invariant($"Value is not a {typeof(TPropertyType).Name} instance"), propertyName);
				}
			}
		}
		return defaultValue;
	}
}
