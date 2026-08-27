using System.Collections.ObjectModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class ConfigurationApiProperties
{
	private ConfigurationApiItemWrapper _wrapper;

	public Collection<string> Keys
	{
		get
		{
			Collection<string> collection = new Collection<string>();
			if (_wrapper.ConfigurationItem.Properties != null)
			{
				bool flag = _wrapper.ConfigurationItem.ItemType == "DeviceDriverSettings" || _wrapper.ConfigurationItem.ItemType == "Stream";
				Property[] properties = _wrapper.ConfigurationItem.Properties;
				foreach (Property property in properties)
				{
					if (flag && (property.Key.Contains(".") || property.Key.Contains(":") || property.Key.Contains("/")))
					{
						string[] array = property.Key.Split('/');
						if (array.Length > 2)
						{
							collection.Add(array[1]);
							continue;
						}
						collection.Add(property.Key);
						EnvironmentManager.Instance.Log(error: true, "ConfigurationApi.Properties", "Key contains a '.' but no '/' - expected from DeviceDriverSettings xml keys" + property.Key);
					}
					else
					{
						collection.Add(property.Key);
					}
				}
			}
			return collection;
		}
	}

	public Collection<string> KeysFullName
	{
		get
		{
			Collection<string> collection = new Collection<string>();
			if (_wrapper.ConfigurationItem.Properties != null)
			{
				Property[] properties = _wrapper.ConfigurationItem.Properties;
				foreach (Property property in properties)
				{
					collection.Add(property.Key);
				}
			}
			return collection;
		}
	}

	public ConfigurationApiProperties(ConfigurationApiItemWrapper wrapper)
	{
		_wrapper = wrapper;
	}

	public string GetValue(string keyOrKeyFullName)
	{
		return _wrapper.GetProperty(keyOrKeyFullName);
	}

	public void SetValue(string keyOrKeyFullName, string newValue)
	{
		_wrapper.SetProperty(keyOrKeyFullName, newValue);
	}

	public Collection<ValueTypeInformation> GetValueTypeInfoCollection(string keyOrKeyFullName)
	{
		Collection<ValueTypeInformation> collection = new Collection<ValueTypeInformation>();
		if (_wrapper.ConfigurationItem.Properties != null)
		{
			Property[] properties = _wrapper.ConfigurationItem.Properties;
			foreach (Property property in properties)
			{
				if (!(property.Key == keyOrKeyFullName) && !property.Key.Contains("/" + keyOrKeyFullName + "/"))
				{
					continue;
				}
				if (property.ValueTypeInfos != null)
				{
					ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
					foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
					{
						collection.Add(new ValueTypeInformation(valueTypeInfo));
					}
				}
				break;
			}
		}
		return collection;
	}
}
