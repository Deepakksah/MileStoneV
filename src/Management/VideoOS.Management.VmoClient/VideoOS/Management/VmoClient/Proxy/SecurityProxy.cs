using System;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

public sealed class SecurityProxy : ProxyClientServiceBase<SecurityServiceClient>
{
	public override string ServiceName => ResolveServiceName("SecurityService");

	public override Uri ServiceAddress => new Uri("urn:videoos:management:server:securityservice");

	public override ICommunicationObject CommunicationObject
	{
		get
		{
			if (base.ServiceClient != null)
			{
				return base.ServiceClient.ChannelFactory;
			}
			return null;
		}
	}

	public SecurityProxy(ServerConnection serverConnection)
		: base((ConnectionSettings)serverConnection)
	{
	}

	public override void Initialize()
	{
		if (base.ServiceClient != null)
		{
			try
			{
				base.ServiceClient.Close();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				base.ServiceClient.Abort();
			}
		}
		base.ServiceClient = new SecurityServiceClient(GetBinding(), GetEndpointAddress());
		ConfigureEndpoint(base.ServiceClient.Endpoint);
		ConfigureCredentials(base.ServiceClient.ClientCredentials);
		base.IsInitialized = true;
	}

	public VMOWebserviceObject[] QueryRoles()
	{
		RoleInfo[] roleInfo;
		try
		{
			roleInfo = CallWithRetry(() => base.ServiceClient.QueryRoles());
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObjects(roleInfo);
	}

	public VMOWebserviceObject QueryRoleById(string id)
	{
		RoleInfo roleInfo;
		try
		{
			roleInfo = CallWithRetry(() => base.ServiceClient.QueryRoleById(id));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(roleInfo);
	}

	public VMOWebserviceObject QueryRoleByName(string name)
	{
		RoleInfo roleInfo;
		try
		{
			roleInfo = CallWithRetry(() => base.ServiceClient.QueryRoleByName(name));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(roleInfo);
	}

	public VMOWebserviceObject CreateRole(VMOWebserviceObject roleInfo)
	{
		RoleInfo roleInfo2 = null;
		try
		{
			RoleInfo info = ConvertToRoleInfo(roleInfo);
			roleInfo2 = CallWithRetry(() => base.ServiceClient.CreateRole(info.Name, info.Description));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(roleInfo2);
	}

	public VMOWebserviceObject UpdateRole(VMOWebserviceObject roleInfo)
	{
		RoleInfo roleInfo2 = null;
		try
		{
			RoleInfo info = ConvertToRoleInfo(roleInfo);
			roleInfo2 = CallWithRetry(() => base.ServiceClient.UpdateRole(info.Id, info.Name, info.Description));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(roleInfo2);
	}

	public void DeleteRole(string roleId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteRole(roleId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId)
	{
		SecurityNamespaceInfo[] array = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.QuerySecurityNamespaces(namespaceId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public AccessControlEntry[] QueryPermissions(string namespaceId, string securityToken, string identity)
	{
		AccessControlEntry[] array = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryPermissions(namespaceId, securityToken, identity));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void AddPermissions(string namespaceId, string securityToken, AccessControlEntry[] entries)
	{
		try
		{
			base.ServiceClient.AddPermissions(namespaceId, securityToken, entries);
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void RemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] entries)
	{
		try
		{
			base.ServiceClient.RemovePermissions(namespaceId, securityToken, entries);
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void AddRemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries)
	{
		try
		{
			base.ServiceClient.AddRemovePermissions(namespaceId, securityToken, addEntries, removeEntries);
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public Identity ReadIdentity(SearchFactor searchFactor, string factoryValue, QueryMembership queryMembership)
	{
		Identity[] array;
		try
		{
			array = CallWithRetry(() => base.ServiceClient.ReadIdentities(searchFactor, new string[1] { factoryValue }, queryMembership));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		if (array != null && array.Length != 0)
		{
			return array[0];
		}
		return null;
	}

	public Identity[] ReadIdentities(SearchFactor searchFactor, string[] factoryValues, QueryMembership queryMembership)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.ReadIdentities(searchFactor, factoryValues, queryMembership));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public Identity ReadIdentityFromSource(SearchFactor searchFactor, string factorValue)
	{
		Identity identity = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.ReadIdentityFromSource(searchFactor, factorValue));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void RemoveIdentity(string userSid)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.RemoveIdentity(userSid);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public Identity[] QueryRoleMembers(string roleId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryRoleMembers(roleId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryRoleMembersExtended(roleId, queryMembership, includeMappedExternalUsers));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public VMOWebserviceObject CopyRole(string sourceRoleId, string name, string description)
	{
		RoleInfo roleInfo = null;
		try
		{
			roleInfo = CallWithRetry(() => base.ServiceClient.CopyRole(sourceRoleId, name, description));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
		return ConvertToVMOWebserviceObject(roleInfo);
	}

	public Identity AddMemberToRole(string roleId, string memberSid)
	{
		Identity identity = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.AddMemberToRole(roleId, memberSid));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void RemoveMemberFromRole(string roleId, string memberSid)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.RemoveMemberFromRole(roleId, memberSid);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public SecurityAction[] QueryObjectClassActions(string classId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryObjectClassActions(classId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void RegisterObject(string objectId, string classId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.RegisterSecurityObject(objectId, classId, null);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void UnregisterObject(string objectId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.UnregisterSecurityObject(objectId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void CheckPermission(string objectId, string actionId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.CheckPermission(objectId, actionId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public AccessControlEntry[] QueryAccessControlList(string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryAccessControlList(objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public AccessControlEntry[] QueryAccessControlList(string identity, string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryAccessControlListByIdentity(identity, objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void AddAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.AddAccessControlEntries(objectId, entries);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void CreateSecurityNamespace(SecurityNamespaceInfo info, string orderAfterNamespaceId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.CreateSecurityNamespace(info, orderAfterNamespaceId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void UpdateSecurityNamespace(SecurityNamespaceInfo info)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.UpdateSecurityNamespace(info);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void DeleteSecurityNamespace(string namespaceId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteSecurityNamespace(namespaceId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.RemoveAccessControlEntries(objectId, entries);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.ReplaceAccessControlEntries(objectId, entries);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void SetInheritance(string objectId, string parentObjectId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.SetInheritance(objectId, parentObjectId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.IsPermittedByIdentityList(identityList, actionId, objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.IsPermittedByActionList(identity, actionIdList, objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.IsPermittedByObjectList(identity, actionId, objectIdList));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool IsPermittedTimeBasedAction(string actionId, string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.IsPermittedTimeBasedAction(actionId, objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public AccessControlTimeProfileEntry QueryTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		AccessControlTimeProfileEntry accessControlTimeProfileEntry = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.QueryAccessControlTimeProfileEntry(objectId, identity, actionId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public AccessControlTimeProfileEntry CreateUpdateTimeBasedAccessEntry(string identity, string actionId, string objectId, string timeProfileId, bool isDefaultTimeProfile)
	{
		AccessControlTimeProfileEntry accessControlTimeProfileEntry = null;
		try
		{
			return CallWithRetry(() => base.ServiceClient.AddUpdateAccessControlTimeProfileEntry(objectId, identity, actionId, timeProfileId, isDefaultTimeProfile));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public void DeleteTimeBasedAccessEntry(string identity, string actionId, string objectId)
	{
		try
		{
			CallWithRetry(delegate
			{
				base.ServiceClient.DeleteAccessControlTimeProfile(objectId, identity, actionId);
			});
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.HasPermissionByIdentityList(namespaceId, identityList, actionId, objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.HasPermissionByActionList(namespaceId, identity, actionList, objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.HasPermissionByActionListCurrentUser(namespaceId, actionList, objectId));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	public bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList)
	{
		try
		{
			return CallWithRetry(() => base.ServiceClient.HasPermissionByObjectList(namespaceId, identity, actionId, objectList));
		}
		catch (Exception ex)
		{
			ThrowConvertedExceptionIfNeeded(ex);
			throw;
		}
	}

	private static VMOWebserviceObject ConvertToVMOWebserviceObject(RoleInfo roleInfo)
	{
		if (roleInfo == null)
		{
			return null;
		}
		return new VMOWebserviceObject(new VmoPropertyCollection
		{
			{ "Id", roleInfo.Id },
			{ "Name", roleInfo.Name },
			{ "Description", roleInfo.Description },
			{ "LastModified", roleInfo.LastModified },
			{
				"RoleType",
				roleInfo.Type.ToString()
			}
		}, new VmoRelationCollection(), roleInfo.Id, "Role");
	}

	private static VMOWebserviceObject[] ConvertToVMOWebserviceObjects(RoleInfo[] roleInfo)
	{
		List<VMOWebserviceObject> list = new List<VMOWebserviceObject>();
		foreach (RoleInfo roleInfo2 in roleInfo)
		{
			list.Add(ConvertToVMOWebserviceObject(roleInfo2));
		}
		return list.ToArray();
	}

	private static RoleInfo ConvertToRoleInfo(VMOWebserviceObject vmoWebserviceObject)
	{
		RoleInfo roleInfo = new RoleInfo();
		roleInfo.Id = vmoWebserviceObject.Id;
		if (vmoWebserviceObject.Properties.ContainsKey("Id"))
		{
			roleInfo.Id = vmoWebserviceObject.Properties.GetPropertyValue<string>("Id");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("Name"))
		{
			roleInfo.Name = vmoWebserviceObject.Properties.GetPropertyValue<string>("Name");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("Description"))
		{
			roleInfo.Description = vmoWebserviceObject.Properties.GetPropertyValue<string>("Description");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("RoleType"))
		{
			roleInfo.Type = vmoWebserviceObject.Properties.GetPropertyValue<RoleType>("RoleType");
		}
		if (vmoWebserviceObject.Properties.ContainsKey("LastModified"))
		{
			roleInfo.LastModified = vmoWebserviceObject.Properties.GetPropertyValue<DateTime>("LastModified");
		}
		return roleInfo;
	}
}
