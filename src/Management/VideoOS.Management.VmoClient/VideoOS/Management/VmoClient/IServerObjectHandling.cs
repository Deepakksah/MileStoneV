using System;

namespace VideoOS.Management.VmoClient;

public interface IServerObjectHandling
{
	VMOWebserviceObject CreateObject(Type type, VmoPropertyCollection properties, CustomPropertyCollection customProperties);

	VMOWebserviceObject UpdateObject(Type type, string id, VmoPropertyCollection properties, CustomPropertyCollection customProperties);

	void DeleteObject(Type type, string id);

	VMOWebserviceObject GetObjectById(Type type, string id);

	VMOWebserviceObject[] GetObjectsByIds(Type type, string[] ids);

	string[] GetObjectRelations(Type parentType, string parentId, Type type);

	void AddGroupMembership(string parentServerGroupId, Type type, string id);

	void RemoveGroupMembership(string parentServerGroupId, Type type, string id);

	CustomProperty[] GetCustomProperties(string objectId);

	CustomProperty GetCustomProperty(string objectId, string key);

	void DeleteCustomProperty(string objectId, string key);

	void SaveCustomProperties(string objectId, CustomProperty[] customProperties);
}
