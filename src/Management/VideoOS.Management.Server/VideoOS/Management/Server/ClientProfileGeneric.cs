using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("ManagementClientProfile")]
[EntityName("SmartClientProfile")]
[EntityName("ClientProfile")]
public class ClientProfileGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToClientProfileGenericObject2(ClientProfile.QueryClientProfileById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToClientProfileGenericObject2s(ClientProfile.QueryClientProfilesByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName.Equals("ManagementServer", StringComparison.OrdinalIgnoreCase) && entityName.Equals("ManagementClientProfile", StringComparison.OrdinalIgnoreCase))
		{
			return ClientProfile.QueryClientProfileIds(requestContext, ClientProfileType.ManagementClientProfile);
		}
		if (parentEntityName.Equals("ManagementServer", StringComparison.OrdinalIgnoreCase) && entityName.Equals("SmartClientProfile", StringComparison.OrdinalIgnoreCase))
		{
			return ClientProfile.QueryClientProfileIds(requestContext, ClientProfileType.SmartClientProfile);
		}
		if (parentEntityName.Equals("Role", StringComparison.OrdinalIgnoreCase) && entityName.Equals("ManagementClientProfile", StringComparison.OrdinalIgnoreCase))
		{
			string text = ClientProfile.QueryClientProfileIdByRoleId(requestContext, ClientProfileType.ManagementClientProfile, parentId);
			return new string[1] { text };
		}
		if (parentEntityName.Equals("Role", StringComparison.OrdinalIgnoreCase) && entityName.Equals("SmartClientProfile", StringComparison.OrdinalIgnoreCase))
		{
			string text2 = ClientProfile.QueryClientProfileIdByRoleId(requestContext, ClientProfileType.SmartClientProfile, parentId);
			return new string[1] { text2 };
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		ClientProfile clientProfile = ConvertToClientProfile(properties, GetClientProfileType(entityName));
		clientProfile = ClientProfile.CreateClientProfile(requestContext, clientProfile.Id, clientProfile.Name, clientProfile.Description, clientProfile.SettingsXml, clientProfile.Profiletype);
		return ConvertToClientProfileGenericObject2(clientProfile);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		ClientProfile clientProfile = ConvertToClientProfile(genericObject);
		clientProfile = ClientProfile.UpdateClientProfile(requestContext, clientProfile.Id, clientProfile.Name, clientProfile.Description, clientProfile.SettingsXml, GetClientProfileType(genericObject.EntityName));
		return ConvertToClientProfileGenericObject2(clientProfile);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		ClientProfile.DeleteClientProfile(requestContext, id);
	}

	public static void MovePriorityUp(IGeminiRequestContext requestContext, string id)
	{
		ClientProfile.UpdateClientProfileMoveUp(requestContext, id);
	}

	public static void MovePriorityDown(IGeminiRequestContext requestContext, string id)
	{
		ClientProfile.UpdateClientProfileMoveDown(requestContext, id);
	}

	public static string QueryActiveManagementClientProfileId(GeminiRequestContext requestContext)
	{
		return ClientProfile.QueryActiveProfileId(requestContext, ClientProfileType.ManagementClientProfile);
	}

	public static string QueryActiveSmartClientProfileId(GeminiRequestContext requestContext)
	{
		return ClientProfile.QueryActiveProfileId(requestContext, ClientProfileType.SmartClientProfile);
	}

	public static string QueryActiveSmartClientProfileId(IGeminiRequestContext requestContext, string sid)
	{
		return ClientProfile.QueryActiveProfileId(requestContext, ClientProfileType.SmartClientProfile, sid);
	}

	public static void AttachClientProfileToRole(IGeminiRequestContext requestContext, string id, string roleId)
	{
		ClientProfile.AttachClientProfileToRole(requestContext, id, roleId);
	}

	private static GenericObject2 ConvertToClientProfileGenericObject2(ClientProfile clientProfile)
	{
		if (clientProfile == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", clientProfile.Id));
		list.Add(new ObjectProperty2("Name", clientProfile.Name));
		list.Add(new ObjectProperty2("Description", clientProfile.Description));
		list.Add(new ObjectProperty2("LastModified", clientProfile.LastModified));
		list.Add(new ObjectProperty2("Settings", clientProfile.SettingsXml));
		list.Add(new ObjectProperty2("IsDefault", clientProfile.IsDefaultProfile));
		if (clientProfile.Profiletype == ClientProfileType.ManagementClientProfile)
		{
			return new GenericObject2(clientProfile.Id, "ManagementClientProfile", list.ToArray(), null);
		}
		if (clientProfile.Profiletype == ClientProfileType.SmartClientProfile)
		{
			return new GenericObject2(clientProfile.Id, "SmartClientProfile", list.ToArray(), null);
		}
		throw new NotSupportedException("Unknown profile type");
	}

	private static GenericObject2[] ConvertToClientProfileGenericObject2s(ClientProfile[] clientProfiles)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (ClientProfile clientProfile in clientProfiles)
		{
			list.Add(ConvertToClientProfileGenericObject2(clientProfile));
		}
		return list.ToArray();
	}

	private static ClientProfile ConvertToClientProfile(GenericObject2 genericObject)
	{
		return ConvertToClientProfile(genericObject.Properties, GetClientProfileType(genericObject.EntityName));
	}

	private static ClientProfile ConvertToClientProfile(ObjectProperty2[] properties, ClientProfileType profileType)
	{
		ClientProfile clientProfile = new ClientProfile();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			clientProfile.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			clientProfile.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			clientProfile.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			clientProfile.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Settings"))
		{
			clientProfile.SettingsXml = GenericObjectHelper2.GetProperty<string>(properties, "Settings");
		}
		clientProfile.Profiletype = profileType;
		return clientProfile;
	}

	private static ClientProfileType GetClientProfileType(string entityName)
	{
		if (entityName.Equals("ManagementClientProfile", StringComparison.OrdinalIgnoreCase))
		{
			return ClientProfileType.ManagementClientProfile;
		}
		if (entityName.Equals("SmartClientProfile", StringComparison.OrdinalIgnoreCase))
		{
			return ClientProfileType.SmartClientProfile;
		}
		throw new NotSupportedException("Unknown profile type");
	}
}
