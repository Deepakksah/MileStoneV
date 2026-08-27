using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("RetentionOptionRoleRelation")]
public class RetentionOptionRoleRelationGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToRetentionOptionRoleRelationGenericObject2(RetentionOptionRoleRelation.QueryRetentionOptionRoleRelationByRoleId(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToRetentionOptionRoleRelationGenericObject2s(QueryRetentionOptionRoleRelationByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "RetentionOptionRoleRelation")
		{
			return QueryRetentionOptionRoleRelationIds(requestContext);
		}
		return null;
	}

	private static string[] QueryRetentionOptionRoleRelationIds(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRetentionOptionRoleRelationComponent retentionOptionRoleRelationComponent = requestContext.CreateRetentionOptionRoleRelationComponent();
		return retentionOptionRoleRelationComponent.QueryRetentionOptionRoleRelationIds();
	}

	private static RetentionOptionRoleRelation[] QueryRetentionOptionRoleRelationByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRetentionOptionRoleRelationComponent retentionOptionRoleRelationComponent = requestContext.CreateRetentionOptionRoleRelationComponent();
		return retentionOptionRoleRelationComponent.QueryRetentionOptionRoleRelationsByIds(ids);
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		RetentionOptionRoleRelation retentionOptionRoleRelation = ConvertToRetentionOptionRoleRelation(properties);
		retentionOptionRoleRelation = RetentionOptionRoleRelation.CreateRetentionOptionRoleRelation(requestContext, retentionOptionRoleRelation.IdRole, retentionOptionRoleRelation.IdRetention);
		return ConvertToRetentionOptionRoleRelationGenericObject2(retentionOptionRoleRelation);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		RetentionOptionRoleRelation retentionOptionRoleRelation = ConvertToRetentionOptionRoleRelation(genericObject);
		retentionOptionRoleRelation = RetentionOptionRoleRelation.UpdateRetentionOptionRoleRelation(requestContext, retentionOptionRoleRelation.IdRole, retentionOptionRoleRelation.IdRetention, retentionOptionRoleRelation.RetentionName);
		return ConvertToRetentionOptionRoleRelationGenericObject2(retentionOptionRoleRelation);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string idRole, int clientVersion)
	{
		RetentionOptionRoleRelation.DeleteRetentionOptionRoleRelation(requestContext, idRole);
	}

	private static GenericObject2 ConvertToRetentionOptionRoleRelationGenericObject2(RetentionOptionRoleRelation retentionOptionRoleRelation)
	{
		if (retentionOptionRoleRelation == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("IdRole", retentionOptionRoleRelation.IdRole));
		list.Add(new ObjectProperty2("IdRetentionOption", retentionOptionRoleRelation.IdRetention));
		list.Add(new ObjectProperty2("RetentionName", retentionOptionRoleRelation.RetentionName));
		return new GenericObject2(retentionOptionRoleRelation.IdRole, "RetentionOptionRoleRelation", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToRetentionOptionRoleRelationGenericObject2s(RetentionOptionRoleRelation[] retentionOptionRoleRelations)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (RetentionOptionRoleRelation retentionOptionRoleRelation in retentionOptionRoleRelations)
		{
			list.Add(ConvertToRetentionOptionRoleRelationGenericObject2(retentionOptionRoleRelation));
		}
		return list.ToArray();
	}

	private static RetentionOptionRoleRelation ConvertToRetentionOptionRoleRelation(GenericObject2 genericObject)
	{
		return ConvertToRetentionOptionRoleRelation(genericObject.Properties);
	}

	private static RetentionOptionRoleRelation ConvertToRetentionOptionRoleRelation(ObjectProperty2[] properties)
	{
		RetentionOptionRoleRelation retentionOptionRoleRelation = new RetentionOptionRoleRelation();
		if (GenericObjectHelper2.Contains(properties, "IdRole"))
		{
			retentionOptionRoleRelation.IdRole = GenericObjectHelper2.GetProperty<string>(properties, "IdRole");
		}
		if (GenericObjectHelper2.Contains(properties, "IdRetentionOption"))
		{
			retentionOptionRoleRelation.IdRetention = GenericObjectHelper2.GetProperty<string>(properties, "IdRetentionOption");
		}
		if (GenericObjectHelper2.Contains(properties, "RetentionName"))
		{
			retentionOptionRoleRelation.RetentionName = GenericObjectHelper2.GetProperty<string>(properties, "RetentionName");
		}
		return retentionOptionRoleRelation;
	}
}
