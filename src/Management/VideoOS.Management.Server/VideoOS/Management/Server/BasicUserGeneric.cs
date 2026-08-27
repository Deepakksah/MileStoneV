using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects.ManagementServer;

namespace VideoOS.Management.Server;

[EntityName("BasicUser")]
[FunctionHandler("QueryUserClaims")]
public class BasicUserGeneric : IGenericObjectSupport2, IGenericFunction
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToBasicUserGenericObject2(BasicUser.QueryBasicUserById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToBasicUserGenericObject2s(BasicUser.QueryBasicUsersByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "BasicUser")
		{
			return BasicUser.QueryBasicUserIds(requestContext);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		BasicUser basicUser = ConvertToBasicUser(properties);
		string property = GenericObjectHelper2.GetProperty<string>(properties, "Password");
		basicUser = BasicUser.CreateBasicUser(requestContext, basicUser.Id, basicUser.Name, basicUser.Description, basicUser.CanChangePassword, property, basicUser.ForcePasswordChange, basicUser.Status);
		return ConvertToBasicUserGenericObject2(basicUser);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		BasicUser basicUser = ConvertToBasicUser(genericObject);
		basicUser = BasicUser.UpdateBasicUser(requestContext, basicUser.Id, basicUser.Description, basicUser.CanChangePassword, basicUser.ForcePasswordChange, basicUser.Status);
		return ConvertToBasicUserGenericObject2(basicUser);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		BasicUser.DeleteBasicUser(requestContext, id);
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		if (functionName == "QueryUserClaims")
		{
			return QueryUserClaims(requestContext, parameters);
		}
		throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>");
	}

	private static GenericObject2[] QueryUserClaims(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string propertyOrDefault = genericObject.Properties.GetPropertyOrDefault<string>("Id");
		return EntityConvert.ConvertToGenericObject2s(BasicUser.QueryBasicUserById(requestContext, propertyOrDefault).Claims);
	}

	public static void ChangePasswordBasicUser(IGeminiRequestContext requestContext, string id, string newPassword)
	{
		BasicUser.ChangePasswordBasicUser(requestContext, id, newPassword);
	}

	private static GenericObject2 ConvertToBasicUserGenericObject2(BasicUser basicUser)
	{
		if (basicUser == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", basicUser.Id));
		list.Add(new ObjectProperty2("Name", basicUser.Name));
		list.Add(new ObjectProperty2("Description", basicUser.Description));
		list.Add(new ObjectProperty2("CreatedDate", basicUser.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", basicUser.LastModified));
		list.Add(new ObjectProperty2("Sid", basicUser.Sid));
		list.Add(new ObjectProperty2("CanChangePassword", basicUser.CanChangePassword));
		list.Add(new ObjectProperty2("ForcePasswordChange", basicUser.ForcePasswordChange));
		list.Add(new ObjectProperty2("Status", basicUser.Status.ToString()));
		if (basicUser.LockoutEnd.HasValue)
		{
			list.Add(new ObjectProperty2("LockoutEnd", basicUser.LockoutEnd.Value.ToString("o", CultureInfo.InvariantCulture)));
		}
		list.Add(new ObjectProperty2("IsExternal", basicUser.IsExternal));
		return new GenericObject2(basicUser.Id, "BasicUser", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToBasicUserGenericObject2s(BasicUser[] basicUsers)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (BasicUser basicUser in basicUsers)
		{
			list.Add(ConvertToBasicUserGenericObject2(basicUser));
		}
		return list.ToArray();
	}

	private static BasicUser ConvertToBasicUser(GenericObject2 genericObject)
	{
		return ConvertToBasicUser(genericObject.Properties);
	}

	private static BasicUser ConvertToBasicUser(ObjectProperty2[] properties)
	{
		BasicUser basicUser = new BasicUser();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			basicUser.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			basicUser.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			basicUser.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			basicUser.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			basicUser.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Sid"))
		{
			basicUser.Sid = GenericObjectHelper2.GetProperty<string>(properties, "Sid");
		}
		if (GenericObjectHelper2.Contains(properties, "CanChangePassword"))
		{
			basicUser.CanChangePassword = GenericObjectHelper2.GetProperty<bool>(properties, "CanChangePassword");
		}
		if (GenericObjectHelper2.Contains(properties, "ForcePasswordChange"))
		{
			basicUser.ForcePasswordChange = GenericObjectHelper2.GetProperty<bool>(properties, "ForcePasswordChange");
		}
		if (GenericObjectHelper2.Contains(properties, "Status"))
		{
			basicUser.Status = GenericObjectHelper2.GetProperty<BasicUserStatus>(properties, "Status");
		}
		if (GenericObjectHelper2.Contains(properties, "LockoutEnd"))
		{
			basicUser.LockoutEnd = GenericObjectHelper2.GetProperty<DateTime>(properties, "LockoutEnd");
		}
		if (GenericObjectHelper2.Contains(properties, "IsExternal"))
		{
			basicUser.IsExternal = GenericObjectHelper2.GetProperty<bool>(properties, "IsExternal");
		}
		return basicUser;
	}
}
