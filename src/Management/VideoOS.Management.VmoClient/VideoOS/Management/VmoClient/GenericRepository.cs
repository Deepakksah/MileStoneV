using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace VideoOS.Management.VmoClient;

public class GenericRepository<V> : GenericRepositoryBase where V : VmoObject
{
	private readonly Dictionary<string, V> _cache;

	private IVmoRepositories _repositories;

	public Dictionary<string, V> Cache => _cache;

	private IServer ServerCommunication
	{
		[DebuggerStepThrough]
		get
		{
			return _repositories.ServerCommunication;
		}
	}

	public GenericRepository(IVmoRepositories repositories)
		: this(repositories, new Dictionary<string, V>(StringComparer.OrdinalIgnoreCase))
	{
	}

	public GenericRepository(IVmoRepositories repositories, Dictionary<string, V> cache)
	{
		_repositories = repositories;
		_cache = cache;
	}

	public V GetObjectById(string id, bool refreshCachedObject)
	{
		return GetObjectById(id, refreshCachedObject, typeof(V));
	}

	public V GetObjectById(string id, bool refreshCachedObject, Type type)
	{
		V value = null;
		bool flag = false;
		lock (_repositories.RepositoryLocker)
		{
			flag = _cache.TryGetValue(id, out value);
		}
		if (!flag || refreshCachedObject)
		{
			VMOWebserviceObject objectById = _repositories.ServerCommunication.GetObjectById(type, id);
			if (objectById != null)
			{
				return AddOrRefresh(objectById);
			}
		}
		return value;
	}

	public override T GetObjectById<T>(string id, bool refreshCachedObject)
	{
		return GetObjectById(id, refreshCachedObject, typeof(T)) as T;
	}

	public V[] GetObjectsByIds(string[] ids, bool refreshCachedObjects)
	{
		if (ids != null && ids.Length != 0)
		{
			string[] array;
			if (refreshCachedObjects)
			{
				array = ids;
			}
			else
			{
				lock (_repositories.RepositoryLocker)
				{
					array = ids.Where((string id) => !_cache.ContainsKey(id)).ToArray();
				}
			}
			if (array.Length != 0)
			{
				foreach (VMOWebserviceObject item in from o in _repositories.ServerCommunication.GetObjectsByIds(typeof(V), array)
					where refreshCachedObjects || !_cache.ContainsKey(o.Id)
					select o)
				{
					AddOrRefresh(item);
				}
			}
			lock (_repositories.RepositoryLocker)
			{
				List<V> list = new List<V>();
				foreach (string key in ids)
				{
					if (_cache.TryGetValue(key, out var value))
					{
						list.Add(value);
					}
				}
				return list.ToArray();
			}
		}
		return new List<V>().ToArray();
	}

	public override T[] GetObjectsByIds<T>(string[] ids, bool refreshCachedObject)
	{
		return GetObjectsByIds(ids, refreshCachedObject).OfType<T>().ToArray();
	}

	public override string[] GetObjectRelations(Type parentObjectType, string parentId, Type type)
	{
		return _repositories.ServerCommunication.GetObjectRelations(parentObjectType, parentId, type);
	}

	public override void RefreshObject(string id, bool updateRelations = false, bool updateRelatedObjects = false)
	{
		bool flag;
		V value;
		lock (_repositories.RepositoryLocker)
		{
			flag = _cache.TryGetValue(id, out value);
		}
		if (flag)
		{
			Type type = value.GetType();
			VMOWebserviceObject objectById = _repositories.ServerCommunication.GetObjectById(type, id);
			if (objectById != null)
			{
				RefreshObjectInternal(value, objectById, updateRelations, updateRelatedObjects);
				return;
			}
			CultureInfo culture = VmoClient.Environment.ResolveUICulture();
			throw new ObjectNotFoundException(VmoResourcesLookup.GetErrorMessage("VMO61032", culture));
		}
		CultureInfo culture2 = VmoClient.Environment.ResolveUICulture();
		throw new ObjectNotFoundException(VmoResourcesLookup.GetErrorMessage("VMO61032", culture2));
	}

	public void RefreshObjectInternal(VmoObject vmoObject, VMOWebserviceObject webObj, bool updateRelations = false, bool updateRelatedObjects = false)
	{
		vmoObject.ObjectProperties = webObj.Properties;
		if (updateRelations)
		{
			vmoObject.Relations = webObj.Relations;
			_repositories.Notification.UpdateRegistrations(vmoObject, updateRelatedObjects);
		}
	}

	public override void CreateObject(VmoObject vmoObject)
	{
		Type type = vmoObject.GetType();
		IServer serverCommunication = _repositories.ServerCommunication;
		VmoPropertyCollection objectProperties = vmoObject.ObjectProperties;
		CustomPropertyCollection customProperties = vmoObject.GetCustomProperties();
		VMOWebserviceObject vMOWebserviceObject = serverCommunication.CreateObject(type, objectProperties, customProperties);
		vmoObject.ObjectProperties = vMOWebserviceObject.Properties;
		vmoObject.Relations = vMOWebserviceObject.Relations;
		vmoObject.InternalId = vMOWebserviceObject.Id;
		AddToCache(vmoObject);
	}

	public override void UpdateObject(VmoObject vmoObject)
	{
		Type type = vmoObject.GetType();
		IServer serverCommunication = _repositories.ServerCommunication;
		string internalId = vmoObject.InternalId;
		VmoPropertyCollection objectProperties = vmoObject.ObjectProperties;
		CustomPropertyCollection customProperties = vmoObject.GetCustomProperties();
		VMOWebserviceObject webObj = serverCommunication.UpdateObject(type, internalId, objectProperties, customProperties);
		RefreshObjectInternal(vmoObject, webObj);
	}

	public void DeleteObject(string id)
	{
		_repositories.ServerCommunication.DeleteObject(typeof(V), id);
		RemoveFromCache(id);
	}

	public override void DeleteObject(VmoObject vmoObject)
	{
		IServer serverCommunication = _repositories.ServerCommunication;
		Type type = vmoObject.GetType();
		string internalId = vmoObject.InternalId;
		serverCommunication.DeleteObject(type, internalId);
		RemoveFromCache(internalId);
	}

	private V MapObject(VMOWebserviceObject webObj)
	{
		object[] args = new object[5] { webObj.Id, webObj.Properties, webObj.Relations, _repositories, false };
		return (V)Activator.CreateInstance(webObj.Type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null);
	}

	private V AddOrRefresh(VMOWebserviceObject webObj)
	{
		lock (_repositories.RepositoryLocker)
		{
			if (!_cache.TryGetValue(webObj.Id, out var value))
			{
				value = MapObject(webObj);
				AddToCache(value);
			}
			else
			{
				RefreshObjectInternal(value, webObj);
			}
			return value;
		}
	}

	public void AddToCache(VmoObject vmoObject)
	{
		lock (_repositories.RepositoryLocker)
		{
			vmoObject.IsDeleted = false;
			if (!_cache.ContainsKey(vmoObject.InternalId))
			{
				_cache.Add(vmoObject.InternalId, (V)vmoObject);
				_repositories.Notification.Register(vmoObject);
				vmoObject.Deleted += ObjectDeleted;
			}
		}
	}

	private void ObjectDeleted(object sender, EventArgs e)
	{
		RemoveFromCache(((VmoObject)sender).InternalId);
	}

	public void RemoveFromCache(string id)
	{
		lock (_repositories.RepositoryLocker)
		{
			if (_cache.ContainsKey(id))
			{
				VmoObject vmoObject = _cache[id];
				vmoObject.Deleted -= ObjectDeleted;
				_repositories.Notification.Unregister(vmoObject);
				_cache.Remove(id);
			}
		}
	}

	public V GetObjectFromCache(string id)
	{
		lock (_repositories.RepositoryLocker)
		{
			if (_cache.ContainsKey(id))
			{
				return _cache[id];
			}
			return null;
		}
	}

	public V[] GetAllObjectsFromCache()
	{
		lock (_repositories.RepositoryLocker)
		{
			return _cache.Values.ToArray();
		}
	}

	public override VmoObject GetVmoObjectFromCache(string id)
	{
		return GetObjectFromCache(id);
	}
}
