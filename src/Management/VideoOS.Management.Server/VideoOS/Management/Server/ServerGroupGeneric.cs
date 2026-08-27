using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("PublicViewGroup")]
[EntityName("PrivateViewGroup")]
[EntityName("ViewLayoutGroup")]
[EntityName("FailoverServerGroup")]
[EntityName("ViewGroup")]
[EntityName("ServerGroup")]
public class ServerGroupGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		ServerGroup serverGroup = ServerGroup.QueryServerGroupById(requestContext, id);
		return ConvertToGenericObject2(requestContext, serverGroup);
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		ServerGroup[] serverGroups = ServerGroup.QueryServerGroupsByIds(requestContext, ids);
		return ConvertToGenericObject2s(requestContext, serverGroups);
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		parentId = ConvertManagementServerIdToTopServerId(requestContext, parentEntityName, entityName, parentId);
		ServerGroup.EnumStructureType structureType = ServerGroup.ConvertFromEntityNameToEnumStructureType(entityName);
		ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, structureType, parentId);
		List<string> list = new List<string>();
		ServerGroup[] array2 = array;
		foreach (ServerGroup serverGroup in array2)
		{
			if (!string.Equals(serverGroup.Id, "4BF8A8B0-54AB-4E82-994F-98D8E9584176", StringComparison.OrdinalIgnoreCase))
			{
				list.Add(serverGroup.Id);
			}
		}
		return list.ToArray();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		ServerGroup serverGroup = ConvertToServerGroup(properties, customProperties);
		if (IsManagementServerId(requestContext, serverGroup.ParentId))
		{
			serverGroup.ParentId = ConvertManagementServerIdToTopServerId(requestContext, "ManagementServer", entityName, serverGroup.ParentId);
		}
		serverGroup.StructureType = ServerGroup.ConvertFromEntityNameToEnumStructureType(entityName);
		if (string.Equals(entityName, "ViewLayoutGroup", StringComparison.OrdinalIgnoreCase))
		{
			throw new ServerGroupCreationException(serverGroup.Name, serverGroup.ParentId);
		}
		serverGroup = ServerGroup.CreateServerGroup(requestContext, serverGroup);
		return ConvertToGenericObject2(requestContext, serverGroup);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		ServerGroup serverGroup = ConvertToServerGroup(genericObject.Properties, customProperties);
		if (string.Equals(genericObject.EntityName, "ViewLayoutGroup", StringComparison.OrdinalIgnoreCase))
		{
			throw new ServerGroupUpdateException();
		}
		serverGroup = ServerGroup.UpdateServerGroup(requestContext, serverGroup);
		return ConvertToGenericObject2(requestContext, serverGroup);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		if (ServerGroup.QueryServerGroupById(requestContext, id).StructureType == ServerGroup.EnumStructureType.ViewLayoutGroup)
		{
			throw new ServerGroupDeleteException();
		}
		ServerGroup.DeleteServerGroup(requestContext, id);
	}

	private static string GetPrivateUserViewGroupId(IGeminiRequestContext requestContext)
	{
		ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.ViewGroup, "4BF8A8B0-54AB-4E82-994F-98D8E9584176");
		foreach (ServerGroup serverGroup in array)
		{
			if (string.Equals(serverGroup.Name, requestContext.UserDomainAccountName, StringComparison.OrdinalIgnoreCase))
			{
				return serverGroup.Id;
			}
		}
		return null;
	}

	public static bool IsManagementServerId(IGeminiRequestContext requestContext, string id)
	{
		Site site = Site.QueryHomeSiteNoSecurity(requestContext);
		if (string.Equals(id, site.Id, StringComparison.OrdinalIgnoreCase) || string.Equals(id, "909749AE-88C4-4780-A381-383D691E98FA", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static string ConvertManagementServerIdToTopServerId(IGeminiRequestContext requestContext, string parentEntityName, string entityName, string parentGroupId)
	{
		if (string.Equals(parentEntityName, "ManagementServer", StringComparison.OrdinalIgnoreCase))
		{
			if (string.Equals(entityName, "PublicViewGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "430C2FE2-B233-42DA-B508-1A22EF03383D";
			}
			else if (string.Equals(entityName, "PrivateViewGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = GetPrivateUserViewGroupId(requestContext);
			}
			else if (string.Equals(entityName, "ViewLayoutGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "430C2FE2-B233-42DA-B508-1A22EF03383D";
			}
			else if (string.Equals(entityName, "FailoverServerGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "6A85FFC3-C134-4338-A072-024817CB5820";
			}
			else if (string.Equals(entityName, "MIPObjectGroup", StringComparison.OrdinalIgnoreCase))
			{
				parentGroupId = "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683";
			}
		}
		return parentGroupId;
	}

	private static GenericObject2[] ConvertToGenericObject2s(IGeminiRequestContext requestContext, ServerGroup[] serverGroups)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (ServerGroup serverGroup in serverGroups)
		{
			GenericObject2 genericObject = ConvertToGenericObject2(requestContext, serverGroup);
			if (genericObject != null)
			{
				list.Add(genericObject);
			}
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToGenericObject2(IGeminiRequestContext requestContext, ServerGroup serverGroup)
	{
		if (serverGroup == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", serverGroup.Id));
		list.Add(new ObjectProperty2("Name", serverGroup.Name));
		list.Add(new ObjectProperty2("Description", serverGroup.Description));
		list.Add(new ObjectProperty2("CreatedDate", serverGroup.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", serverGroup.LastModified));
		list.Add(new ObjectProperty2("ParentServerGroupId", serverGroup.ParentId));
		string[] array = (from r in serverGroup.RelationIds.Split(';')
			select r.Trim() into r
			where !string.IsNullOrEmpty(r)
			select r).ToArray();
		bool flag = ServerGroup.HasPrivateViewGroupInRelations(array);
		string text = null;
		if (ServerGroup.IsStructureTypeViewGroup(serverGroup.StructureType))
		{
			text = ((!flag) ? "PublicViewGroup" : "PrivateViewGroup");
		}
		else if (ServerGroup.IsStructureTypeViewLayoutGroup(serverGroup.StructureType))
		{
			text = "ViewLayoutGroup";
		}
		else if (ServerGroup.IsStructureTypeFailoverGroup(serverGroup.StructureType))
		{
			text = "FailoverServerGroup";
		}
		else if (ServerGroup.IsStructureTypeMIPObjectGroup(serverGroup.StructureType))
		{
			text = "MIPObjectGroup";
		}
		if (string.IsNullOrEmpty(text))
		{
			throw new ArgumentException("Unsupported structure type for group: " + serverGroup.StructureType, "serverGroup");
		}
		string privateUserViewGroupId = string.Empty;
		if (flag)
		{
			privateUserViewGroupId = ServerGroup.QueryPrivateUserViewGroupId(requestContext, requestContext.UserDomainAccountName);
		}
		List<RelationProperty> list2 = new List<RelationProperty>();
		foreach (string item in array.Where((string r) => !string.Equals(r, serverGroup.Id, StringComparison.OrdinalIgnoreCase) && !string.Equals(r, privateUserViewGroupId, StringComparison.OrdinalIgnoreCase) && !ServerGroup.IsPrivateViewGroupRoot(r) && !ServerGroup.IsSpecialType(r, "6A85FFC3-C134-4338-A072-024817CB5820") && !ServerGroup.IsSpecialType(r, "430C2FE2-B233-42DA-B508-1A22EF03383D") && !ServerGroup.IsSpecialType(r, "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683")))
		{
			list2.Add(new RelationProperty(text, item));
		}
		return new GenericObject2(serverGroup.Id, text, list.ToArray(), list2.ToArray());
	}

	public static ServerGroup ConvertToServerGroup(ObjectProperty2[] properties, CustomProperty[] customProperties)
	{
		ServerGroup serverGroup = new ServerGroup();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			serverGroup.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		else
		{
			serverGroup.Id = string.Empty;
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			serverGroup.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			serverGroup.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			serverGroup.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			serverGroup.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "ParentServerGroupId"))
		{
			serverGroup.ParentId = GenericObjectHelper2.GetProperty<string>(properties, "ParentServerGroupId");
		}
		serverGroup.CustomProperties = customProperties;
		return serverGroup;
	}
}
