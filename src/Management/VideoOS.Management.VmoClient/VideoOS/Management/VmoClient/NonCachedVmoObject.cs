using System;
using System.Collections.Generic;
using System.Reflection;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class NonCachedVmoObject
{
	public Dictionary<string, object> _objectPropertiesBase;

	public VmoPropertyCollection _objectProperties;

	private readonly IVmoRepositories _repositories;

	private CustomPropertyCollection _customProperties;

	public IVmoRepositories Repositories => _repositories;

	public string InternalId { get; set; }

	public VmoPropertyCollection ObjectProperties
	{
		get
		{
			return _objectProperties;
		}
		set
		{
			_objectProperties = value;
			_objectPropertiesBase = value.ToDictionary();
			PropertiesLoaded();
		}
	}

	public bool IsCreated => !string.IsNullOrEmpty(InternalId);

	public bool IsDeleted { get; protected set; }

	public ServerVersion ServerVersion => Repositories.ServerVersion;

	public CustomPropertyCollection CustomProperties
	{
		get
		{
			if (!IsCustomPropertiesSupported)
			{
				throw new NotSupportedException();
			}
			return _customProperties;
		}
	}

	public virtual bool IsCustomPropertiesSupported => true;

	public NonCachedVmoObject(VmoPropertyCollection objectProperties, IVmoRepositories repositories, string internalId)
	{
		_objectProperties = objectProperties;
		_repositories = repositories;
		_objectPropertiesBase = objectProperties.ToDictionary();
		CreateCustomPropertyCollection();
		InternalId = internalId;
	}

	protected virtual void PropertiesLoaded()
	{
	}

	public void Undo()
	{
		_objectProperties.Clear();
		_objectProperties.ImportFromDictionary(_objectPropertiesBase);
		PropertiesLoaded();
	}

	public bool IsDirty()
	{
		if (_objectProperties.Count != _objectPropertiesBase.Count)
		{
			return true;
		}
		EqualityComparer<object> equalityComparer = EqualityComparer<object>.Default;
		foreach (KeyValuePair<string, object> objectProperty in _objectProperties)
		{
			if (!_objectPropertiesBase.TryGetValue(objectProperty.Key, out var value))
			{
				return true;
			}
			if (!equalityComparer.Equals(objectProperty.Value, value))
			{
				return true;
			}
		}
		return false;
	}

	public void CreateObject(NonCachedVmoObject nonCachedVmoObject)
	{
		VMOWebserviceObject vMOWebserviceObject = _repositories.ServerCommunication.CreateObject(properties: nonCachedVmoObject.ObjectProperties, customProperties: nonCachedVmoObject.GetCustomProperties(), type: nonCachedVmoObject.GetType());
		if (vMOWebserviceObject == null)
		{
			throw new InvalidOperationException("The object was not created");
		}
		nonCachedVmoObject.ObjectProperties = vMOWebserviceObject.Properties;
		nonCachedVmoObject.InternalId = vMOWebserviceObject.Id;
	}

	public void ThrowIfCheckVersionFail(ServerVersion requiredVersion)
	{
		if (!Repositories.CheckVersion(requiredVersion))
		{
			throw new NotSupportedException();
		}
	}

	public T GetVMOObjectById<T>(string Id) where T : VmoObject
	{
		return Repositories.GetGenericRepository<T>().GetObjectById<T>(Id, refreshCachedObject: true);
	}

	public T GetObjectById<T>(string Id) where T : NonCachedVmoObject
	{
		return GetObjectById<T>(Id, Repositories);
	}

	public static T GetObjectById<T>(string Id, IVmoRepositories iVmoRepositories) where T : NonCachedVmoObject
	{
		T result = null;
		VMOWebserviceObject objectById = iVmoRepositories.ServerCommunication.GetObjectById(typeof(T), Id);
		if (objectById != null)
		{
			return MapObject<T>(objectById, iVmoRepositories);
		}
		return result;
	}

	public static V MapObject<V>(VMOWebserviceObject webObj, IVmoRepositories iServer) where V : NonCachedVmoObject
	{
		object[] args = new object[3] { webObj.Id, webObj.Properties, iServer };
		return (V)Activator.CreateInstance(webObj.Type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null);
	}

	public T GetPropertyValue<T>(string key)
	{
		return ObjectProperties.GetPropertyValue<T>(key);
	}

	public void SetPropertyValue<T>(string key, T value)
	{
		ObjectProperties.SetPropertyValue(key, value);
	}

	protected void CreateCustomPropertyCollection()
	{
		_customProperties = new CustomPropertyCollection(Repositories, string.Empty);
	}

	public CustomPropertyCollection GetCustomProperties()
	{
		return _customProperties;
	}

	public void SetCustomProperties(CustomPropertyCollection customProperties)
	{
		_customProperties = customProperties;
	}

	public T GetNestedPropertyValue<T>(string nestedPropertiesCollectionName, string key)
	{
		string key2 = NestedPropertyHelper.FormatNestedPropertyName(nestedPropertiesCollectionName);
		VmoPropertyCollection propertyValue = ObjectProperties.GetPropertyValue<VmoPropertyCollection>(key2);
		if (propertyValue == null)
		{
			return default(T);
		}
		return propertyValue.GetPropertyValue<T>(key);
	}

	public void SetNestedPropertyValue<T>(string nestedPropertiesCollectionName, string key, T value)
	{
		string key2 = NestedPropertyHelper.FormatNestedPropertyName(nestedPropertiesCollectionName);
		VmoPropertyCollection vmoPropertyCollection = ObjectProperties.GetPropertyValue<VmoPropertyCollection>(key2);
		if (vmoPropertyCollection == null)
		{
			vmoPropertyCollection = new VmoPropertyCollection();
		}
		vmoPropertyCollection.SetPropertyValue(key, value);
		ObjectProperties.SetPropertyValue(key2, vmoPropertyCollection);
	}

	public void CreateObject()
	{
		CreateObject(this);
		PropertiesLoaded();
	}

	public void ThrowIfNotCreated()
	{
		if (!IsCreated)
		{
			throw new InvalidOperationException();
		}
	}

	public void ThrowIfDeleted()
	{
		if (IsDeleted)
		{
			throw new InvalidOperationException();
		}
	}

	public void ThrowIfNotCreatedOrDeleted()
	{
		ThrowIfNotCreated();
		ThrowIfDeleted();
	}
}
