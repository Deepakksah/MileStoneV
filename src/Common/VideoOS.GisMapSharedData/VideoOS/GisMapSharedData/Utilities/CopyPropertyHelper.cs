using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VideoOS.GisMapSharedData.Utilities;

public static class CopyPropertyHelper
{
	private class PropertyMap
	{
		public PropertyInfo SourceProperty { get; set; }

		public PropertyInfo TargetProperty { get; set; }
	}

	private const string PROPERTY_SEARCH_TYPE_INHERITED = "Inherited";

	private const string PROPERTY_SEARCH_TYPE_DECLARED_ONLY = "DeclaredOnly";

	private static ConcurrentDictionary<string, PropertyMap[]> _mapping = new ConcurrentDictionary<string, PropertyMap[]>();

	private static bool IsValidAndSameType(Type source, Type target)
	{
		if (source == target)
		{
			if (source.IsValueType || target == typeof(string))
			{
				return true;
			}
			if (source.IsArray && target.IsArray && source.HasElementType && target.HasElementType)
			{
				return IsValidAndSameType(source.GetElementType(), target.GetElementType());
			}
		}
		return false;
	}

	private static IList<PropertyMap> GetMatchingProperties(Type sourceType, Type targetType, bool declaredOnly)
	{
		BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
		if (declaredOnly)
		{
			bindingFlags |= BindingFlags.DeclaredOnly;
		}
		PropertyInfo[] properties = sourceType.GetProperties(bindingFlags);
		PropertyInfo[] targetProperties = targetType.GetProperties(bindingFlags);
		return (from source in properties
			from target in targetProperties
			where source.Name == target.Name && source.CanRead && target.CanWrite && IsValidAndSameType(source.PropertyType, target.PropertyType)
			select new PropertyMap
			{
				SourceProperty = source,
				TargetProperty = target
			}).ToList();
	}

	private static string GetClassName(Type sourceType, Type targetType, bool declaredOnly)
	{
		StringBuilder stringBuilder = new StringBuilder("Copy_");
		if (declaredOnly)
		{
			stringBuilder.Append("DeclaredOnly_");
		}
		stringBuilder.Append(sourceType.FullName);
		stringBuilder.Append("_");
		stringBuilder.Append(targetType.FullName);
		return stringBuilder.ToString();
	}

	public static void AddPropertyMap<Type1, Type2>()
	{
		string className = GetClassName(typeof(Type1), typeof(Type2), declaredOnly: false);
		if (!_mapping.ContainsKey(className))
		{
			IList<PropertyMap> matchingProperties = GetMatchingProperties(typeof(Type1), typeof(Type2), declaredOnly: false);
			_mapping.TryAdd(className, matchingProperties.ToArray());
		}
		className = GetClassName(typeof(Type1), typeof(Type2), declaredOnly: true);
		if (!_mapping.ContainsKey(className))
		{
			IList<PropertyMap> matchingProperties = GetMatchingProperties(typeof(Type1), typeof(Type2), declaredOnly: true);
			_mapping.TryAdd(className, matchingProperties.ToArray());
		}
		className = GetClassName(typeof(Type2), typeof(Type1), declaredOnly: false);
		if (!_mapping.ContainsKey(className))
		{
			IList<PropertyMap> matchingProperties = GetMatchingProperties(typeof(Type2), typeof(Type1), declaredOnly: false);
			_mapping.TryAdd(className, matchingProperties.ToArray());
		}
		className = GetClassName(typeof(Type2), typeof(Type1), declaredOnly: true);
		if (!_mapping.ContainsKey(className))
		{
			IList<PropertyMap> matchingProperties = GetMatchingProperties(typeof(Type2), typeof(Type1), declaredOnly: true);
			_mapping.TryAdd(className, matchingProperties.ToArray());
		}
	}

	public static void CopyMatchingProperties<Type1, Type2>(object source, object target, bool declaredOnly)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (!(source is Type1))
		{
			throw new InvalidOperationException("source is not same or inherited type of Type1");
		}
		if (!(target is Type2))
		{
			throw new InvalidOperationException("target is not same or inherited type of Type2");
		}
		string className = GetClassName(typeof(Type1), typeof(Type2), declaredOnly);
		if (_mapping.TryGetValue(className, out var value))
		{
			foreach (PropertyMap obj in value)
			{
				object value2 = obj.SourceProperty.GetValue(source, null);
				obj.TargetProperty.SetValue(target, value2, null);
			}
			return;
		}
		throw new InvalidOperationException("Unable to find a mapping for the source and target types, use AddPropertyMap<source, target>() first");
	}
}
