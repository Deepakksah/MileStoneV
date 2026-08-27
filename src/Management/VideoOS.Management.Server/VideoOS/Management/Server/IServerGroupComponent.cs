using System;

namespace VideoOS.Management.Server;

public interface IServerGroupComponent : IDisposable
{
	void CreateServerGroup(ServerGroup serverGroup, string structureType, string userSid);

	ServerGroup QueryServerGroupById(string serverGroupId);

	ServerGroup QueryServerGroupByParentGroupId(string parentGroupId, string serverGroupId);

	ServerGroup[] QueryServerGroupsByIds(string[] serverGroupIds);

	ServerGroup[] QueryServerGroupsByParentGroupId(string groupStructureType, string parentGroupId);

	ServerGroup[] QueryServerGroupHierarchyById(string groupStructureType, string parentGroupId);

	ServerGroup[] QueryServerGroupAllNoSecurity(string groupStructureType);

	ServerGroup QueryPrivateViewGroup(string userSid, out bool created);

	void UpdateServerGroup(string id, string name, string description, CustomProperty[] customProperties);

	void DeleteServerGroup(string serverGroupId);

	string FindRootViewGroupId(string viewGroupId);

	string GetGroupStructure(string serverGroupId);

	string GetIdFromName(string parentId, string name);

	string GetParentServerGroupId(string serverGroupId);

	string GetServerGroupIdFromItem(string objectId);

	string GetItemFromServerGroupId(string serverGroupId);

	string QueryServerGroupRoot(string structureType);
}
