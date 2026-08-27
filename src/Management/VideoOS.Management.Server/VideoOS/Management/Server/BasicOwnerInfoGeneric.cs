using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("BasicOwnerInfo")]
public class BasicOwnerInfoGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToBasicOwnerInfoGenericObject2(BasicOwnerInfo.QueryBasicOwnerInfoById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToBasicOwnerInfoGenericObject2s(QueryBasicOwnerInfoByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "BasicOwnerInfo")
		{
			return QueryBasicOwnerInfoIds(requestContext);
		}
		return null;
	}

	private static string[] QueryBasicOwnerInfoIds(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
		return basicOwnerInfoComponent.QueryBasicOwnerInfoIds();
	}

	private static BasicOwnerInfo[] QueryBasicOwnerInfoByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
		return basicOwnerInfoComponent.QueryBasicOwnerInfosByIds(ids);
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		BasicOwnerInfo basicOwnerInfo = ConvertToBasicOwnerInfo(properties);
		basicOwnerInfo = BasicOwnerInfo.CreateBasicOwnerInfo(requestContext, basicOwnerInfo.Id, basicOwnerInfo.Name, basicOwnerInfo.Property, basicOwnerInfo.Order);
		return ConvertToBasicOwnerInfoGenericObject2(basicOwnerInfo);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		BasicOwnerInfo basicOwnerInfo = ConvertToBasicOwnerInfo(genericObject);
		basicOwnerInfo = BasicOwnerInfo.UpdateBasicOwnerInfo(requestContext, basicOwnerInfo.Id, basicOwnerInfo.Name, basicOwnerInfo.Property, basicOwnerInfo.Order);
		return ConvertToBasicOwnerInfoGenericObject2(basicOwnerInfo);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		BasicOwnerInfo.DeleteBasicOwnerInfo(requestContext, id);
	}

	private static GenericObject2 ConvertToBasicOwnerInfoGenericObject2(BasicOwnerInfo basicOwnerInfo)
	{
		if (basicOwnerInfo == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", basicOwnerInfo.Id));
		list.Add(new ObjectProperty2("Name", basicOwnerInfo.Name));
		list.Add(new ObjectProperty2("Property", basicOwnerInfo.Property));
		list.Add(new ObjectProperty2("Order", basicOwnerInfo.Order));
		return new GenericObject2(basicOwnerInfo.Id, "BasicOwnerInfo", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToBasicOwnerInfoGenericObject2s(BasicOwnerInfo[] basicOwnerInfos)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (BasicOwnerInfo basicOwnerInfo in basicOwnerInfos)
		{
			list.Add(ConvertToBasicOwnerInfoGenericObject2(basicOwnerInfo));
		}
		return list.ToArray();
	}

	private static BasicOwnerInfo ConvertToBasicOwnerInfo(GenericObject2 genericObject)
	{
		return ConvertToBasicOwnerInfo(genericObject.Properties);
	}

	private static BasicOwnerInfo ConvertToBasicOwnerInfo(ObjectProperty2[] properties)
	{
		BasicOwnerInfo basicOwnerInfo = new BasicOwnerInfo();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			basicOwnerInfo.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			basicOwnerInfo.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Property"))
		{
			basicOwnerInfo.Property = GenericObjectHelper2.GetProperty<string>(properties, "Property");
		}
		if (GenericObjectHelper2.Contains(properties, "Order"))
		{
			basicOwnerInfo.Order = GenericObjectHelper2.GetProperty<string>(properties, "Order");
		}
		return basicOwnerInfo;
	}
}
