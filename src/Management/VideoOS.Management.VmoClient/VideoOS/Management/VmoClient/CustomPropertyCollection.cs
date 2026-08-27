using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class CustomPropertyCollection : IEnumerable<CustomProperty>, IEnumerable
{
	private IVmoRepositories _repositories;

	private Dictionary<string, CustomProperty> _internalStorage = new Dictionary<string, CustomProperty>();

	private bool _isInitialized;

	public string CustomPropertyEntityId { get; private set; }

	public int Count
	{
		get
		{
			CheckForDataLoaded(forceReload: false);
			return _internalStorage.Count;
		}
	}

	public string this[string key]
	{
		get
		{
			if (_internalStorage.ContainsKey(key))
			{
				return _internalStorage[key].Value;
			}
			if (!_isInitialized)
			{
				CustomProperty customProperty = _repositories.ServerCommunication.GetCustomProperty(CustomPropertyEntityId, key);
				if (customProperty != null)
				{
					_internalStorage[key] = customProperty;
					return _internalStorage[key].Value;
				}
			}
			return string.Empty;
		}
		set
		{
			if (_internalStorage.ContainsKey(key))
			{
				_internalStorage[key].Value = value;
			}
			else
			{
				_internalStorage[key] = new CustomProperty(key, value, dirty: true);
			}
		}
	}

	public CustomPropertyCollection(IVmoRepositories repositories, string id)
	{
		_repositories = repositories;
		CustomPropertyEntityId = id;
	}

	public CustomProperty[] DirtyCustomPropertyArray()
	{
		return new List<CustomProperty>(_internalStorage.Values).Where((CustomProperty o) => o.Dirty).ToArray();
	}

	public void Refresh()
	{
		CheckForDataLoaded(forceReload: true);
	}

	public void Remove(string key)
	{
		_repositories.ServerCommunication.DeleteCustomProperty(CustomPropertyEntityId, key);
		if (_internalStorage.ContainsKey(key))
		{
			_internalStorage.Remove(key);
		}
	}

	public IEnumerator GetEnumerator()
	{
		CheckForDataLoaded(forceReload: false);
		return _internalStorage.Values.GetEnumerator();
	}

	IEnumerator<CustomProperty> IEnumerable<CustomProperty>.GetEnumerator()
	{
		CheckForDataLoaded(forceReload: false);
		return _internalStorage.Values.GetEnumerator();
	}

	private void CheckForDataLoaded(bool forceReload)
	{
		if (!(!_isInitialized || forceReload))
		{
			return;
		}
		CustomProperty[] customProperties = _repositories.ServerCommunication.GetCustomProperties(CustomPropertyEntityId);
		_internalStorage.Clear();
		_isInitialized = true;
		if (customProperties != null)
		{
			CustomProperty[] array = customProperties;
			foreach (CustomProperty customProperty in array)
			{
				_internalStorage.Add(customProperty.Key, customProperty);
			}
		}
	}
}
