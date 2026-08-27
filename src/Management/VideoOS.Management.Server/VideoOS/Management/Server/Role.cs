using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Utilities;

namespace VideoOS.Management.Server;

public sealed class Role
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public RoleType Type { get; set; }

	public bool DualAuthorizationRequired { get; set; }

	public bool ShowPtzReservedNameAsAnonymous { get; set; }

	public bool AllowMobileClientLogOn { get; set; }

	public bool AllowSmartClientLogOn { get; set; }

	public bool AllowWebClientLogOn { get; set; }

	public DateTime LastModified { get; set; }

	public static Role[] QueryRoles(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryRoles();
	}

	public static Role QueryRoleById(IGeminiRequestContext requestContext, string roleId)
	{
		requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_READ");
		return QueryRoleByIdNoSecurity(requestContext, roleId);
	}

	public static Role QueryRoleByIdNoSecurity(IGeminiRequestContext requestContext, string roleId)
	{
		Validation.CheckObjectId(ref roleId, "roleId", allowNullOrEmpty: false);
		return FindRoleById(requestContext, null, roleId);
	}

	public static Role QueryRoleByName(IGeminiRequestContext requestContext, string name)
	{
		Validation.CheckName(name, "name");
		requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_READ");
		return FindRoleByName(requestContext, null, name);
	}

	public static Role[] QueryRolesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryRolesByIds(ids);
	}

	public static string[] QueryRoleIds(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryRoleIds();
	}

	public static string[] QueryRoleIdsByClientProfileId(IGeminiRequestContext requestContext, string parentId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryRoleIdsByClientProfileId(parentId);
	}

	public static Role CreateRole(IGeminiRequestContext requestContext, string name, string description, bool dualAuthorizationRequired, bool showPtzReservedNameAsAnonymous, bool allowMobileClientLogOn, bool allowSmartClientLogOn, bool allowWebClientLogOn)
	{
		Validation.CheckName(name, "name");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		ValidateLicenses(requestContext, ref dualAuthorizationRequired, ref showPtzReservedNameAsAnonymous, ref allowMobileClientLogOn, ref allowSmartClientLogOn, ref allowWebClientLogOn);
		Role role = new Role();
		role.Id = GeminiResourceComponent.GetGuid();
		role.Name = Character.RemoveInvalidXmlChars(name);
		role.Description = Character.RemoveInvalidXmlChars(description);
		role.ShowPtzReservedNameAsAnonymous = showPtzReservedNameAsAnonymous;
		role.DualAuthorizationRequired = dualAuthorizationRequired;
		role.AllowMobileClientLogOn = allowMobileClientLogOn;
		role.AllowSmartClientLogOn = allowSmartClientLogOn;
		role.AllowWebClientLogOn = allowWebClientLogOn;
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			string roleId = roleComponent.CreateRole(role);
			role = FindRoleById(requestContext, roleComponent, roleId);
			Integration.TryNotifyServerAsync();
		}
		Integration.FireConfigurationChanged(requestContext, "Role", role.Id, EventAction.Added, role.Name);
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditRoleCreated(new Guid(role.Id), role.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return role;
	}

	private static void ValidateLicenses(IGeminiRequestContext requestContext, ref bool dualAuthorizationRequired, ref bool showPtzReservedNameAsAnonymous, ref bool allowMobileClientLogOn, ref bool allowSmartClientLogOn, ref bool allowWebClientLogOn)
	{
		ValidateLicense(requestContext, "DualAuthorization", ref dualAuthorizationRequired, defaultValue: false);
		ValidateLicense(requestContext, "ExtendedPtz", ref showPtzReservedNameAsAnonymous, defaultValue: false);
		ValidateLicense(requestContext, "MobileLogin", ref allowMobileClientLogOn, defaultValue: false);
		ValidateLicense(requestContext, "SmartClientLogin", ref allowSmartClientLogOn, defaultValue: true);
		ValidateLicense(requestContext, "MobileLogin", ref allowWebClientLogOn, defaultValue: false);
	}

	private static void ValidateLicense(IGeminiRequestContext requestContext, string featureName, ref bool property, bool defaultValue)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, featureName))
		{
			property = defaultValue;
		}
	}

	public static bool RoleExists(IGeminiRequestContext requestContext, string roleId)
	{
		Validation.CheckObjectId(ref roleId, "roleId", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			return false;
		}
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryRoleById(roleId) != null;
	}

	public static Role UpdateRole(IGeminiRequestContext requestContext, string roleId, string name, string description, bool? dualAuthorizationRequired = null, bool? showPtzReservedNameAsAnonymous = null, bool? allowMobileClientLogOn = null, bool? allowSmartClientLogOn = null, bool? allowWebClientLogOn = null)
	{
		Validation.CheckObjectId(ref roleId, "role", allowNullOrEmpty: false);
		Validation.CheckName(name, "name");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Guid sessionId = Guid.NewGuid();
		ValidateLicenses(requestContext, ref dualAuthorizationRequired, ref showPtzReservedNameAsAnonymous, ref allowMobileClientLogOn, ref allowSmartClientLogOn, ref allowWebClientLogOn);
		Role role;
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.UpdateRole(roleId, Character.RemoveInvalidXmlChars(name), Character.RemoveInvalidXmlChars(description), dualAuthorizationRequired, showPtzReservedNameAsAnonymous, allowMobileClientLogOn, allowSmartClientLogOn, allowWebClientLogOn, sessionId);
			role = FindRoleById(requestContext, roleComponent, roleId);
			Integration.TryNotifyServerAsync();
		}
		Integration.FireConfigurationChanged(requestContext, "Role", role.Id, EventAction.Updated, role.Name);
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return role;
	}

	private static void ValidateLicenses(IGeminiRequestContext requestContext, ref bool? dualAuthorizationRequired, ref bool? showPtzReservedNameAsAnonymous, ref bool? allowMobileClientLogOn, ref bool? allowSmartClientLogOn, ref bool? allowWebClientLogOn)
	{
		ValidateLicense(requestContext, "DualAuthorization", ref dualAuthorizationRequired, null);
		ValidateLicense(requestContext, "ExtendedPtz", ref showPtzReservedNameAsAnonymous, null);
		ValidateLicense(requestContext, "MobileLogin", ref allowMobileClientLogOn, null);
		ValidateLicense(requestContext, "SmartClientLogin", ref allowSmartClientLogOn, null);
		ValidateLicense(requestContext, "MobileLogin", ref allowWebClientLogOn, null);
	}

	private static void ValidateLicense(IGeminiRequestContext requestContext, string featureName, ref bool? property, bool? defaultValue)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, featureName))
		{
			property = defaultValue;
		}
	}

	public static bool AllowClientLogOn(IGeminiRequestContext requestContext, string userSid, ClientType clientType)
	{
		if (userSid == null)
		{
			return false;
		}
		if (!userSid.Equals(requestContext.AuthenticatedUserSid, StringComparison.OrdinalIgnoreCase))
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		}
		Role[] source;
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			source = roleComponent.QueryRolesByUserSid(userSid);
		}
		return clientType switch
		{
			ClientType.MobileClient => source.Any((Role f) => f.AllowMobileClientLogOn), 
			ClientType.WebClient => source.Any((Role f) => f.AllowWebClientLogOn), 
			ClientType.SmartClient => source.Any((Role f) => f.AllowSmartClientLogOn), 
			_ => false, 
		};
	}

	public static Role CopyRole(IGeminiRequestContext requestContext, string sourceRoleId, string name, string description)
	{
		Validation.CheckObjectId(ref sourceRoleId, "role", allowNullOrEmpty: false);
		Validation.CheckName(name, "name");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Role role = new Role();
		role.Id = GeminiResourceComponent.GetGuid();
		role.Name = Character.RemoveInvalidXmlChars(name);
		role.Description = Character.RemoveInvalidXmlChars(description);
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			role.Id = roleComponent.CopyRole(role, sourceRoleId);
			role = FindRoleById(requestContext, roleComponent, role.Id);
			Integration.TryNotifyServerAsync();
			Integration.FireConfigurationChanged(requestContext, "Role", role.Id, EventAction.Added, role.Name);
			IdentityCache.ClearMemoryCache();
			requestContext.GetSecurityManagerService().ForceReloadOfAllNamespaces(requestContext);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditRoleCreated(new Guid(role.Id), role.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return role;
	}

	public static void DeleteRole(IGeminiRequestContext requestContext, string roleId)
	{
		Validation.CheckObjectId(ref roleId, "roleId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Guid sessionId = Guid.NewGuid();
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.DeleteRole(roleId, sessionId);
			Integration.TryNotifyServerAsync();
			Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Deleted, string.Empty);
			IdentityCache.ClearMemoryCache();
			requestContext.GetSecurityManagerService().ForceReloadOfAllNamespaces(requestContext);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static Identity[] QueryRoleMembersExtended(IGeminiRequestContext requestContext, string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers)
	{
		Validation.CheckObjectId(ref roleId, "roleId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		List<Identity> list = new List<Identity>();
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			string[] array = roleComponent.QueryRoleMembers(roleId, includeMappedExternalUsers);
			foreach (string sid in array)
			{
				Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, sid, queryMembership);
				if (identity != null)
				{
					list.Add(identity);
				}
			}
		}
		return list.ToArray();
	}

	public static Identity AddMemberToRole(IGeminiRequestContext requestContext, string roleId, string memberSid)
	{
		Validation.CheckObjectId(ref roleId, "roleId", allowNullOrEmpty: false);
		SecurityValidation.CheckSid(ref memberSid, "memberSid");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		if (roleId.Equals(Guid.Empty))
		{
			throw new AddMemberRoleMissingException();
		}
		if (string.IsNullOrEmpty(memberSid))
		{
			throw new AddMemberIdentityMissingException();
		}
		Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, memberSid, QueryMembership.Direct);
		if (identity == null)
		{
			identity = IdentityAccessor.ReadIdentityFromSource(requestContext, memberSid);
		}
		if (identity == null)
		{
			throw new AddMemberIdentityMissingException();
		}
		Guid sessionId = Guid.NewGuid();
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.AddIdentityToRole(roleId, identity, isDynamic: false, sessionId);
			if (identity.Type == IdentityType.WindowsGroup)
			{
				requestContext.GetGeminiIdentityService().SynchronizeWindowsGroup(identity);
			}
		}
		IdentityCache.ClearMemoryCache();
		requestContext.GetSecurityManagerService().ForceReloadOfAllNamespaces(requestContext);
		if (roleId.Equals(SecurityManager.AdministratorRoleId.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			Integration.TryNotifyServerAsync();
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Updated, string.Empty);
		return identity;
	}

	public static void AddClaimToRole(IGeminiRequestContext requestContext, string roleId, string claimProvider, string claimName, string claimValue)
	{
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		Validation.CheckGuidValue(claimProvider, "claimProvider");
		Validation.CheckStringNotNullOrWhiteSpace(claimName, "claimName");
		Validation.CheckStringNotNullOrWhiteSpace(claimValue, "claimValue");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Guid sessionId = Guid.NewGuid();
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.AddClaimToRole(roleId, claimProvider, claimName, claimValue, sessionId);
			ClaimToRoleMapper.ClaimAddedToRole(requestContext, new Guid(claimProvider), claimName);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Updated, string.Empty);
	}

	public static void RemoveClaimFromRole(IGeminiRequestContext requestContext, string roleId, string claimProvider, string claimName, string claimValue)
	{
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		Validation.CheckStringNotNullOrWhiteSpace(claimProvider, "claimProvider");
		Validation.CheckStringNotNullOrWhiteSpace(claimName, "claimName");
		Validation.CheckStringNotNullOrWhiteSpace(claimValue, "claimValue");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Guid sessionId = Guid.NewGuid();
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.RemoveClaimFromRole(roleId, claimProvider, claimName, claimValue, sessionId);
			ClaimToRoleMapper.ClaimRemovedFromRole(requestContext, roleId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Updated, string.Empty);
	}

	public static Claim[] QueryClaimsForRole(IGeminiRequestContext requestContext, string roleId)
	{
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryClaimsForRole(roleId);
	}

	public static void RemoveMemberFromRole(IGeminiRequestContext requestContext, string roleId, string memberSid)
	{
		Validation.CheckObjectId(ref roleId, "roleId", allowNullOrEmpty: false);
		SecurityValidation.CheckSid(ref memberSid, "memberSid");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		if (roleId.Equals(Guid.Empty))
		{
			throw new RemoveMemberRoleMissingException();
		}
		if (string.IsNullOrEmpty(memberSid))
		{
			throw new RemoveMemberIdentityMissingException();
		}
		Guid sessionId = Guid.NewGuid();
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.RemoveIdentityFromRole(roleId, memberSid, removeDynamic: false, sessionId);
		}
		IdentityCache.ClearMemoryCache();
		requestContext.GetSecurityManagerService().ForceReloadOfAllNamespaces(requestContext);
		if (roleId.Equals(SecurityManager.AdministratorRoleId.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			Integration.TryNotifyServerAsync();
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Updated, string.Empty);
	}

	public static Role FindRoleById(IGeminiRequestContext requestContext, IRoleComponent roleComponent, string roleId)
	{
		bool flag = false;
		try
		{
			if (roleComponent == null)
			{
				roleComponent = requestContext.CreateRoleComponent();
				flag = true;
			}
			return roleComponent.QueryRoleById(roleId);
		}
		finally
		{
			if (flag)
			{
				roleComponent.Dispose();
			}
		}
	}

	public static Role[] FindRolesByMember(IGeminiRequestContext requestContext, string memberSid)
	{
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		return roleComponent.QueryRolesByUserSid(memberSid);
	}

	private static Role FindRoleByName(IGeminiRequestContext requestContext, IRoleComponent roleComponent, string name)
	{
		bool flag = false;
		try
		{
			if (roleComponent == null)
			{
				roleComponent = requestContext.CreateRoleComponent();
				flag = true;
			}
			return roleComponent.QueryRoleByName(name);
		}
		finally
		{
			if (flag)
			{
				roleComponent.Dispose();
			}
		}
	}

	public void Validate(string parameterName)
	{
		string roleName = Name;
		SecurityValidation.CheckRoleName(ref roleName, parameterName);
		Name = roleName;
		string description = Description;
		Validation.CheckDescription(ref description, parameterName);
		Description = description;
	}

	public static void UpdateClientLoginLegacyPermission(IGeminiRequestContext requestContext, string roleId, string timeProfileId, bool isDefaultTimeProfile)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		Validation.CheckObjectId(timeProfileId, "timeProfileId", allowNullOrEmpty: true);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Guid sessionId = Guid.NewGuid();
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"))
		{
			throw new FeatureNotLicensedException();
		}
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.UpdateClientLoginLegacyPermission(roleId, timeProfileId, isDefaultTimeProfile, sessionId);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Updated, string.Empty);
	}

	public static string QueryClientLoginLegacyPermission(IGeminiRequestContext requestContext, string roleId)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Guid sessionId = Guid.NewGuid();
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"))
		{
			throw new FeatureNotLicensedException();
		}
		string result;
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			result = roleComponent.QueryClientLoginLegacyPermission(roleId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static string QueryDefaultTimeProfileId(IGeminiRequestContext requestContext, string roleId)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Guid sessionId = Guid.NewGuid();
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"))
		{
			throw new FeatureNotLicensedException();
		}
		string result;
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			result = roleComponent.QueryDefaultTimeProfileId(roleId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static Tuple<string, bool> QueryClientLoginLegacyPermissionId(IGeminiRequestContext requestContext, string roleId)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Guid sessionId = Guid.NewGuid();
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"))
		{
			throw new FeatureNotLicensedException();
		}
		Tuple<string, bool> result;
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			result = roleComponent.QueryClientLoginLegacyPermissionId(roleId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static void UpdateDefaultTimeProfileId(IGeminiRequestContext requestContext, string roleId, string timeProfileId)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(roleId, "roleId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"))
		{
			throw new FeatureNotLicensedException();
		}
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			roleComponent.UpdateDefaultTimeProfileId(roleId, timeProfileId, sessionId);
		}
		Integration.TryNotifyServerAsync();
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Role", roleId, EventAction.Updated, string.Empty);
	}

	public static void UpdateRoleValidation(IGeminiRequestContext requestContext, string id, string name, string description, bool? dualAuthorizationRequired, bool? showPtzReservedNameAsAnonymous, bool? allowMobileClientLogOn, bool? allowSmartClientLogOn, bool? allowWebClientLogOn)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		Validation.CheckName(name, "name", allowNull: false);
		Validation.CheckDescription(ref description, "description");
		Validation.CheckBooleanValue(dualAuthorizationRequired, "dualAuthorizationRequired", allowNull: false);
		Validation.CheckBooleanValue(showPtzReservedNameAsAnonymous, "showPtzReservedNameAsAnonymous", allowNull: false);
		Validation.CheckBooleanValue(allowMobileClientLogOn, "allowMobileClientLogOn", allowNull: false);
		Validation.CheckBooleanValue(allowSmartClientLogOn, "allowSmartClientLogOn", allowNull: false);
		Validation.CheckBooleanValue(allowWebClientLogOn, "allowWebClientLogOn", allowNull: false);
	}
}
