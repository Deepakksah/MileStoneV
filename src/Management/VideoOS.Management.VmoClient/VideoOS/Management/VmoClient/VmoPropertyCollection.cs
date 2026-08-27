using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public class VmoPropertyCollection : IEnumerable<KeyValuePair<string, object>>, IEnumerable
{
	private Dictionary<string, object> data = new Dictionary<string, object>();

	public object this[string key] => data[key];

	public int Count => data.Count;

	public bool TryGetPropertyValue<T>(string key, out T value)
	{
		if (data.TryGetValue(key, out var value2))
		{
			Type underlyingType = Nullable.GetUnderlyingType(typeof(T));
			if (underlyingType != null && value2 == null)
			{
				value = default(T);
				return data.ContainsKey(key);
			}
			Type type = ((underlyingType != null) ? underlyingType : typeof(T));
			if (type.IsEnum)
			{
				value = (T)Enum.Parse(type, value2.ToString());
				return true;
			}
			if (type.Equals(typeof(Guid)))
			{
				value = (T)TypeDescriptor.GetConverter(type).ConvertFromInvariantString((string)value2);
				return true;
			}
			value = (T)Convert.ChangeType(value2, type, CultureInfo.InvariantCulture);
			return true;
		}
		value = default(T);
		return false;
	}

	public T GetPropertyValue<T>(string key)
	{
		if (TryGetPropertyValue<T>(key, out var value))
		{
			return value;
		}
		return default(T);
	}

	public void SetPropertyValue<T>(string key, T value)
	{
		if (typeof(T).IsEnum)
		{
			object obj = value;
			data[key] = obj.ToString();
		}
		else
		{
			data[key] = value;
		}
	}

	public bool ContainsKey(string key)
	{
		return data.ContainsKey(key);
	}

	public void Clear()
	{
		data.Clear();
	}

	public void Add(string key, object value)
	{
		data.Add(key, value);
	}

	public void Add(string key, bool value)
	{
		data.Add(key, value);
	}

	public void Add(string key, int value)
	{
		data.Add(key, value);
	}

	public void Add(string key, double value)
	{
		data.Add(key, value);
	}

	public void Add(string key, float value)
	{
		data.Add(key, value);
	}

	public void Add(string key, string value)
	{
		data.Add(key, value);
	}

	public void Add(string key, DateTime value)
	{
		data.Add(key, value);
	}

	public void Add(string key, StorageEncryptionMethod value)
	{
		data.Add(key, value);
	}

	public void Add(string key, ServiceStatus value)
	{
		data.Add(key, value);
	}

	public void Add(string key, RecordingServerDiskUsages value)
	{
		data.Add(key, value);
	}

	public void Add(string key, DeviceType value)
	{
		data.Add(key, value);
	}

	public void Add(string nestedPropertiesCollectionName, VmoPropertyCollection nestedProperties)
	{
		string key = (NestedPropertyHelper.IsNestedPropertyName(nestedPropertiesCollectionName) ? nestedPropertiesCollectionName : NestedPropertyHelper.FormatNestedPropertyName(nestedPropertiesCollectionName));
		data.Add(key, nestedProperties);
	}

	public T GetNestedPropertyOrDefault<T>(string collectionName, string key)
	{
		string key2 = NestedPropertyHelper.FormatNestedPropertyName(collectionName);
		VmoPropertyCollection propertyValue = GetPropertyValue<VmoPropertyCollection>(key2);
		if (propertyValue == null)
		{
			return default(T);
		}
		return propertyValue.GetPropertyValue<T>(key);
	}

	public void SetNestedPropertyOrDefault<T>(string nestedPropertiesCollectionName, string key, T value)
	{
		string key2 = NestedPropertyHelper.FormatNestedPropertyName(nestedPropertiesCollectionName);
		VmoPropertyCollection vmoPropertyCollection = GetPropertyValue<VmoPropertyCollection>(key2);
		if (vmoPropertyCollection == null)
		{
			vmoPropertyCollection = new VmoPropertyCollection();
			SetPropertyValue(key2, vmoPropertyCollection);
		}
		vmoPropertyCollection.SetPropertyValue(key, value);
	}

	public VmoPropertyCollection GetNestedPropertyCollection(string collectionName)
	{
		string key = NestedPropertyHelper.FormatNestedPropertyName(collectionName);
		return GetPropertyValue<VmoPropertyCollection>(key);
	}

	public void SetNestedPropertyCollection(string collectionName, VmoPropertyCollection properties)
	{
		string key = NestedPropertyHelper.FormatNestedPropertyName(collectionName);
		SetPropertyValue(key, properties);
	}

	public static VmoPropertyCollection FromKeyAndValueArray(KeyAndValue[] keyAndValues)
	{
		VmoPropertyCollection collection = new VmoPropertyCollection();
		keyAndValues.ToList().ForEach(delegate(KeyAndValue item)
		{
			collection.data.Add(item.Key, item.Value);
		});
		return collection;
	}

	public KeyAndValue[] ToKeyAndValueArray()
	{
		return (from pair in data.ToList()
			select new KeyAndValue(pair.Key, pair.Value)).ToArray();
	}

	public static VmoPropertyCollection FromKeyValueInfoArray(KeyValueInfo[] keyValues)
	{
		VmoPropertyCollection collection = new VmoPropertyCollection();
		keyValues.ToList().ForEach(delegate(KeyValueInfo item)
		{
			collection.data.Add(item.Key, item.Value);
		});
		return collection;
	}

	public Dictionary<string, object> ToDictionary()
	{
		return new Dictionary<string, object>(data);
	}

	public void ImportFromDictionary(Dictionary<string, object> dictionary)
	{
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			data.Add(item.Key, item.Value);
		}
	}

	public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		return data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return data.GetEnumerator();
	}
}
