using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class ServerGroup
{
	public enum EnumStructureType
	{
		Unknown,
		SmartClientGroup,
		ViewGroup,
		ViewLayoutGroup,
		FailoverGroup,
		MIPObjectGroup,
		AccessControlGroup
	}

	public const string SmartClientGroupId = "430C2FE2-B233-42DA-B508-1A22EF03383D";

	public const string RecordingServerGroupId = "E0BE6096-A1D6-464e-B028-B45FF2BA28AF";

	public const string FailoverGroupId = "6A85FFC3-C134-4338-A072-024817CB5820";

	public const string MIPObjectGroupId = "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683";

	public const string DevicesGroupId = "9178D2CE-888E-4058-9992-C99D506BE63A";

	public const string ManagementServerId = "909749AE-88C4-4780-A381-383D691E98FA";

	public const string PrivateViewGroupId = "4BF8A8B0-54AB-4E82-994F-98D8E9584176";

	public const string ViewLayoutGroup4_3Id = "7EB5B9CC-D56F-4785-BA72-4E71FADA8C6B";

	public const string ViewLayoutGroup16_9Id = "14C3A703-6835-4843-B18C-8DB95436A80C";

	public const string ViewLayoutGroup4_3PortraitId = "E0199F86-4A16-4A1F-B655-8BE7B9DCA3E1";

	public const string ViewLayoutGroup16_9PortraitId = "9F4A3B56-9A30-44B5-9C19-5E7421164F96";

	public const string AccessControlGroupId = "6ACA51C5-A716-48A1-A2DC-AEEADA59C771";

	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Path { get; set; }

	public string ParentId { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public EnumStructureType StructureType { get; set; }

	public string RelationIds { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static ServerGroup[] QueryServerGroupsByParentGroupIdNoSecurity(IGeminiRequestContext requestContext, EnumStructureType structureType, string parentGroupId)
	{
		string groupStructureType = ConvertFromEnumStructureTypeToStringStructureType(structureType);
		return QueryServerGroupsByParentGroupIdNoSecurity(requestContext, groupStructureType, parentGroupId);
	}

	private static ServerGroup[] QueryServerGroupsByParentGroupIdNoSecurity(IGeminiRequestContext requestContext, string groupStructureType, string parentGroupId)
	{
		ServerGroup[] array = null;
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			array = serverGroupComponent.QueryServerGroupsByParentGroupId(groupStructureType, parentGroupId);
		}
		if (array == null)
		{
			return new ServerGroup[0];
		}
		return array;
	}

	public static ServerGroup[] QueryServerGroupsByParentGroupId(IGeminiRequestContext requestContext, EnumStructureType structureType, string parentGroupId)
	{
		string groupStructureType = ConvertFromEnumStructureTypeToStringStructureType(structureType);
		ServerGroup[] array = QueryServerGroupsByParentGroupIdNoSecurity(requestContext, groupStructureType, parentGroupId);
		if (HasPermissions(groupStructureType))
		{
			array = FilterServerGroupsBasedOnPermissions(requestContext, array);
		}
		return array;
	}

	public static ServerGroup QueryServerGroupByParentGroupId(IGeminiRequestContext requestContext, string parentGroupId, string serverGroupId)
	{
		ServerGroup serverGroup = null;
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			serverGroup = serverGroupComponent.QueryServerGroupByParentGroupId(parentGroupId, serverGroupId);
		}
		return FilterServerGroupBasedOnPermissions(requestContext, serverGroup);
	}

	public static ServerGroup QueryServerGroupByName(IGeminiRequestContext requestContext, string parentGroupId, string name)
	{
		string idFromName = GetIdFromName(requestContext, parentGroupId, name);
		if (!string.IsNullOrEmpty(idFromName))
		{
			return QueryServerGroupByParentGroupId(requestContext, parentGroupId, idFromName);
		}
		return null;
	}

	public static string GetIdFromName(IGeminiRequestContext requestContext, string parentGroupId, string name)
	{
		using IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent();
		return serverGroupComponent.GetIdFromName(parentGroupId, name);
	}

	public static string GetParentServerGroupId(IGeminiRequestContext requestContext, string serverGroupId)
	{
		using IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent();
		return serverGroupComponent.GetParentServerGroupId(serverGroupId);
	}

	public static ServerGroup[] QueryServerGroupHeriarchyByIdNoSecurity(IGeminiRequestContext requestContext, string serverGroupId)
	{
		ServerGroup serverGroup = QueryServerGroupById(requestContext, serverGroupId);
		List<ServerGroup> list = new List<ServerGroup>();
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			list.AddRange(serverGroupComponent.QueryServerGroupHierarchyById(ConvertFromEnumStructureTypeToStringStructureType(serverGroup.StructureType), serverGroupId));
		}
		return list.ToArray();
	}

	public static ServerGroup[] QueryServerGroupAllNoSecurity(IGeminiRequestContext requestContext, EnumStructureType serverGroupType)
	{
		List<ServerGroup> list = new List<ServerGroup>();
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			list.AddRange(serverGroupComponent.QueryServerGroupAllNoSecurity(ConvertFromEnumStructureTypeToStringStructureType(serverGroupType)));
		}
		return list.ToArray();
	}

	public static ServerGroup[] QueryServerGroupAllChecked(IGeminiRequestContext requestContext, EnumStructureType serverGroupType)
	{
		List<ServerGroup> list = new List<ServerGroup>();
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			ServerGroup[] array = serverGroupComponent.QueryServerGroupAllNoSecurity(ConvertFromEnumStructureTypeToStringStructureType(serverGroupType));
			foreach (ServerGroup serverGroup in array)
			{
				if (FilterServerGroupBasedOnPermissions(requestContext, serverGroup) != null)
				{
					list.Add(serverGroup);
				}
			}
		}
		return list.ToArray();
	}

	public static ServerGroup[] QueryServerGroupHeriarchyByIdNoSecurity(IGeminiRequestContext requestContext, string serverGroupId, EnumStructureType topGroupStructure)
	{
		List<ServerGroup> list = new List<ServerGroup>();
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			list.AddRange(serverGroupComponent.QueryServerGroupHierarchyById(ConvertFromEnumStructureTypeToStringStructureType(topGroupStructure), serverGroupId));
		}
		return list.ToArray();
	}

	public static ServerGroup QueryServerGroupById(IGeminiRequestContext requestContext, string serverGroupId)
	{
		ServerGroup serverGroup = null;
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			serverGroup = serverGroupComponent.QueryServerGroupById(serverGroupId);
		}
		serverGroup = FilterServerGroupBasedOnPermissions(requestContext, serverGroup);
		if (serverGroup == null)
		{
			throw new ServerGroupNotFoundException();
		}
		return serverGroup;
	}

	public static ServerGroup[] QueryServerGroupsByIds(IGeminiRequestContext requestContext, string[] serverGroupIds)
	{
		ServerGroup[] serverGroups = null;
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			serverGroups = serverGroupComponent.QueryServerGroupsByIds(serverGroupIds);
		}
		return FilterServerGroupsBasedOnPermissions(requestContext, serverGroups);
	}

	public static string GetItemIdFromServerGroupId(IGeminiRequestContext requestContext, string serverGroupId)
	{
		using IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent();
		return serverGroupComponent.GetItemFromServerGroupId(serverGroupId);
	}

	public static ServerGroup CreateServerGroup(IGeminiRequestContext requestContext, ServerGroup serverGroup)
	{
		return CreateServerGroup(requestContext, serverGroup, checkPermissions: true);
	}

	public static ServerGroup CreateServerGroup(IGeminiRequestContext requestContext, ServerGroup serverGroup, bool checkPermissions)
	{
		try
		{
			if (serverGroup.StructureType == EnumStructureType.FailoverGroup && !requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "RecordingServerFailover"))
			{
				throw new FeatureNotLicensedException();
			}
			if (serverGroup.StructureType == EnumStructureType.ViewGroup)
			{
				if (IsPrivateViewGroupRoot(serverGroup.ParentId))
				{
					PrivateViewGroupTokenCache.Clear();
				}
				if (IsNonSecurityGroup(serverGroup.ParentId))
				{
					if (IsSpecialType(serverGroup.ParentId, "430C2FE2-B233-42DA-B508-1A22EF03383D") && checkPermissions)
					{
						requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "CREATE_VIEWGROUP");
					}
				}
				else if (checkPermissions)
				{
					requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", GetRootViewGroupId(requestContext, serverGroup.ParentId));
				}
			}
			else
			{
				requestContext.GetService<SecurityManagerService>().CheckApplicationWritePermission(requestContext);
			}
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new ServerGroupNotFoundException();
		}
		if (!string.IsNullOrWhiteSpace(serverGroup.ParentId) && !IsSpecialType(serverGroup.ParentId, "430C2FE2-B233-42DA-B508-1A22EF03383D") && !IsCorrectGroupStructure(requestContext, serverGroup.ParentId, serverGroup.StructureType))
		{
			throw new ServerGroupNotFoundException();
		}
		serverGroup.Id = GeminiResourceComponent.GetGuid(serverGroup.Id);
		using (ServerGroupComponent serverGroupComponent = requestContext.CreateComponent<ServerGroupComponent>())
		{
			string structureType = ConvertFromEnumStructureTypeToStringStructureType(serverGroup.StructureType);
			serverGroupComponent.CreateServerGroup(serverGroup, structureType, requestContext.AuthenticatedUserSid);
			if (IsPrivateViewGroupRoot(serverGroup.ParentId))
			{
				serverGroup.Id = serverGroupComponent.GetIdFromName(serverGroup.ParentId, serverGroup.Name);
			}
			serverGroup = FindServerGroup(requestContext, serverGroupComponent, serverGroup.ParentId, serverGroup.Id);
		}
		if (serverGroup.StructureType == EnumStructureType.ViewGroup)
		{
			ViewGroupCache.Clear();
			Integration.FireViewGroupChanged(requestContext, serverGroup.Id, serverGroup.ParentId, EventAction.Added);
		}
		return serverGroup;
	}

	public static void UpdateServerGroupValidation(IGeminiRequestContext requestContext, string id, string name, string description)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
	}

	public static ServerGroup UpdateServerGroup(IGeminiRequestContext requestContext, ServerGroup serverGroup)
	{
		if (IsCorrectGroupStructure(requestContext, serverGroup.Id, EnumStructureType.ViewGroup))
		{
			string rootViewGroupId = GetRootViewGroupId(requestContext, serverGroup.Id);
			try
			{
				if (string.Equals(rootViewGroupId, serverGroup.Id, StringComparison.OrdinalIgnoreCase))
				{
					requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "GENERIC_WRITE", rootViewGroupId);
				}
				else
				{
					requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", rootViewGroupId);
				}
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new ServerGroupNotFoundException();
			}
		}
		else
		{
			requestContext.GetService<SecurityManagerService>().CheckApplicationWritePermission(requestContext);
		}
		serverGroup.ParentId = GetParentServerGroupId(requestContext, serverGroup.Id);
		using (ServerGroupComponent serverGroupComponent = requestContext.CreateComponent<ServerGroupComponent>())
		{
			serverGroupComponent.UpdateServerGroup(serverGroup.Id, serverGroup.Name, serverGroup.Description, serverGroup.CustomProperties);
			serverGroup = FindServerGroup(requestContext, serverGroupComponent, serverGroup.ParentId, serverGroup.Id);
		}
		if (serverGroup.StructureType == EnumStructureType.ViewGroup)
		{
			Integration.FireViewGroupChanged(requestContext, serverGroup.Id, serverGroup.ParentId, EventAction.Updated);
			ViewGroupCache.Clear();
		}
		return serverGroup;
	}

	public static void DeleteServerGroup(IGeminiRequestContext requestContext, string serverGroupId)
	{
		if (IsCorrectGroupStructure(requestContext, serverGroupId, EnumStructureType.ViewGroup))
		{
			string rootViewGroupId = GetRootViewGroupId(requestContext, serverGroupId);
			try
			{
				if (string.Equals(rootViewGroupId, serverGroupId, StringComparison.OrdinalIgnoreCase))
				{
					requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "DELETE", rootViewGroupId);
				}
				else
				{
					requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "OPERATE", rootViewGroupId);
				}
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new ServerGroupNotFoundException();
			}
		}
		else
		{
			requestContext.GetService<SecurityManagerService>().CheckApplicationWritePermission(requestContext);
		}
		ServerGroup serverGroup = null;
		using ServerGroupComponent serverGroupComponent = requestContext.CreateComponent<ServerGroupComponent>();
		serverGroup = serverGroupComponent.QueryServerGroupById(serverGroupId);
		if (serverGroup.StructureType == EnumStructureType.FailoverGroup && !requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "RecordingServerFailover"))
		{
			throw new FeatureNotLicensedException();
		}
		serverGroupComponent.DeleteServerGroup(serverGroupId);
		if (serverGroup.StructureType == EnumStructureType.ViewGroup)
		{
			Integration.FireViewGroupChanged(requestContext, serverGroup.Id, serverGroup.ParentId, EventAction.Deleted);
			ViewGroupCache.Clear();
		}
	}

	public static ServerGroup QueryPrivateViewGroup(IGeminiRequestContext requestContext, string userSid)
	{
		ServerGroup serverGroup = null;
		bool created;
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			serverGroup = serverGroupComponent.QueryPrivateViewGroup(userSid, out created);
		}
		if (created)
		{
			PrivateViewGroupTokenCache.Clear();
			IGeminiSecurityNamespace securityNamespaceFromToken = requestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(requestContext, serverGroup.Id);
			requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid).ForceNamespaceReload(requestContext);
		}
		return serverGroup;
	}

	public static bool IsNonSecurityGroup(string serverGroupId)
	{
		return new List<string>
		{
			"E0BE6096-A1D6-464e-B028-B45FF2BA28AF", "6A85FFC3-C134-4338-A072-024817CB5820", "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683", "9178D2CE-888E-4058-9992-C99D506BE63A", "430C2FE2-B233-42DA-B508-1A22EF03383D", "909749AE-88C4-4780-A381-383D691E98FA", "4BF8A8B0-54AB-4E82-994F-98D8E9584176", "7EB5B9CC-D56F-4785-BA72-4E71FADA8C6B", "14C3A703-6835-4843-B18C-8DB95436A80C", "E0199F86-4A16-4A1F-B655-8BE7B9DCA3E1",
			"9F4A3B56-9A30-44B5-9C19-5E7421164F96", "6ACA51C5-A716-48A1-A2DC-AEEADA59C771"
		}.Contains(serverGroupId);
	}

	public static bool IsViewLayoutGroup(string serverGroupId)
	{
		return new List<string> { "7EB5B9CC-D56F-4785-BA72-4E71FADA8C6B", "14C3A703-6835-4843-B18C-8DB95436A80C", "E0199F86-4A16-4A1F-B655-8BE7B9DCA3E1", "9F4A3B56-9A30-44B5-9C19-5E7421164F96" }.Contains(serverGroupId);
	}

	public static bool IsPrivateViewGroupRoot(string serverGroupId)
	{
		return serverGroupId.Equals("4BF8A8B0-54AB-4E82-994F-98D8E9584176", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsSpecialType(string serverGroupId, string specialGroupType)
	{
		return serverGroupId.Equals(specialGroupType, StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsStructureTypeViewGroup(EnumStructureType structureType)
	{
		return structureType == EnumStructureType.ViewGroup;
	}

	public static bool IsStructureTypeViewLayoutGroup(EnumStructureType structureType)
	{
		return structureType == EnumStructureType.ViewLayoutGroup;
	}

	public static bool IsStructureTypeFailoverGroup(EnumStructureType structureType)
	{
		return structureType == EnumStructureType.FailoverGroup;
	}

	public static bool IsStructureTypeMIPObjectGroup(EnumStructureType structureType)
	{
		return structureType == EnumStructureType.MIPObjectGroup;
	}

	public static bool HasPermissions(string groupStructureType)
	{
		if (new List<string> { "VIEWLAYOUT_GROUP", "FAILOVER_GROUP", "MIPOBJECT_GROUP" }.Contains(groupStructureType))
		{
			return false;
		}
		return true;
	}

	public static string GetRootViewGroupId(IGeminiRequestContext requestContext, string viewGroupId)
	{
		string text = ViewGroupCache.GetRootViewGroupIdFromViewGroupId(viewGroupId);
		if (text != null)
		{
			return text;
		}
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			text = serverGroupComponent.FindRootViewGroupId(viewGroupId);
		}
		if (text == null)
		{
			throw new InvalidOperationException("Specified group id is not a view group");
		}
		ViewGroupCache.AddToCache(viewGroupId, text);
		return text;
	}

	public static ServerGroup FilterServerGroupBasedOnPermissions(IGeminiRequestContext requestContext, ServerGroup serverGroup)
	{
		if (IsPrivateViewGroupRoot(serverGroup.Id))
		{
			return serverGroup;
		}
		if (serverGroup.StructureType == EnumStructureType.FailoverGroup || serverGroup.StructureType == EnumStructureType.MIPObjectGroup)
		{
			if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
			{
				return serverGroup;
			}
		}
		else if (serverGroup.StructureType == EnumStructureType.ViewGroup)
		{
			string rootViewGroupId = GetRootViewGroupId(requestContext, serverGroup.Id);
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.ViewGroup, "GENERIC_READ", rootViewGroupId))
			{
				return serverGroup;
			}
		}
		else
		{
			if (serverGroup.StructureType == EnumStructureType.SmartClientGroup || serverGroup.StructureType == EnumStructureType.ViewLayoutGroup)
			{
				return serverGroup;
			}
			if (serverGroup.StructureType == EnumStructureType.AccessControlGroup)
			{
				if (requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
				{
					return serverGroup;
				}
				string fullPath = requestContext.SecurityObjectMapCache.GetFullPath(requestContext, SecurityNamespaceIdentifier.AccessControlUnits, serverGroup.Id);
				if (fullPath == null)
				{
					return null;
				}
				if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.AccessControlUnits, "GENERIC_READ", fullPath))
				{
					return serverGroup;
				}
			}
		}
		return null;
	}

	public static ServerGroup[] FilterServerGroupsBasedOnPermissions(IGeminiRequestContext requestContext, ServerGroup[] serverGroups)
	{
		List<ServerGroup> list = new List<ServerGroup>();
		foreach (ServerGroup serverGroup in serverGroups)
		{
			if (FilterServerGroupBasedOnPermissions(requestContext, serverGroup) != null)
			{
				list.Add(serverGroup);
			}
		}
		return list.ToArray();
	}

	public static bool IsCorrectGroupStructure(IGeminiRequestContext requestContext, string serverGroupId, EnumStructureType structureType)
	{
		ServerGroup serverGroup = null;
		using (IServerGroupComponent serverGroupComponent = requestContext.CreateServerGroupComponent())
		{
			serverGroup = serverGroupComponent.QueryServerGroupById(serverGroupId);
		}
		return serverGroup.StructureType == structureType;
	}

	public static bool HasPrivateViewGroupInRelations(string[] serverGroupRelations)
	{
		for (int i = 0; i < serverGroupRelations.Length; i++)
		{
			if (string.Equals(serverGroupRelations[i].Trim(), "4BF8A8B0-54AB-4E82-994F-98D8E9584176", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private static ServerGroup FindServerGroup(IGeminiRequestContext requestContext, IServerGroupComponent serverGroupComponent, string parentGroupId, string serverGroupId)
	{
		bool flag = false;
		try
		{
			if (serverGroupComponent == null)
			{
				serverGroupComponent = requestContext.CreateServerGroupComponent();
				flag = true;
			}
			return serverGroupComponent.QueryServerGroupByParentGroupId(parentGroupId, serverGroupId);
		}
		finally
		{
			if (flag)
			{
				serverGroupComponent.Dispose();
			}
		}
	}

	public static string QueryPrivateUserViewGroupId(IGeminiRequestContext requestContext, string userDomainAccountName)
	{
		string privateViewGroupId = PrivateUserViewGroupIdCache.Instance.GetPrivateViewGroupId(userDomainAccountName);
		if (privateViewGroupId != null)
		{
			return privateViewGroupId;
		}
		ServerGroup serverGroup = null;
		try
		{
			serverGroup = QueryServerGroupByName(requestContext, "4BF8A8B0-54AB-4E82-994F-98D8E9584176", userDomainAccountName);
		}
		catch (ServerGroupNotFoundException)
		{
		}
		if (serverGroup != null)
		{
			PrivateUserViewGroupIdCache.Instance.AddToCache(userDomainAccountName, serverGroup.Id);
			return serverGroup.Id;
		}
		return string.Empty;
	}

	public static EnumStructureType ConvertFromEntityNameToEnumStructureType(string entityName)
	{
		if (string.Equals(entityName, "PublicViewGroup", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.ViewGroup;
		}
		if (string.Equals(entityName, "PrivateViewGroup", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.ViewGroup;
		}
		if (string.Equals(entityName, "ViewLayoutGroup", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.ViewLayoutGroup;
		}
		if (string.Equals(entityName, "FailoverServerGroup", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.FailoverGroup;
		}
		if (string.Equals(entityName, "MIPObjectGroup", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.MIPObjectGroup;
		}
		if (string.Equals(entityName, "AccessControlGroup", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.AccessControlGroup;
		}
		throw new ServerGroupNoGroupException();
	}

	public static EnumStructureType ConvertFromStringStructureTypeToEnumStructureType(string structureType)
	{
		if (string.Equals(structureType, "SMARTCLIENT_GROUP", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.SmartClientGroup;
		}
		if (string.Equals(structureType, "VIEW_GROUP", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.ViewGroup;
		}
		if (string.Equals(structureType, "VIEWLAYOUT_GROUP", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.ViewLayoutGroup;
		}
		if (string.Equals(structureType, "FAILOVER_GROUP", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.FailoverGroup;
		}
		if (string.Equals(structureType, "MIPOBJECT_GROUP", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.MIPObjectGroup;
		}
		if (string.Equals(structureType, "ACCESSCONTROL_GROUP", StringComparison.OrdinalIgnoreCase))
		{
			return EnumStructureType.AccessControlGroup;
		}
		throw new ServerGroupNoGroupException();
	}

	public static string ConvertFromEnumStructureTypeToStringStructureType(EnumStructureType structureType)
	{
		return structureType switch
		{
			EnumStructureType.SmartClientGroup => "SMARTCLIENT_GROUP", 
			EnumStructureType.ViewGroup => "VIEW_GROUP", 
			EnumStructureType.ViewLayoutGroup => "VIEWLAYOUT_GROUP", 
			EnumStructureType.FailoverGroup => "FAILOVER_GROUP", 
			EnumStructureType.MIPObjectGroup => "MIPOBJECT_GROUP", 
			EnumStructureType.AccessControlGroup => "ACCESSCONTROL_GROUP", 
			_ => throw new ServerGroupNoGroupException(), 
		};
	}
}
