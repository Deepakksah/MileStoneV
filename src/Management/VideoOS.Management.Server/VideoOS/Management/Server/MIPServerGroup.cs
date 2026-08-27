using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class MIPServerGroup
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Path { get; set; }

	public string Kind { get; set; }

	public string PluginId { get; set; }

	public string CustomData { get; set; }

	public string ParentId { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string RelationIds { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static MIPServerGroup[] QueryServerGroupsByParentGroupId(IGeminiRequestContext requestContext, ServerGroup.EnumStructureType structureType, string parentGroupId)
	{
		string groupStructureType = ServerGroup.ConvertFromEnumStructureTypeToStringStructureType(structureType);
		MIPServerGroup[] array = null;
		using (IMIPServerGroupComponent iMIPServerGroupComponent = requestContext.CreateMIPServerGroupComponent())
		{
			array = iMIPServerGroupComponent.QueryMIPServerGroupsByParentGroupId(groupStructureType, parentGroupId);
		}
		if (ServerGroup.HasPermissions(groupStructureType))
		{
			array = FilterServerGroupsBasedOnPermissions(requestContext, array);
		}
		return array;
	}

	public static MIPServerGroup QueryServerGroupById(IGeminiRequestContext requestContext, string serverGroupId)
	{
		MIPServerGroup mIPServerGroup = null;
		using IMIPServerGroupComponent iMIPServerGroupComponent = requestContext.CreateMIPServerGroupComponent();
		return iMIPServerGroupComponent.QueryMIPServerGroupById(serverGroupId);
	}

	public static MIPServerGroup[] QueryServerGroupsByIds(IGeminiRequestContext requestContext, string[] serverGroupIds)
	{
		MIPServerGroup[] array = null;
		using IMIPServerGroupComponent iMIPServerGroupComponent = requestContext.CreateMIPServerGroupComponent();
		return iMIPServerGroupComponent.QueryMIPServerGroupsByIds(serverGroupIds);
	}

	public static MIPServerGroup CreateServerGroup(IGeminiRequestContext requestContext, MIPServerGroup serverGroup)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		serverGroup.Id = GeminiResourceComponent.GetGuid(serverGroup.Id);
		using (IMIPServerGroupComponent iMIPServerGroupComponent = requestContext.CreateMIPServerGroupComponent())
		{
			iMIPServerGroupComponent.CreateMIPServerGroup(serverGroup, requestContext.AuthenticatedUserSid, "MIPOBJECT_GROUP");
			serverGroup = FindServerGroup(requestContext, iMIPServerGroupComponent, serverGroup.ParentId, serverGroup.Id);
		}
		Integration.TryNotifyServerAsync();
		return serverGroup;
	}

	public static MIPServerGroup UpdateServerGroup(IGeminiRequestContext requestContext, MIPServerGroup serverGroup)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		serverGroup.ParentId = ServerGroup.GetParentServerGroupId(requestContext, serverGroup.Id);
		using (IMIPServerGroupComponent iMIPServerGroupComponent = requestContext.CreateMIPServerGroupComponent())
		{
			iMIPServerGroupComponent.UpdateServerGroup(serverGroup.Id, serverGroup.Name, serverGroup.Description, serverGroup.CustomData, serverGroup.CustomProperties);
			serverGroup = FindServerGroup(requestContext, null, serverGroup.ParentId, serverGroup.Id);
		}
		Integration.TryNotifyServerAsync();
		return serverGroup;
	}

	public static void DeleteServerGroup(IGeminiRequestContext requestContext, string serverGroupId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using (IMIPServerGroupComponent iMIPServerGroupComponent = requestContext.CreateMIPServerGroupComponent())
		{
			iMIPServerGroupComponent.DeleteMIPServerGroup(serverGroupId);
		}
		Integration.TryNotifyServerAsync();
	}

	public static MIPServerGroup FilterServerGroupBasedOnPermissions(IGeminiRequestContext requestContext, MIPServerGroup serverGroup)
	{
		return serverGroup;
	}

	public static MIPServerGroup[] FilterServerGroupsBasedOnPermissions(IGeminiRequestContext requestContext, MIPServerGroup[] serverGroups)
	{
		List<MIPServerGroup> list = new List<MIPServerGroup>();
		foreach (MIPServerGroup mIPServerGroup in serverGroups)
		{
			if (FilterServerGroupBasedOnPermissions(requestContext, mIPServerGroup) != null)
			{
				list.Add(mIPServerGroup);
			}
		}
		return list.ToArray();
	}

	private static MIPServerGroup FindServerGroup(IGeminiRequestContext requestContext, IMIPServerGroupComponent serverGroupComponent, string parentGroupId, string serverGroupId)
	{
		bool flag = false;
		try
		{
			if (serverGroupComponent == null)
			{
				serverGroupComponent = requestContext.CreateMIPServerGroupComponent();
				flag = true;
			}
			return serverGroupComponent.QueryMIPServerGroupByParentGroupId(parentGroupId, serverGroupId);
		}
		finally
		{
			if (flag)
			{
				serverGroupComponent.Dispose();
			}
		}
	}
}
