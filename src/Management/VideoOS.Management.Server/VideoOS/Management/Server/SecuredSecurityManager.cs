using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.Server;

public class SecuredSecurityManager : IGeminiServerService
{
	private SecurityManagerService _securityManagerService;

	public List<SecurityNamespaceInfo> QuerySecurityNamespaces(IGeminiRequestContext requestContext, string namespaceId)
	{
		_securityManagerService.CheckApplicationReadPermission(requestContext);
		List<SecurityNamespaceInfo> list = new List<SecurityNamespaceInfo>();
		if (string.IsNullOrEmpty(namespaceId))
		{
			list = (from e in _securityManagerService.GetSecurityNamespaces(requestContext)
				select e.NamespaceInfo).ToList();
		}
		else
		{
			IGeminiSecurityNamespace securityNamespace = _securityManagerService.GetSecurityNamespace(requestContext, new Guid(namespaceId));
			if (securityNamespace != null)
			{
				list.Add(securityNamespace.NamespaceInfo);
			}
		}
		List<SecurityNamespaceInfo> list2 = new List<SecurityNamespaceInfo>();
		foreach (SecurityNamespaceInfo item in list)
		{
			if ((!(item.NamespaceGuid == SecurityNamespaceIdentifier.SmartWall) || GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartWall")) && (!(item.NamespaceGuid == SecurityNamespaceIdentifier.Site) || GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "FederatedSites")) && (!(item.NamespaceGuid == SecurityNamespaceIdentifier.SystemMonitor) || GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SystemMonitor")) && (!(item.NamespaceGuid == SecurityNamespaceIdentifier.SaveSearch) || GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SaveSearches")))
			{
				list2.Add(FilterActions(requestContext, item));
			}
		}
		return list2;
	}

	private static SecurityNamespaceInfo FilterActions(IGeminiRequestContext requestContext, SecurityNamespaceInfo namespaceInfo)
	{
		SecurityNamespaceInfo securityNamespaceInfo = new SecurityNamespaceInfo();
		securityNamespaceInfo.DisplayName = namespaceInfo.DisplayName;
		securityNamespaceInfo.Name = namespaceInfo.Name;
		securityNamespaceInfo.NamespaceId = namespaceInfo.NamespaceId;
		securityNamespaceInfo.Separator = namespaceInfo.Separator;
		securityNamespaceInfo.Structure = namespaceInfo.Structure;
		securityNamespaceInfo.UserDefined = namespaceInfo.UserDefined;
		List<ActionDefinition> list = new List<ActionDefinition>();
		foreach (ActionDefinition action in namespaceInfo.Actions)
		{
			if ((GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "DifferentiatedAdministratorSecurity") || action.Visible) && IsActionSupported(requestContext, action.NamespaceGuid, action.Name))
			{
				list.Add(action);
			}
		}
		securityNamespaceInfo.Actions = list;
		return securityNamespaceInfo;
	}

	private static bool IsActionSupportedForCameraMicrophoneSpeakerMetadata(IGeminiRequestContext requestContext, string actionName)
	{
		if (actionName.Equals("LIFT_PRIVACY_MASKING", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "ExtendedPrivacyMasking");
		}
		if (actionName.Equals("CREATE_BOOKMARKS", StringComparison.OrdinalIgnoreCase) || actionName.Equals("DELETE_BOOKMARKS", StringComparison.OrdinalIgnoreCase) || actionName.Equals("EDIT_BOOKMARKS", StringComparison.OrdinalIgnoreCase) || actionName.Equals("READ_BOOKMARKS", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "Bookmarking");
		}
		if (actionName.Equals("CREATE_EVIDENCE_LOCK", StringComparison.OrdinalIgnoreCase) || actionName.Equals("DELETE_EVIDENCE_LOCK", StringComparison.OrdinalIgnoreCase) || actionName.Equals("READ_EVIDENCE_LOCK", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock");
		}
		if (actionName.Equals("READ_REMOTE_RECORDINGS", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EdgeStorage");
		}
		if (actionName.Equals("RESERVE_PTZ_SESSION", StringComparison.OrdinalIgnoreCase) || actionName.Equals("RELEASE_PTZ_SESSION", StringComparison.OrdinalIgnoreCase) || actionName.Equals("MANAGE_LOCK_PTZ_PRESET", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "ExtendedPtz");
		}
		if (actionName.Equals("CREATE_RESTRICTIONS", StringComparison.OrdinalIgnoreCase) || actionName.Equals("READ_RESTRICTIONS", StringComparison.OrdinalIgnoreCase) || actionName.Equals("DELETE_RESTRICTIONS", StringComparison.OrdinalIgnoreCase) || actionName.Equals("VIEW_RESTRICTED_LIVE", StringComparison.OrdinalIgnoreCase) || actionName.Equals("LISTEN_RESTRICTED_LIVE", StringComparison.OrdinalIgnoreCase) || actionName.Equals("PLAYBACK_RESTRICTED_RECORDINGS", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "RestrictedMedia");
		}
		return true;
	}

	private static bool IsActionSupportedForManagementServer(IGeminiRequestContext requestContext, string actionName)
	{
		if (actionName.Equals("ADMIN_FEDERATED_HIERARCHY", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "FederatedSites");
		}
		if (actionName.Equals("AUTHORIZE_USER", StringComparison.OrdinalIgnoreCase))
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "DualAuthorization");
		}
		return true;
	}

	private static bool IsActionSupported(IGeminiRequestContext requestContext, Guid namespaceId, string actionName)
	{
		if (namespaceId == SecurityNamespaceIdentifier.Camera || namespaceId == SecurityNamespaceIdentifier.Microphone || namespaceId == SecurityNamespaceIdentifier.Speaker || namespaceId == SecurityNamespaceIdentifier.Metadata)
		{
			return IsActionSupportedForCameraMicrophoneSpeakerMetadata(requestContext, actionName);
		}
		if (namespaceId == SecurityNamespaceIdentifier.ManagementServer)
		{
			return IsActionSupportedForManagementServer(requestContext, actionName);
		}
		return true;
	}

	public void CreateSecurityNamespace(IGeminiRequestContext requestContext, SecurityNamespaceInfo description, string orderAfterNamespaceId)
	{
		_securityManagerService.CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "ADMIN_SECURITY");
		_securityManagerService.CreateSecurityNamespace(requestContext, description, orderAfterNamespaceId);
	}

	public void UpdateSecurityNamespace(IGeminiRequestContext requestContext, SecurityNamespaceInfo description)
	{
		_securityManagerService.CheckPermission(requestContext, description.NamespaceGuid, "ADMIN_SECURITY");
		_securityManagerService.UpdateSecurityNamespace(requestContext, description);
	}

	public void DeleteSecurityNamespace(IGeminiRequestContext requestContext, string namespaceId)
	{
		_securityManagerService.CheckPermission(requestContext, new Guid(namespaceId), "ADMIN_SECURITY");
		_securityManagerService.DeleteSecurityNamespace(requestContext, new Guid(namespaceId));
	}

	public void RegisterObject(IGeminiRequestContext requestContext, string namespaceId, string objectId, string parentObjectId)
	{
		_securityManagerService.CheckPermission(requestContext, new Guid(namespaceId), "GENERIC_WRITE", objectId);
		_securityManagerService.RegisterSecurityObject(requestContext, new Guid(namespaceId), objectId, parentObjectId);
	}

	public void UnregisterObject(IGeminiRequestContext requestContext, string namespaceId, string objectId)
	{
		_securityManagerService.CheckPermission(requestContext, new Guid(namespaceId), "GENERIC_WRITE", objectId);
		_securityManagerService.UnregisterSecurityObject(requestContext, new Guid(namespaceId), objectId);
	}

	public AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, string namespaceId, string securityToken)
	{
		_securityManagerService.CheckPermission(requestContext, new Guid(namespaceId), new string[2] { "GENERIC_READ", "ADMIN_SECURITY" }, securityToken);
		return _securityManagerService.QueryPermissions(requestContext, new Guid(namespaceId), securityToken);
	}

	public AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, string namespaceId, string securityToken, string identity)
	{
		_securityManagerService.CheckPermission(requestContext, new Guid(namespaceId), new string[2] { "GENERIC_READ", "ADMIN_SECURITY" }, securityToken);
		return _securityManagerService.QueryPermissions(requestContext, new Guid(namespaceId), securityToken, identity);
	}

	public void AddPermissions(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string securityToken, string objectName, AccessControlEntry[] entries)
	{
		if (!requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext))
		{
			string roleId = entries?.First()?.Sid;
			CheckPermissionAndDoAuditLog(requestContext, securityNamespace, securityToken, roleId, objectName);
		}
		_securityManagerService.AddPermissions(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, securityToken, objectName, entries);
	}

	public void RemovePermissions(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string securityToken, string objectName, AccessControlEntry[] entries)
	{
		if (!requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext))
		{
			string roleId = entries?.First()?.Sid;
			CheckPermissionAndDoAuditLog(requestContext, securityNamespace, securityToken, roleId, objectName);
		}
		_securityManagerService.RemovePermissions(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, securityToken, objectName, entries);
	}

	public void AddRemovePermissions(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string securityToken, string objectName, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries)
	{
		if (!requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext))
		{
			string text = addEntries?.First()?.Sid;
			if (text == null)
			{
				text = removeEntries?.First()?.Sid;
			}
			CheckPermissionAndDoAuditLog(requestContext, securityNamespace, securityToken, text, objectName);
		}
		_securityManagerService.AddRemovePermissions(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, securityToken, objectName, addEntries, removeEntries);
	}

	public void ReplacePermissions(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string securityToken, string objectName, AccessControlEntry[] entries)
	{
		if (!requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext))
		{
			string roleId = entries?.First()?.Sid;
			CheckPermissionAndDoAuditLog(requestContext, securityNamespace, securityToken, roleId, objectName);
		}
		_securityManagerService.ReplacePermissions(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, securityToken, objectName, entries);
	}

	public static void FailedPermissionAuditLog(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string securityToken, Guid roleId, string roleName, string objectName)
	{
		string namespaceName = ((securityNamespace.NamespaceInfo.NamespaceGuid == SecurityNamespaceIdentifier.SmartWall) ? OemResources.Instance.VideoWall.ApplicationNameShort : ((!(securityNamespace.NamespaceInfo.NamespaceGuid == SecurityNamespaceIdentifier.Matrix)) ? securityNamespace.NamespaceInfo.DisplayName : OemResources.Instance.Manager.MatrixApplicationName));
		if (roleName == null)
		{
			return;
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		if (securityToken.Equals("/", StringComparison.Ordinal))
		{
			vmoLogManager.LogEntryAuditLogger.AuditRoleOverallSecurityChangeAccessDenied(roleId, roleName, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress, namespaceName);
		}
		else
		{
			vmoLogManager.LogEntryAuditLogger.AuditRoleObjectSecurityChangeAccessDenied(roleId, roleName, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress, namespaceName, objectName);
		}
	}

	private void CheckPermissionAndDoAuditLog(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string securityToken, string roleId, string objectName)
	{
		if ((securityNamespace.HasAction("ADMIN_SECURITY") && !_securityManagerService.HasPermission(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, "ADMIN_SECURITY", securityToken)) || !_securityManagerService.HasPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "CONNECT", "/"))
		{
			if (roleId != null && Guid.TryParse(roleId, out var result))
			{
				string roleName = requestContext.GetService<NameLookupService>().GetRoleName(requestContext, result);
				FailedPermissionAuditLog(requestContext, securityNamespace, securityToken, result, roleName, objectName);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
	}

	public AccessControlTimeProfileEntry AddUpdateAccessControlTimeProfileEntry(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string objectId, string objectName, string identity, string actionId, string timeProfileId, bool isDefaultTimeProfile)
	{
		if (!requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext))
		{
			CheckPermissionAndDoAuditLog(requestContext, securityNamespace, objectId, identity, objectName);
		}
		return _securityManagerService.AddUpdateAccessControlTimeProfileEntry(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, objectId, objectName, identity, actionId, timeProfileId, isDefaultTimeProfile);
	}

	public void DeleteAccessControlTimeProfileEntry(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string objectId, string objectName, string identity, string actionId)
	{
		if (!requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext))
		{
			CheckPermissionAndDoAuditLog(requestContext, securityNamespace, objectId, identity, objectName);
		}
		_securityManagerService.DeleteAccessControlTimeProfileEntry(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, objectId, objectName, identity, actionId);
	}

	public void UpdateDeviceTimeProfilePermissions(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, List<AccessControlDeviceTimeProfileEntry> entries)
	{
		List<(AccessControlDeviceTimeProfileEntry, string)> list = new List<(AccessControlDeviceTimeProfileEntry, string)>();
		Guid? guid = null;
		string text = null;
		Guid? guid2 = null;
		string roleName = null;
		if (!requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext))
		{
			bool flag = _securityManagerService.HasPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "CONNECT", "/");
			bool flag2 = securityNamespace.HasAction("ADMIN_SECURITY");
			foreach (AccessControlDeviceTimeProfileEntry entry in entries)
			{
				if (!guid.HasValue || guid.Value != entry.DeviceId)
				{
					guid = entry.DeviceId;
					text = requestContext.GetNameLookupService().GetObjectName(requestContext, securityNamespace.NamespaceInfo.NamespaceId, entry.DeviceId.ToString());
				}
				if (!guid2.HasValue || guid2.Value != entry.RoleId)
				{
					guid2 = entry.RoleId;
					roleName = requestContext.GetNameLookupService().GetRoleName(requestContext, entry.RoleId);
				}
				if (!flag || (flag2 && !_securityManagerService.HasPermission(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, "ADMIN_SECURITY", entry.DeviceId.ToString())))
				{
					FailedPermissionAuditLog(requestContext, securityNamespace, entry.DeviceId.ToString(), entry.RoleId, roleName, text);
					throw new UnauthorizedAccessException(VmoResources.VMO61008);
				}
				list.Add((entry, text));
			}
		}
		else
		{
			foreach (AccessControlDeviceTimeProfileEntry entry2 in entries)
			{
				text = requestContext.GetNameLookupService().GetObjectName(requestContext, securityNamespace.NamespaceInfo.NamespaceId, entry2.DeviceId.ToString());
				list.Add((entry2, text));
			}
		}
		_securityManagerService.UpdateDeviceTimeProfilePermissions(requestContext, securityNamespace.NamespaceInfo.NamespaceGuid, list);
	}

	public void SetInheritance(IGeminiRequestContext requestContext, string namespaceId, string securityToken, string parentObjectId)
	{
		_securityManagerService.CheckPermission(requestContext, new Guid(namespaceId), "ADMIN_SECURITY", securityToken);
		_securityManagerService.SetInheritance(requestContext, new Guid(namespaceId), securityToken, parentObjectId);
	}

	public bool HasPermission(IGeminiRequestContext requestContext, string namespaceId, string identity, string actionId, string objectId)
	{
		_securityManagerService.CheckPermission(requestContext, new Guid(namespaceId), "GENERIC_READ", objectId);
		return _securityManagerService.HasPermission(requestContext, new Guid(namespaceId), identity, actionId, objectId);
	}

	public bool HasPermission(IGeminiRequestContext requestContext, string namespaceId, string actionId, string objectId)
	{
		return _securityManagerService.HasPermission(requestContext, new Guid(namespaceId), requestContext.AuthenticatedUserSid, actionId, objectId);
	}

	public void RemoveIdentity(IGeminiRequestContext requestContext, string memberSid)
	{
		_securityManagerService.CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "ADMIN_SECURITY");
		_securityManagerService.RemoveIdentity(requestContext, memberSid);
	}

	public string[] QueryObjectClasses(IGeminiRequestContext requestContext)
	{
		_securityManagerService.CheckApplicationReadPermission(requestContext);
		return _securityManagerService.QueryObjectClasses(requestContext);
	}

	public SecurityAction[] QueryObjectClassActions(IGeminiRequestContext requestContext, string objectClassId)
	{
		_securityManagerService.CheckApplicationReadPermission(requestContext);
		return _securityManagerService.QueryObjectClassActions(requestContext, objectClassId);
	}

	public void CreateObjectClass(IGeminiRequestContext requestContext, string objectClassId, string[] classActions)
	{
		requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_WRITE");
		_securityManagerService.CreateObjectClass(requestContext, objectClassId, classActions);
	}

	public void DeleteObjectClass(IGeminiRequestContext requestContext, string objectClassId)
	{
		requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_WRITE");
		_securityManagerService.DeleteObjectClass(requestContext, objectClassId);
	}

	public void StartService(IGeminiRequestContext systemRequestContext)
	{
		GeminiValidation.CheckForNull(systemRequestContext, "systemRequestContext");
		_securityManagerService = systemRequestContext.GetService<SecurityManagerService>();
	}

	public void StopService(IGeminiRequestContext systemRequestContext)
	{
	}
}
