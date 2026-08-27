using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using VideoOS.IdentityServer.Extensions;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public static class ScimUserMapper
{
	private static readonly PropertyInfo[] ScimUserProperties = typeof(ScimUser).GetProperties();

	public static ExpandoObject GenerateScimUserResponse<T>(T response, string[] attributes) where T : IScimUserResponse
	{
		IDictionary<string, object> dictionary = new ExpandoObject();
		AddCustomProperties(dictionary, response, "ScimUser");
		ExpandoObject expandoObject = GenerateResponse(response.ScimUser, attributes, ScimUserProperties);
		foreach (KeyValuePair<string, object> item in (IEnumerable<KeyValuePair<string, object>>)expandoObject)
		{
			dictionary.Add(item);
		}
		return (ExpandoObject)dictionary;
	}

	public static ExpandoObject GenerateScimUserListResponse<T>(T response, string[] attributes, string[] removeAttributes = null) where T : IScimUserListResponse
	{
		IDictionary<string, object> dictionary = new ExpandoObject();
		if (removeAttributes == null)
		{
			removeAttributes = Array.Empty<string>();
		}
		AddCustomProperties(dictionary, response, "Resources");
		List<ExpandoObject> value = response.Resources.Select((ScimUser user) => GenerateResponse(user, attributes, ScimUserProperties, removeAttributes)).ToList();
		dictionary["Resources".ToLowerFirstChar()] = value;
		return (ExpandoObject)dictionary;
	}

	public static ExpandoObject GenerateResponse(object sourceObject, string[] attributes, PropertyInfo[] properties, string[] removedAttributes = null)
	{
		if (removedAttributes == null)
		{
			removedAttributes = Array.Empty<string>();
		}
		if (attributes != null)
		{
			attributes = ValidateAndExpandAttributes(sourceObject.GetType(), attributes).ToArray();
		}
		removedAttributes = ValidateAndExpandAttributes(sourceObject.GetType(), removedAttributes).ToArray();
		return GenerateResponseRecursive(sourceObject, attributes, properties, removedAttributes);
	}

	private static List<string> ValidateAndExpandAttributes(Type type, IEnumerable<string> attributes)
	{
		List<string> list = new List<string>();
		foreach (string attribute in attributes)
		{
			string[] array = attribute.Split('.');
			if (IsValidPropertyPath(type, array, out var finalType))
			{
				if (array.Length == 1 && !attribute.Contains('.'))
				{
					List<string> nestedPropertyPaths = GetNestedPropertyPaths(finalType, attribute);
					list.AddRange(nestedPropertyPaths);
				}
				else
				{
					list.Add(attribute);
				}
			}
		}
		return list;
	}

	private static bool IsValidPropertyPath(Type type, string[] propertyPathSegments, out Type finalType)
	{
		finalType = type;
		foreach (string name in propertyPathSegments)
		{
			PropertyInfo property = finalType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (property == null)
			{
				finalType = null;
				return false;
			}
			finalType = property.PropertyType;
		}
		return true;
	}

	private static List<string> GetNestedPropertyPaths(Type type, string parentPath)
	{
		List<string> list = new List<string> { parentPath };
		if (type == typeof(string) || !type.IsClass)
		{
			return list;
		}
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			string parentPath2 = parentPath + "." + propertyInfo.Name;
			list.AddRange(GetNestedPropertyPaths(propertyInfo.PropertyType, parentPath2));
		}
		return list;
	}

	private static ExpandoObject GenerateResponseRecursive(object sourceObject, string[] attributes, PropertyInfo[] properties, string[] removedAttributes)
	{
		IDictionary<string, object> dictionary = new ExpandoObject();
		bool flag = attributes == null || attributes.Length == 0;
		foreach (PropertyInfo prop in properties)
		{
			bool flag2 = prop.GetCustomAttribute<ScimSchemaAttribute>()?.Returned == "always";
			if (prop.GetIndexParameters().Length != 0 || removedAttributes.Contains<string>(prop.Name, StringComparer.OrdinalIgnoreCase))
			{
				continue;
			}
			object value = prop.GetValue(sourceObject);
			if (value == null)
			{
				continue;
			}
			string[] array = attributes?.Where((string a) => a.StartsWith(prop.Name + ".", StringComparison.OrdinalIgnoreCase)).Select(delegate(string a)
			{
				int num = prop.Name.Length + 1;
				return a.Substring(num, a.Length - num);
			}).ToArray();
			string[] removedAttributes2 = removedAttributes.Where((string a) => a.StartsWith(prop.Name + ".", StringComparison.OrdinalIgnoreCase)).Select(delegate(string a)
			{
				int num = prop.Name.Length + 1;
				return a.Substring(num, a.Length - num);
			}).Concat(removedAttributes.Where((string a) => !a.Contains('.')))
				.ToArray();
			if (!flag && !attributes.Contains<string>(prop.Name, StringComparer.OrdinalIgnoreCase) && !flag2 && array.Length == 0)
			{
				continue;
			}
			if (value is IEnumerable<object> source && value.GetType() != typeof(string))
			{
				dictionary[prop.Name.ToLowerFirstChar()] = source.ToArray();
			}
			else if (value.GetType().IsClass && value.GetType() != typeof(string))
			{
				PropertyInfo[] properties2 = value.GetType().GetProperties();
				ExpandoObject expandoObject = GenerateResponseRecursive(value, array, properties2, removedAttributes2);
				if (((ICollection<KeyValuePair<string, object>>)expandoObject).Count > 0)
				{
					dictionary[prop.Name.ToLowerFirstChar()] = expandoObject;
				}
			}
			else if (array == null || array.Length <= 0)
			{
				dictionary[prop.Name.ToLowerFirstChar()] = value;
			}
		}
		return (ExpandoObject)dictionary;
	}

	private static void AddCustomProperties<T>(IDictionary<string, object> result, T response, string excludePropertyName)
	{
		PropertyInfo[] properties = typeof(T).GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanRead && propertyInfo.Name != excludePropertyName)
			{
				result[propertyInfo.Name.ToLowerFirstChar()] = propertyInfo.GetValue(response);
			}
		}
	}
}
