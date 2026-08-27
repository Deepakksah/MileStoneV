using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects.ManagementServer;

namespace VideoOS.Management.Server;

[EntityName("Role")]
[FunctionHandler("AddRoleClaim")]
[FunctionHandler("RemoveRoleClaim")]
[FunctionHandler("QueryRoleClaims")]
public class RoleGeneric : IGenericObjectSupport2, IGenericFunction
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToRoleGenericObject2(Role.QueryRoleById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToRoleGenericObject2s(Role.QueryRolesByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "Role")
		{
			return Role.QueryRoleIds(requestContext);
		}
		switch (parentEntityName)
		{
		case "ClientProfile":
		case "ManagementClientProfile":
		case "SmartClientProfile":
			if (entityName == "Role")
			{
				return Role.QueryRoleIdsByClientProfileId(requestContext, parentId);
			}
			break;
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		Role role = ConvertToRole(properties);
		return ConvertToRoleGenericObject2(Role.CreateRole(requestContext, role.Name, role.Description, role.DualAuthorizationRequired, role.ShowPtzReservedNameAsAnonymous, role.AllowMobileClientLogOn, role.AllowSmartClientLogOn, role.AllowWebClientLogOn));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		Role role = ConvertToRole(genericObject);
		return ConvertToRoleGenericObject2(Role.UpdateRole(requestContext, role.Id, role.Name, role.Description, role.DualAuthorizationRequired, role.ShowPtzReservedNameAsAnonymous, role.AllowMobileClientLogOn, role.AllowSmartClientLogOn, role.AllowWebClientLogOn));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		Role.DeleteRole(requestContext, id);
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		return functionName switch
		{
			"AddRoleClaim" => AddRoleClaim(requestContext, parameters), 
			"QueryRoleClaims" => QueryRoleClaims(requestContext, parameters), 
			"RemoveRoleClaim" => RemoveRoleClaim(requestContext, parameters), 
			_ => throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>"), 
		};
	}

	private static GenericObject2[] AddRoleClaim(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string propertyOrDefault = genericObject.Properties.GetPropertyOrDefault<string>("Id");
		string propertyOrDefault2 = genericObject.Properties.GetPropertyOrDefault<string>("ClaimProvider");
		string propertyOrDefault3 = genericObject.Properties.GetPropertyOrDefault<string>("ClaimName");
		string propertyOrDefault4 = genericObject.Properties.GetPropertyOrDefault<string>("ClaimValue");
		Role.AddClaimToRole(requestContext, propertyOrDefault, propertyOrDefault2, propertyOrDefault3, propertyOrDefault4);
		return null;
	}

	private static GenericObject2[] QueryRoleClaims(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string propertyOrDefault = genericObject.Properties.GetPropertyOrDefault<string>("Id");
		return EntityConvert.ConvertToGenericObject2s(Role.QueryClaimsForRole(requestContext, propertyOrDefault));
	}

	private static GenericObject2[] RemoveRoleClaim(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string propertyOrDefault = genericObject.Properties.GetPropertyOrDefault<string>("Id");
		string propertyOrDefault2 = genericObject.Properties.GetPropertyOrDefault<string>("ClaimProvider");
		string propertyOrDefault3 = genericObject.Properties.GetPropertyOrDefault<string>("ClaimName");
		string propertyOrDefault4 = genericObject.Properties.GetPropertyOrDefault<string>("ClaimValue");
		Role.RemoveClaimFromRole(requestContext, propertyOrDefault, propertyOrDefault2, propertyOrDefault3, propertyOrDefault4);
		return null;
	}

	private static GenericObject2 ConvertToRoleGenericObject2(Role role)
	{
		if (role == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", role.Id));
		list.Add(new ObjectProperty2("Name", role.Name));
		list.Add(new ObjectProperty2("Description", role.Description));
		list.Add(new ObjectProperty2("LastModified", role.LastModified));
		list.Add(new ObjectProperty2("RoleType", role.Type.ToString()));
		list.Add(new ObjectProperty2("DualAuthorizationRequired", role.DualAuthorizationRequired));
		list.Add(new ObjectProperty2("ShowPtzReservedNameAsAnonymous", role.ShowPtzReservedNameAsAnonymous));
		list.Add(new ObjectProperty2("AllowMobileClientLogOn", role.AllowMobileClientLogOn));
		list.Add(new ObjectProperty2("AllowSmartClientLogOn", role.AllowSmartClientLogOn));
		list.Add(new ObjectProperty2("AllowWebClientLogOn", role.AllowWebClientLogOn));
		return new GenericObject2(role.Id, "Role", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToRoleGenericObject2s(Role[] roles)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (Role role in roles)
		{
			list.Add(ConvertToRoleGenericObject2(role));
		}
		return list.ToArray();
	}

	private static Role ConvertToRole(GenericObject2 genericObject)
	{
		return ConvertToRole(genericObject.Properties);
	}

	private static Role ConvertToRole(ObjectProperty2[] properties)
	{
		Role role = new Role();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			role.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			role.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			role.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			role.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "RoleType"))
		{
			role.Type = GenericObjectHelper2.GetProperty<RoleType>(properties, "RoleType");
		}
		if (GenericObjectHelper2.Contains(properties, "DualAuthorizationRequired"))
		{
			role.DualAuthorizationRequired = GenericObjectHelper2.GetProperty<bool>(properties, "DualAuthorizationRequired");
		}
		if (GenericObjectHelper2.Contains(properties, "ShowPtzReservedNameAsAnonymous"))
		{
			role.ShowPtzReservedNameAsAnonymous = GenericObjectHelper2.GetProperty<bool>(properties, "ShowPtzReservedNameAsAnonymous");
		}
		if (GenericObjectHelper2.Contains(properties, "AllowMobileClientLogOn"))
		{
			role.AllowMobileClientLogOn = GenericObjectHelper2.GetProperty<bool>(properties, "AllowMobileClientLogOn");
		}
		if (GenericObjectHelper2.Contains(properties, "AllowSmartClientLogOn"))
		{
			role.AllowSmartClientLogOn = GenericObjectHelper2.GetProperty<bool>(properties, "AllowSmartClientLogOn");
		}
		if (GenericObjectHelper2.Contains(properties, "AllowWebClientLogOn"))
		{
			role.AllowWebClientLogOn = GenericObjectHelper2.GetProperty<bool>(properties, "AllowWebClientLogOn");
		}
		return role;
	}
}
