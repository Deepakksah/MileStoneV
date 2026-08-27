using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class PropertyInfoDetails
{
	public string Name { get; }

	public bool IsSimpleTypeWildcard => Name == "*";

	public bool IsCollection { get; }

	public bool IsRecursive { get; }

	public TypeFilterBase TypeFilter { get; }

	public PropertyInfoDetails(string name, bool isCollection, bool isRecursive, TypeFilterBase typeFilter)
	{
		Name = name;
		IsCollection = isCollection;
		IsRecursive = isRecursive;
		TypeFilter = typeFilter;
	}

	public bool TryGetValue(object obj, out object value)
	{
		value = null;
		if (obj == null)
		{
			return false;
		}
		try
		{
			value = obj.GetType().GetProperty(Name).GetValue(obj);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public bool TryGetValues(object obj, out List<ObjectValueInfo> values)
	{
		values = new List<ObjectValueInfo>();
		if (obj == null)
		{
			return false;
		}
		PropertyInfo[] properties = obj.GetType().GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (IsSimpleType(propertyInfo.PropertyType, out var _))
			{
				try
				{
					object value = propertyInfo.GetValue(obj);
					values.Add(new ObjectValueInfo(propertyInfo.Name, value, value?.GetType() ?? propertyInfo.PropertyType));
				}
				catch
				{
				}
			}
		}
		return values.Any();
	}

	public static bool IsSimpleType(Type type, out TypeInfo simplePropertyTypeInfo)
	{
		simplePropertyTypeInfo = type.GetTypeInfo();
		if (!simplePropertyTypeInfo.IsPrimitive && !simplePropertyTypeInfo.IsEnum && !(type == typeof(string)))
		{
			return type == typeof(decimal);
		}
		return true;
	}
}
