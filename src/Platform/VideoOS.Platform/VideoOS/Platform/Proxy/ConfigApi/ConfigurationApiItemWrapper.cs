using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.Proxy.ConfigApi;

public class ConfigurationApiItemWrapper
{
	private ConfigurationItem _configItem;

	private readonly ServerId _serverId;

	private ConfigurationApiItemWrapper _parentWrapper;

	private bool _isDirty;

	private IConfigurationClientDecorator _configurationService;

	private IConfigurationClientDecorator _configurationServiceConfigApi;

	public static readonly string InvokeInfoStates_Success = "Success";

	public static readonly string InvokeInfoProperty_State = "State";

	public static readonly string ItemTypes_InvokeResult = "InvokeResult";

	public static readonly string ItemTypes_InvokeInfo = "InvokeInfo";

	private static string[] _modifyingCacheMethods = new string[4] { "Add", "Remove", "ClientProfile", "SetClientProfile" };

	public ConfigurationApiItemWrapper OriginalWrapperFromExecuteMethod;

	private static Collection<string> _hasChildItems = new Collection<string>
	{
		"TimeProfile", "ArchiveStorage", "ClientProfile", "DeviceDriverSettings", "HardwareDeviceEvent", "HardwareDriverSettings", "HardwarePtzSettings", "LicenseDetailFolder", "LicenseInstalledProductFolder", "PatrollingProfile",
		"SiteAddressFolder", "SystemAddressFolder", "StreamDefinition", "ViewItem"
	};

	public ConfigurationItem ConfigurationItem
	{
		get
		{
			return _configItem;
		}
		set
		{
			_configItem = value;
		}
	}

	public bool IsValid => _configItem != null;

	public string[] MethodIds
	{
		get
		{
			if (_configItem == null)
			{
				return null;
			}
			return _configItem.MethodIds;
		}
	}

	public bool IsDirty
	{
		get
		{
			return _isDirty;
		}
		set
		{
			_isDirty = value;
			if (_parentWrapper != null)
			{
				_parentWrapper.IsDirty |= value;
			}
		}
	}

	public ServerId ServerId => _serverId;

	public string DisplayName
	{
		get
		{
			if (!IsValid)
			{
				return "";
			}
			return _configItem.DisplayName;
		}
		set
		{
			if (IsValid)
			{
				_configItem.DisplayName = value;
				IsDirty = true;
			}
		}
	}

	public string Path
	{
		get
		{
			if (!IsValid)
			{
				return "";
			}
			return _configItem.Path;
		}
	}

	public Property[] PropertyArray
	{
		get
		{
			if (!IsValid || _configItem.Properties == null)
			{
				return Array.Empty<Property>();
			}
			return _configItem.Properties;
		}
	}

	public Guid Id
	{
		get
		{
			if (!IsValid)
			{
				return Guid.Empty;
			}
			string text = GetPropertyValue("Id");
			if (string.IsNullOrEmpty(text))
			{
				int num = _configItem.Path.IndexOf('[');
				int num2 = _configItem.Path.IndexOf(']', num);
				if (num < 0 || num >= num2)
				{
					return Guid.Empty;
				}
				if (num2 > num)
				{
					text = _configItem.Path.Substring(num + 1, num2 - num - 1);
				}
			}
			if (Guid.TryParse(text, out var result))
			{
				return result;
			}
			return Guid.Empty;
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public ConfigurationApiItemWrapper()
	{
	}

	public ConfigurationApiItemWrapper(ServerId serverId, ConfigurationItem configItem, ConfigurationApiItemWrapper parentWrapper)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId", "serverId is null, cannot create ConfigurationApiItemWrapper");
		}
		_serverId = serverId;
		_configItem = configItem;
		_parentWrapper = parentWrapper;
		string itemType = _configItem.ItemType;
		if (itemType == ItemTypes_InvokeInfo)
		{
			itemType = ConfigurationItemPath.CreatePath(_configItem.Path).ItemType;
		}
		_configurationService = ConfigurationApiUtil.GetConfiguration(_serverId).ConfigurationApiManager.GetConfigurationClient(_serverId, itemType);
	}

	public ConfigurationApiItemWrapper(ServerId serverId, ConfigurationItem configItem)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId", "serverId is null, cannot create ConfigurationApiItemWrapper");
		}
		if (configItem == null)
		{
			throw new ArgumentNullException("configItem", "configItem is null, cannot create ConfigurationApiItemWrapper");
		}
		_serverId = serverId;
		_configItem = configItem;
		_configurationService = ConfigurationApiUtil.GetConfiguration(_serverId).ConfigurationApiManager.GetConfigurationClient(_serverId, _configItem.ItemType);
	}

	public ConfigurationApiItemWrapper(ServerId serverId, string path)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId", "serverId is null, cannot create ConfigurationApiItemWrapper");
		}
		if (string.IsNullOrEmpty(path))
		{
			throw new ArgumentNullException("path");
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(path);
		_serverId = serverId;
		_configurationService = ConfigurationApiUtil.GetConfiguration(_serverId).ConfigurationApiManager.GetConfigurationClient(_serverId, configurationItemPath.ItemType);
		if (_serverId.ServerType != "XPCO")
		{
			_configItem = null;
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Server type:{0} does not support ConfigurationAPI", _serverId.ServerType));
		}
		try
		{
			_configItem = _configurationService.GetItem(path);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "ConfigurationApiItemWrapper", "ConfigAPI GetItem(): " + ex.GetType()?.ToString() + ", " + ex.Message);
			_configItem = null;
			throw;
		}
	}

	public string GetPropertyValue(string key)
	{
		if (!IsValid)
		{
			return null;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			Property[] properties = _configItem.Properties;
			foreach (Property property in properties)
			{
				if (string.Compare(property.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return property.Value;
				}
			}
		}
		return null;
	}

	public ObservableCollection<string> GetPropertyArrayString(string key)
	{
		if (!IsValid)
		{
			return null;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			Property property = _configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
			if (property != null)
			{
				return new ObservableCollection<string>(property.ValueArray);
			}
		}
		return null;
	}

	public Dictionary<string, string> GetPropertyDictionaryString(string key)
	{
		if (!IsValid)
		{
			return null;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Property property = _configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
			if (property != null && property.ValueArray != null)
			{
				string[] valueArray = property.ValueArray;
				for (int num = 0; num < valueArray.Length; num++)
				{
					string[] array = valueArray[num].Split(';');
					if (array.Length != 2)
					{
						throw new ArgumentException("Dictionary array values not correct for: " + key);
					}
					dictionary.Add(array[0], array[1]);
				}
			}
			return dictionary;
		}
		return null;
	}

	public void SetProperty(string key, ObservableCollection<string> strings)
	{
		if (!IsValid)
		{
			return;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			(_configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0) ?? throw new ArgumentException("You must provide an existing key", "key")).ValueArray = strings.ToArray();
			IsDirty = true;
		}
	}

	public void SetProperty(string key, Collection<string> strings)
	{
		if (!IsValid)
		{
			return;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			(_configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0) ?? throw new ArgumentException("You must provide an existing key", "key")).ValueArray = strings.ToArray();
			IsDirty = true;
		}
	}

	public void SetProperty(string key, ObservableCollection<int> ints)
	{
		if (!IsValid)
		{
			return;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties == null)
		{
			return;
		}
		Property property = _configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
		if (property == null)
		{
			throw new ArgumentException("You must provide an existing key", "key");
		}
		List<string> list = new List<string>();
		foreach (int @int in ints)
		{
			list.Add(@int.ToString(CultureInfo.InvariantCulture));
		}
		property.ValueArray = list.ToArray();
		IsDirty = true;
	}

	public void SetProperty(string key, Collection<int> ints)
	{
		if (!IsValid)
		{
			return;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties == null)
		{
			return;
		}
		Property property = _configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
		if (property == null)
		{
			throw new ArgumentException("You must provide an exsisting key", "key");
		}
		List<string> list = new List<string>();
		foreach (int @int in ints)
		{
			list.Add(@int.ToString(CultureInfo.InvariantCulture));
		}
		property.ValueArray = list.ToArray();
		IsDirty = true;
	}

	public ObservableCollection<int> GetPropertyArrayInt(string key)
	{
		if (!IsValid)
		{
			return null;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			Property obj = _configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0) ?? throw new ArgumentException("You must provide an exsisting key", "key");
			ObservableCollection<int> observableCollection = new ObservableCollection<int>();
			string[] valueArray = obj.ValueArray;
			for (int num = 0; num < valueArray.Length; num++)
			{
				if (int.TryParse(valueArray[num], out var result))
				{
					observableCollection.Add(result);
				}
				else
				{
					observableCollection.Add(0);
				}
			}
			return observableCollection;
		}
		return null;
	}

	public Dictionary<string, int> GetPropertyDictionaryInt(string key)
	{
		if (!IsValid)
		{
			return null;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Property property = _configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
			if (property != null)
			{
				string[] valueArray = property.ValueArray;
				for (int num = 0; num < valueArray.Length; num++)
				{
					string[] array = valueArray[num].Split(';');
					if (array.Length != 2)
					{
						throw new ArgumentException("Dictionary value not correct for: " + key);
					}
					if (int.TryParse(array[1], out var result))
					{
						dictionary.Add(array[0], result);
					}
					else
					{
						dictionary.Add(array[0], 0);
					}
				}
			}
			return dictionary;
		}
		return null;
	}

	public SecureString GetPropertySecureValue(string key)
	{
		if (!IsValid)
		{
			return null;
		}
		if (key == null)
		{
			throw new ArgumentNullException("key", "You must provide a valid key");
		}
		if (_configItem.Properties != null)
		{
			Property[] properties = _configItem.Properties;
			foreach (Property property in properties)
			{
				if (string.Compare(property.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return property.SecureValue;
				}
			}
		}
		return null;
	}

	public bool GetPropertyValueBool(string key)
	{
		if (!IsValid)
		{
			return false;
		}
		if (bool.TryParse(GetPropertyValue(key), out var result))
		{
			return result;
		}
		return false;
	}

	public ConfigurationItem[] Children()
	{
		if (!IsValid)
		{
			return Array.Empty<ConfigurationItem>();
		}
		if (!_configItem.ChildrenFilled)
		{
			try
			{
				if (HasChildItems(_configItem.ItemType))
				{
					_configItem = _configurationService.GetItem(_configItem.Path);
				}
				if (!_configItem.ChildrenFilled)
				{
					_configItem.Children = _configurationService.GetChildItems(_configItem.Path);
					_configItem.ChildrenFilled = true;
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
				if (ex2 == ex)
				{
					throw;
				}
				throw ex2;
			}
		}
		return _configItem.Children;
	}

	public void FillChildren(string[] itemTypes, ItemFilter[] filters)
	{
		if (!IsValid || _configItem.ChildrenFilled)
		{
			return;
		}
		try
		{
			if (_configurationServiceConfigApi == null)
			{
				_configurationServiceConfigApi = ConfigurationApiUtil.GetConfiguration(_serverId).ConfigurationApiManager.GetConfigurationClient(_serverId, "ConfigAPI");
			}
			_configItem.Children = _configurationServiceConfigApi.GetChildItemsHierarchy(_configItem.Path, itemTypes, filters);
			_configItem.ChildrenFilled = true;
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public void ClearChildrenCache()
	{
		if (IsValid)
		{
			_configItem.Children = Array.Empty<ConfigurationItem>();
			_configItem.ChildrenFilled = false;
			(_configurationService as RestApiClientDecorator)?.ClearCache();
		}
	}

	public ConfigurationApiItemWrapper ExecuteMethod(string methodId, Property[] parameters = null)
	{
		if (!IsValid)
		{
			return null;
		}
		try
		{
			if (_configItem.ItemType == ItemTypes_InvokeResult && parameters == null)
			{
				EnvironmentManager.Instance.Log(error: false, "ItemWrapper.ExecuteMethod", "Ignore 2nd execute on an InvokeResult for method: " + methodId);
				return this;
			}
			ConfigurationItem configurationItem;
			if (parameters != null && parameters.Any())
			{
				configurationItem = _configItem.CloneWithNewProperties(parameters);
				configurationItem.ItemType = ItemTypes_InvokeInfo;
			}
			else
			{
				configurationItem = _configItem.CloneWithOnlyChildItemChildren();
			}
			ConfigurationItem configurationItem2 = _configurationService.InvokeMethod(configurationItem, methodId);
			if (configurationItem2 == null)
			{
				return null;
			}
			ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(_serverId, configurationItem2);
			configurationApiItemWrapper.OriginalWrapperFromExecuteMethod = this;
			if (ModifiesCache(methodId) && (configurationApiItemWrapper == null || configurationApiItemWrapper.ConfigurationItem == null || (configurationApiItemWrapper.ConfigurationItem.ItemType == ItemTypes_InvokeResult && configurationApiItemWrapper.GetPropertyValue(InvokeInfoProperty_State) == InvokeInfoStates_Success)))
			{
				OriginalWrapperFromExecuteMethod?.ClearChildrenCache();
			}
			return configurationApiItemWrapper;
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	private static bool ModifiesCache(string methodId)
	{
		string[] modifyingCacheMethods = _modifyingCacheMethods;
		foreach (string value in modifyingCacheMethods)
		{
			if (methodId.StartsWith(value, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	public ConfigurationItem GetItem(string path)
	{
		try
		{
			return _configurationService.GetItem(path);
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public ValidateResult ValidateItem()
	{
		try
		{
			ValidateResult validateResult = _configurationService.ValidateItem(_configItem);
			if (validateResult.ResultItem != null && validateResult.ResultItem.ItemType == _configItem.ItemType)
			{
				_configItem = validateResult.ResultItem;
			}
			return validateResult;
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public void SaveItem(ConfigurationItem item)
	{
		try
		{
			_configurationService.SetItem(item);
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public string GetProperty(string key)
	{
		if (_configItem.Properties != null)
		{
			string value = "/" + key + "/";
			Property[] properties = _configItem.Properties;
			foreach (Property property in properties)
			{
				if (string.Compare(property.Key, key, StringComparison.OrdinalIgnoreCase) == 0 || property.Key.IndexOf(value, StringComparison.OrdinalIgnoreCase) > 0)
				{
					return property.Value;
				}
			}
		}
		return null;
	}

	public List<string> GetPropertyKeys()
	{
		List<string> list = new List<string>();
		if (_configItem.Properties != null)
		{
			Property[] properties = _configItem.Properties;
			foreach (Property property in properties)
			{
				list.Add(property.Key);
			}
		}
		return list;
	}

	public void SetPropertyValue(string keyOrKeyFullName, string newValue)
	{
		SetProperty(keyOrKeyFullName, newValue);
	}

	public void SetPropertyValue(string keyOrKeyFullName, SecureString newValue)
	{
		SetProperty(keyOrKeyFullName, newValue);
	}

	public void SetPropertyArrayString(string keyOrKeyFullName, ObservableCollection<string> newValue)
	{
		SetProperty(keyOrKeyFullName, newValue);
	}

	public void SetProperty(string keyOrKeyFullName, string newValue)
	{
		if (_configItem.Properties == null)
		{
			_configItem.Properties = Array.Empty<Property>();
		}
		bool flag = false;
		string value = "/" + keyOrKeyFullName + "/";
		Property[] properties = _configItem.Properties;
		foreach (Property property in properties)
		{
			if (string.Compare(property.Key, keyOrKeyFullName, StringComparison.OrdinalIgnoreCase) == 0 || property.Key.IndexOf(value, StringComparison.OrdinalIgnoreCase) > 0)
			{
				flag = true;
				property.Value = newValue;
				IsDirty = true;
				break;
			}
		}
		if (!flag && newValue != null)
		{
			List<Property> list = new List<Property>(_configItem.Properties);
			list.Add(new Property
			{
				Key = keyOrKeyFullName,
				Value = newValue
			});
			_configItem.Properties = list.ToArray();
			IsDirty = true;
		}
	}

	public void SetProperty(string keyOrKeyFullName, SecureString newValue)
	{
		if (_configItem.Properties == null)
		{
			_configItem.Properties = Array.Empty<Property>();
		}
		bool flag = false;
		string strB = "/" + keyOrKeyFullName + "/";
		Property[] properties = _configItem.Properties;
		foreach (Property property in properties)
		{
			if (string.Compare(property.Key, keyOrKeyFullName, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(property.Key, strB, StringComparison.OrdinalIgnoreCase) == 0)
			{
				flag = true;
				if (newValue != null)
				{
					property.SecureValue = newValue;
					IsDirty = true;
				}
				else
				{
					RemoveProperty(_configItem, keyOrKeyFullName);
					IsDirty = true;
				}
				break;
			}
		}
		if (!flag && newValue != null)
		{
			List<Property> list = new List<Property>(_configItem.Properties);
			list.Add(new Property
			{
				Key = keyOrKeyFullName,
				SecureValue = newValue
			});
			_configItem.Properties = list.ToArray();
			IsDirty = true;
		}
	}

	public void ClearProperties()
	{
		_configItem.Properties = new Property[0];
		IsDirty = true;
	}

	public void RemoveProperty(string keyOrKeyFullName)
	{
		RemoveProperty(_configItem, keyOrKeyFullName);
		IsDirty = true;
	}

	private static void RemoveProperty(ConfigurationItem item, string keyOrKeyFullName)
	{
		List<Property> list = new List<Property>();
		string strB = "/" + keyOrKeyFullName + "/";
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (string.Compare(property.Key, keyOrKeyFullName, StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(property.Key, strB, StringComparison.OrdinalIgnoreCase) != 0)
			{
				list.Add(property);
			}
		}
		item.Properties = list.ToArray();
	}

	public List<ValueTypeInfo> GetValueTypeInfoList(string propertyKey)
	{
		if (_configItem.Properties == null)
		{
			return new List<ValueTypeInfo>();
		}
		Property property = _configItem.Properties.FirstOrDefault((Property p) => string.Compare(p.Key, propertyKey, StringComparison.OrdinalIgnoreCase) == 0);
		if (property == null)
		{
			return new List<ValueTypeInfo>();
		}
		if (property.ValueTypeInfos == null)
		{
			return new List<ValueTypeInfo>();
		}
		return new List<ValueTypeInfo>(property.ValueTypeInfos);
	}

	public void Save()
	{
		if (!IsDirty)
		{
			return;
		}
		try
		{
			ValidateResult validateResult = _configurationService.SetItem(_configItem);
			if (!validateResult.ValidatedOk)
			{
				throw new ValidateResultException(validateResult);
			}
			if (validateResult.ResultItem != null)
			{
				if (validateResult.ResultItem.ItemType == _configItem.ItemType)
				{
					_configItem = validateResult.ResultItem;
				}
				else if (validateResult.ResultItem.ItemType == "_internalLastModifiedOnlyItem")
				{
					Property property = validateResult.ResultItem.Properties.FirstOrDefault((Property p) => p.Key == "LastModified");
					if (property != null)
					{
						SetProperty("LastModified", property.Value);
					}
				}
			}
			IsDirty = false;
		}
		catch (Exception ex)
		{
			Exception ex2 = ConfigurationApiUtil.ExceptionHandler(ex);
			if (ex2 == ex)
			{
				throw;
			}
			throw ex2;
		}
	}

	public Dictionary<string, string> GetEnumValues(string key)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (_configItem.Properties != null)
		{
			Property[] properties = _configItem.Properties;
			foreach (Property property in properties)
			{
				if (string.Compare(property.Key, key, StringComparison.OrdinalIgnoreCase) != 0)
				{
					continue;
				}
				if (property.ValueTypeInfos == null)
				{
					break;
				}
				ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
				foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
				{
					if (!dictionary.ContainsKey(valueTypeInfo.Name))
					{
						dictionary.Add(valueTypeInfo.Name, valueTypeInfo.Value);
					}
				}
				break;
			}
		}
		return dictionary;
	}

	public Dictionary<string, string> GetEnumKeys(string key)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (_configItem.Properties != null)
		{
			Property[] properties = _configItem.Properties;
			foreach (Property property in properties)
			{
				if (!(property.Key == key))
				{
					continue;
				}
				if (property.ValueTypeInfos == null)
				{
					break;
				}
				ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
				foreach (ValueTypeInfo valueTypeInfo in valueTypeInfos)
				{
					if (!dictionary.ContainsKey(valueTypeInfo.Value))
					{
						dictionary.Add(valueTypeInfo.Value, valueTypeInfo.Name);
					}
				}
				break;
			}
		}
		return dictionary;
	}

	private static bool HasChildItems(string itemTypeOfMethodOwner)
	{
		return _hasChildItems.Contains(itemTypeOfMethodOwner);
	}
}
