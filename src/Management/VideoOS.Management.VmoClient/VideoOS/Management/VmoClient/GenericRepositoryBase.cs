using System;

namespace VideoOS.Management.VmoClient;

public abstract class GenericRepositoryBase
{
	public abstract VmoObject GetVmoObjectFromCache(string id);

	public abstract T GetObjectById<T>(string id, bool refreshCachedObject) where T : VmoObject;

	public abstract T[] GetObjectsByIds<T>(string[] id, bool refreshCachedObject) where T : VmoObject;

	public abstract string[] GetObjectRelations(Type parentObjectType, string parentId, Type type);

	public abstract void RefreshObject(string internalId, bool updateRelations = false, bool updateRelatedObjects = false);

	public abstract void CreateObject(VmoObject vmoObject);

	public abstract void UpdateObject(VmoObject vmoObject);

	public abstract void DeleteObject(VmoObject vmoObject);
}
