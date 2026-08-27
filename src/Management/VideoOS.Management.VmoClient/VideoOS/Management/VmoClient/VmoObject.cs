using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public abstract class VmoObject : VmoObjectSecurity
{
	private bool _hasCustomPropertyId;

	public Dictionary<string, object> _objectPropertiesBase;

	public VmoPropertyCollection _objectProperties;

	private Dictionary<Type, VmoCollectionBase> _childCollections = new Dictionary<Type, VmoCollectionBase>();

	private CustomPropertyCollection _customProperties;

	public override string InternalId
	{
		get
		{
			return base.InternalId;
		}
		set
		{
			base.InternalId = value;
			if (!_hasCustomPropertyId)
			{
				_customPropertyId = value;
			}
			CreateCustomPropertyCollection();
		}
	}

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
			CreateCustomPropertyCollection();
			PropertiesLoaded();
		}
	}

	public bool IsDirty
	{
		get
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
			if (GetCustomProperties().DirtyCustomPropertyArray().Length != 0)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsCreated => !string.IsNullOrEmpty(InternalId);

	public bool IsDeleted { get; public set; }

	public bool Exists
	{
		get
		{
			if (IsCreated)
			{
				return !IsDeleted;
			}
			return false;
		}
	}

	public VmoRelationCollection Relations { get; set; }

	public GenericRepositoryBase Repository => base.Repositories.GetGenericRepositoryBase(GetType());

	private string _customPropertyId { get; set; }

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

	public ServerVersion ServerVersion => base.Repositories.ServerVersion;

	public event EventHandler Deleted;

	public VmoObject(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relationProperties, IVmoRepositories repositories, string customPropertyId)
		: base(repositories, internalId)
	{
		_hasCustomPropertyId = true;
		_objectProperties = objectProperties ?? throw new ArgumentNullException("objectProperties");
		_objectPropertiesBase = objectProperties.ToDictionary();
		_customPropertyId = customPropertyId;
		Relations = relationProperties ?? throw new ArgumentNullException("relationProperties");
		CreateCustomPropertyCollection();
	}

	public VmoObject(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relationProperties, IVmoRepositories repositories)
		: base(repositories, internalId)
	{
		_hasCustomPropertyId = false;
		_objectProperties = objectProperties ?? throw new ArgumentNullException("objectProperties");
		_objectPropertiesBase = objectProperties.ToDictionary();
		_customPropertyId = internalId;
		Relations = relationProperties;
		CreateCustomPropertyCollection();
	}

	public void Undo()
	{
		_objectProperties.Clear();
		_objectProperties.ImportFromDictionary(_objectPropertiesBase);
		CreateCustomPropertyCollection();
		PropertiesLoaded();
	}

	protected virtual void PropertiesLoaded()
	{
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
		if (!Exists)
		{
			throw new InvalidOperationException();
		}
	}

	public virtual void OnDelete(EventArgs e)
	{
		IsDeleted = true;
		this.Deleted?.Invoke(this, e);
	}

	public virtual void AddNotification(VmoObject obj)
	{
		Type type = obj.GetType();
		foreach (VmoCollectionBase value in _childCollections.Values)
		{
			if (type == value.ContentType || type.IsSubclassOf(value.ContentType))
			{
				value.AddVmoObject(obj);
			}
		}
	}

	public virtual void RemoveNotification(VmoObject obj)
	{
		Type type = obj.GetType();
		foreach (VmoCollectionBase value in _childCollections.Values)
		{
			if (type == value.ContentType || type.IsSubclassOf(value.ContentType))
			{
				value.RemoveVmoObject(obj);
			}
		}
	}

	public void RegisterCollection(Type type, VmoCollectionBase col)
	{
		_childCollections.Add(type, col);
	}

	public bool TryGetPropertyValue<T>(string key, out T value)
	{
		return ObjectProperties.TryGetPropertyValue<T>(key, out value);
	}

	public T GetPropertyValue<T>(string key)
	{
		return ObjectProperties.GetPropertyValue<T>(key);
	}

	public void SetPropertyValue<T>(string key, T value)
	{
		ObjectProperties.SetPropertyValue(key, value);
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

	protected T GetObjectById<T>(string id) where T : VmoObject
	{
		return base.Repositories.GetGenericRepositoryBase(typeof(T)).GetObjectById<T>(id, refreshCachedObject: false);
	}

	protected void CreateCustomPropertyCollection()
	{
		_customProperties = new CustomPropertyCollection(base.Repositories, _customPropertyId);
	}

	public CustomPropertyCollection GetCustomProperties()
	{
		return _customProperties;
	}

	public void SetCustomProperties(CustomPropertyCollection customProperties)
	{
		_customProperties = customProperties;
	}

	public void ThrowIfCheckVersionFail(ServerVersion requiredVersion)
	{
		if (!base.Repositories.CheckVersion(requiredVersion))
		{
			throw new NotSupportedException();
		}
	}
}
