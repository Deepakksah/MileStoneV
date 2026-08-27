using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MIPObjectGroup")]
public sealed class MIPServerGroupGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		MIPServerGroup serverGroup = MIPServerGroup.QueryServerGroupById(requestContext, id);
		return ConvertToGenericObject2(requestContext, serverGroup);
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		MIPServerGroup[] serverGroups = MIPServerGroup.QueryServerGroupsByIds(requestContext, ids);
		return ConvertToGenericObject2s(requestContext, serverGroups);
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		parentId = ConvertManagementServerIdToTopServerId(parentEntityName, entityName, parentId);
		ServerGroup.EnumStructureType structureType = ServerGroup.ConvertFromEntityNameToEnumStructureType(entityName);
		MIPServerGroup[] array = MIPServerGroup.QueryServerGroupsByParentGroupId(requestContext, structureType, parentId);
		List<string> list = new List<string>();
		MIPServerGroup[] array2 = array;
		foreach (MIPServerGroup mIPServerGroup in array2)
		{
			if (!string.Equals(mIPServerGroup.Id, "4BF8A8B0-54AB-4E82-994F-98D8E9584176", StringComparison.OrdinalIgnoreCase))
			{
				list.Add(mIPServerGroup.Id);
			}
		}
		return list.ToArray();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		MIPServerGroup mIPServerGroup = ConvertToServerGroup(properties, customProperties);
		if (ServerGroupGeneric.IsManagementServerId(requestContext, mIPServerGroup.ParentId))
		{
			mIPServerGroup.ParentId = ConvertManagementServerIdToTopServerId("ManagementServer", entityName, mIPServerGroup.ParentId);
		}
		if (string.Equals(entityName, "ViewLayoutGroup", StringComparison.OrdinalIgnoreCase))
		{
			throw new ServerGroupCreationException(mIPServerGroup.Name, mIPServerGroup.ParentId);
		}
		mIPServerGroup = MIPServerGroup.CreateServerGroup(requestContext, mIPServerGroup);
		return ConvertToGenericObject2(requestContext, mIPServerGroup);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		MIPServerGroup serverGroup = ConvertToServerGroup(genericObject.Properties, customProperties);
		serverGroup = MIPServerGroup.UpdateServerGroup(requestContext, serverGroup);
		return ConvertToGenericObject2(requestContext, serverGroup);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		MIPServerGroup.DeleteServerGroup(requestContext, id);
	}

	private static string ConvertManagementServerIdToTopServerId(string parentEntityName, string entityName, string parentGroupId)
	{
		if (string.Equals(parentEntityName, "ManagementServer", StringComparison.OrdinalIgnoreCase) && string.Equals(entityName, "MIPObjectGroup", StringComparison.OrdinalIgnoreCase))
		{
			parentGroupId = "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683";
		}
		return parentGroupId;
	}

	private static GenericObject2[] ConvertToGenericObject2s(IGeminiRequestContext requestContext, MIPServerGroup[] serverGroups)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MIPServerGroup serverGroup in serverGroups)
		{
			GenericObject2 genericObject = ConvertToGenericObject2(requestContext, serverGroup);
			if (genericObject != null)
			{
				list.Add(genericObject);
			}
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToGenericObject2(IGeminiRequestContext requestContext, MIPServerGroup serverGroup)
	{
		if (serverGroup == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", serverGroup.Id));
		list.Add(new ObjectProperty2("Name", serverGroup.Name));
		list.Add(new ObjectProperty2("Description", serverGroup.Description));
		list.Add(new ObjectProperty2("Kind", serverGroup.Kind));
		list.Add(new ObjectProperty2("PluginId", serverGroup.PluginId));
		list.Add(new ObjectProperty2("CustomData", serverGroup.CustomData));
		list.Add(new ObjectProperty2("CreatedDate", serverGroup.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", serverGroup.LastModified));
		list.Add(new ObjectProperty2("ParentServerGroupId", serverGroup.ParentId));
		string[] array = serverGroup.RelationIds.Split(';');
		string entityName = "MIPObjectGroup";
		List<RelationProperty> list2 = new List<RelationProperty>();
		for (int i = 1; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (!string.IsNullOrEmpty(text))
			{
				ServerGroup serverGroup2 = ServerGroup.QueryServerGroupById(requestContext, text);
				serverGroup2 = ServerGroup.FilterServerGroupBasedOnPermissions(requestContext, serverGroup2);
				if (serverGroup2 == null || ServerGroup.IsSpecialType(serverGroup2.Id, "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683") || !ServerGroup.IsStructureTypeMIPObjectGroup(serverGroup2.StructureType))
				{
					break;
				}
				list2.Add(new RelationProperty("MIPObjectGroup", text));
			}
		}
		return new GenericObject2(serverGroup.Id, entityName, list.ToArray(), list2.ToArray());
	}

	private static MIPServerGroup ConvertToServerGroup(ObjectProperty2[] properties, CustomProperty[] customProperties)
	{
		MIPServerGroup mIPServerGroup = new MIPServerGroup();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			mIPServerGroup.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		else
		{
			mIPServerGroup.Id = string.Empty;
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			mIPServerGroup.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			mIPServerGroup.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "Kind"))
		{
			mIPServerGroup.Kind = GenericObjectHelper2.GetProperty<string>(properties, "Kind");
		}
		if (GenericObjectHelper2.Contains(properties, "PluginId"))
		{
			mIPServerGroup.PluginId = GenericObjectHelper2.GetProperty<string>(properties, "PluginId");
		}
		if (GenericObjectHelper2.Contains(properties, "CustomData"))
		{
			mIPServerGroup.CustomData = GenericObjectHelper2.GetProperty<string>(properties, "CustomData");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			mIPServerGroup.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			mIPServerGroup.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "ParentServerGroupId"))
		{
			mIPServerGroup.ParentId = GenericObjectHelper2.GetProperty<string>(properties, "ParentServerGroupId");
		}
		mIPServerGroup.CustomProperties = customProperties;
		return mIPServerGroup;
	}
}
