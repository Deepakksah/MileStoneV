using System;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public abstract class GenericObjectHelper2
{
	public static T GetProperty<T>(ObjectProperty2[] properties, string propertyName)
	{
		ObjectProperty2 objectProperty = properties.First((ObjectProperty2 property) => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
		if (typeof(T).IsEnum)
		{
			return (T)Enum.Parse(typeof(T), objectProperty.Value.ToString());
		}
		return (T)Convert.ChangeType(objectProperty.Value, typeof(T), CultureInfo.InvariantCulture);
	}

	public static void SetPropertyValue<T>(ObjectProperty2[] properties, string propertyName, T value)
	{
		properties.First((ObjectProperty2 property) => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)).Value = value;
	}

	public static bool Contains(ObjectProperty2[] properties, string propertyName)
	{
		return properties.Any((ObjectProperty2 property) => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
	}

	public static ObjectProperty2[] FromKeyAndValue(KeyAndValue[] keyAndValues)
	{
		return (from kv in keyAndValues.ToList()
			select ObjectProperty2.FromKeyAndValue(kv)).ToArray();
	}

	public static KeyAndValue[] FromObjectProperty2(ObjectProperty2[] properties)
	{
		return (from p in properties.ToList()
			select new KeyAndValue(p.Name, p.Value)).ToArray();
	}

	public static GenericObject2 SerializeNestedProperties(GenericObject2 obj)
	{
		if (obj == null)
		{
			return null;
		}
		foreach (ObjectProperty2 item in obj.Properties.Where((ObjectProperty2 p) => NestedPropertyHelper.IsNestedPropertyName(p.Name)))
		{
			string value = KeyAndValue.ToXml(FromObjectProperty2(GetProperty<ObjectProperty2[]>(obj.Properties, item.Name)));
			SetPropertyValue(obj.Properties, item.Name, value);
		}
		return obj;
	}

	public static GenericObject2[] SerializeNestedProperties(GenericObject2[] objs)
	{
		if (objs == null)
		{
			return null;
		}
		objs.ToList().ForEach(delegate(GenericObject2 o)
		{
			SerializeNestedProperties(o);
		});
		return objs;
	}

	public static GenericObject2 DeserializeNestedProperties(GenericObject2 obj)
	{
		DeserializeNestedProperties(obj.Properties);
		return obj;
	}

	public static ObjectProperty2[] DeserializeNestedProperties(ObjectProperty2[] properties)
	{
		foreach (ObjectProperty2 item in properties.Where((ObjectProperty2 p) => NestedPropertyHelper.IsNestedPropertyName(p.Name)))
		{
			ObjectProperty2[] value = FromKeyAndValue(KeyAndValue.FromXml(GetProperty<string>(properties, item.Name)));
			SetPropertyValue(properties, item.Name, value);
		}
		return properties;
	}

	public static T GetNestedPropertyOrDefault<T>(ObjectProperty2[] properties, string nestedPropertiesCollectionName, string propertyName)
	{
		string nestedPropertiesPropertyName = NestedPropertyHelper.FormatNestedPropertyName(nestedPropertiesCollectionName);
		ObjectProperty2 objectProperty = properties.FirstOrDefault((ObjectProperty2 property) => property.Name.Equals(nestedPropertiesPropertyName, StringComparison.OrdinalIgnoreCase));
		if (objectProperty != null && objectProperty.Value != null)
		{
			ObjectProperty2 objectProperty2 = ((ObjectProperty2[])objectProperty.Value).FirstOrDefault((ObjectProperty2 property) => property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
			if (objectProperty2 != null && objectProperty2.Value != null)
			{
				return (T)objectProperty2.Value;
			}
			return default(T);
		}
		return default(T);
	}

	public static ObjectProperty2[] GetNestedProperties(ObjectProperty2[] properties, string nestedPropertiesCollectionName)
	{
		string nestedPropertiesPropertyName = NestedPropertyHelper.FormatNestedPropertyName(nestedPropertiesCollectionName);
		ObjectProperty2 objectProperty = properties.FirstOrDefault((ObjectProperty2 property) => property.Name.Equals(nestedPropertiesPropertyName, StringComparison.OrdinalIgnoreCase));
		if (objectProperty != null && objectProperty.Value != null)
		{
			return (ObjectProperty2[])objectProperty.Value;
		}
		return null;
	}
}
