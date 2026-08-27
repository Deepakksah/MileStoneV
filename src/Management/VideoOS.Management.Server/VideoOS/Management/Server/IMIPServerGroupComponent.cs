using System;

namespace VideoOS.Management.Server;

public interface IMIPServerGroupComponent : IDisposable
{
	void CreateMIPServerGroup(MIPServerGroup serverGroup, string userSid, string structureType);

	MIPServerGroup QueryMIPServerGroupById(string serverGroupId);

	MIPServerGroup QueryMIPServerGroupByParentGroupId(string parentGroupId, string serverGroupId);

	MIPServerGroup[] QueryMIPServerGroupsByIds(string[] serverGroupIds);

	MIPServerGroup[] QueryMIPServerGroupsByParentGroupId(string groupStructureType, string parentGroupId);

	void UpdateServerGroup(string id, string name, string description, string customData, CustomProperty[] customProperties);

	void DeleteMIPServerGroup(string serverGroupId);
}
