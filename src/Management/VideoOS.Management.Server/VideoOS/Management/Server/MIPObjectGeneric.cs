using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MIPObject")]
public sealed class MIPObjectGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(MIPObject.QueryMIPObject(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(MIPObject.QueryMIPObjectsByIds(requestContext, ids, needAdminSecurity: true));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		parentId = ConvertManagementServerIdToTopServerId(parentEntityName, parentId);
		MIPObject[] array = MIPObject.QueryMIPObjects(requestContext, parentId, checkApplicationPermission: true);
		List<string> list = new List<string>();
		MIPObject[] array2 = array;
		foreach (MIPObject mIPObject in array2)
		{
			list.Add(mIPObject.Id);
		}
		return list.ToArray();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		MIPObject mIPObject = ConvertToMIPObject(properties);
		if (IsManagementServerId(requestContext, mIPObject.ParentGroupId))
		{
			mIPObject.ParentGroupId = ConvertManagementServerIdToTopServerId("ManagementServer", mIPObject.ParentGroupId);
		}
		mIPObject = MIPObject.CreateMIPObject(requestContext, mIPObject.Id, mIPObject.ParentGroupId, mIPObject.Name, mIPObject.Kind, mIPObject.SubKind, mIPObject.PluginId, mIPObject.CustomData);
		return ConvertToGenericObject2(mIPObject);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		MIPObject mIPObject = ConvertToMIPObject(genericObject.Properties);
		mIPObject = MIPObject.UpdateMIPObject(requestContext, mIPObject.Id, mIPObject.Name, mIPObject.CustomData);
		return ConvertToGenericObject2(mIPObject);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		if (MIPObject.QueryMIPObject(requestContext, id) == null)
		{
			throw new MIPObjectNotFoundException();
		}
		MIPObject.DeleteMIPObject(requestContext, id);
	}

	private static bool IsManagementServerId(IGeminiRequestContext requestContext, string id)
	{
		Site site = Site.QueryHomeSiteNoSecurity(requestContext);
		if (string.Equals(id, site.Id, StringComparison.OrdinalIgnoreCase) || string.Equals(id, "909749AE-88C4-4780-A381-383D691E98FA", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static string ConvertManagementServerIdToTopServerId(string parentEntityName, string parentGroupId)
	{
		if (string.Equals(parentEntityName, "ManagementServer", StringComparison.OrdinalIgnoreCase))
		{
			parentGroupId = "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683";
		}
		return parentGroupId;
	}

	private static GenericObject2[] ConvertToGenericObject2s(MIPObject[] mipObjects)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		for (int i = 0; i < mipObjects.Length; i++)
		{
			GenericObject2 genericObject = ConvertToGenericObject2(mipObjects[i]);
			if (genericObject != null)
			{
				list.Add(genericObject);
			}
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToGenericObject2(MIPObject mipObject)
	{
		if (mipObject == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", mipObject.Id));
		list.Add(new ObjectProperty2("Name", mipObject.Name));
		list.Add(new ObjectProperty2("CreatedDate", mipObject.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", mipObject.LastModified));
		list.Add(new ObjectProperty2("Kind", mipObject.Kind));
		list.Add(new ObjectProperty2("SubKind", mipObject.SubKind));
		list.Add(new ObjectProperty2("PluginId", mipObject.PluginId));
		list.Add(new ObjectProperty2("CustomData", mipObject.CustomData));
		string[] array = mipObject.RelationIds.Split(';');
		bool flag = false;
		List<RelationProperty> list2 = new List<RelationProperty>();
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (string.IsNullOrEmpty(text))
			{
				continue;
			}
			if (ServerGroup.IsSpecialType(text, "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683"))
			{
				if (!flag)
				{
					list.Add(new ObjectProperty2("ParentServerGroupId", "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683"));
					flag = true;
				}
				break;
			}
			if (!flag)
			{
				list.Add(new ObjectProperty2("ParentServerGroupId", text));
				flag = true;
			}
			list2.Add(new RelationProperty("MIPObjectGroup", text, isWeakRelation: true));
		}
		return new GenericObject2(mipObject.Id, "MIPObject", list.ToArray(), list2.ToArray());
	}

	private static MIPObject ConvertToMIPObject(ObjectProperty2[] properties)
	{
		MIPObject mIPObject = new MIPObject();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			mIPObject.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		else
		{
			mIPObject.Id = string.Empty;
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			mIPObject.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			mIPObject.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			mIPObject.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Kind"))
		{
			mIPObject.Kind = GenericObjectHelper2.GetProperty<string>(properties, "Kind");
		}
		if (GenericObjectHelper2.Contains(properties, "SubKind"))
		{
			mIPObject.SubKind = GenericObjectHelper2.GetProperty<string>(properties, "SubKind");
		}
		if (GenericObjectHelper2.Contains(properties, "PluginId"))
		{
			mIPObject.PluginId = GenericObjectHelper2.GetProperty<string>(properties, "PluginId");
		}
		if (GenericObjectHelper2.Contains(properties, "ParentServerGroupId"))
		{
			mIPObject.ParentGroupId = GenericObjectHelper2.GetProperty<string>(properties, "ParentServerGroupId");
		}
		if (GenericObjectHelper2.Contains(properties, "CustomData"))
		{
			mIPObject.CustomData = GenericObjectHelper2.GetProperty<string>(properties, "CustomData");
		}
		return mIPObject;
	}
}
