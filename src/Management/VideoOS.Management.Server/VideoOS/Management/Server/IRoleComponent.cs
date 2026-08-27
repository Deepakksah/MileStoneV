using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IRoleComponent : IDisposable
{
	Role QueryRoleById(string roleId);

	string[] QueryRoleMembers(string roleId, bool includeDynamicallyAddedIdentities);

	string[] QueryRoleMembersWithDeleted(string roleId, bool includeDynamicallyAddedIdentities);

	Role QueryRoleByName(string name);

	Role[] QueryRolesByUserSid(string memberSid);

	Role[] QueryRoles();

	Role[] QueryRolesByIds(string[] ids);

	string[] QueryRoleIds();

	Guid[] QueryRoleIdsByUserSidDynamic(string memberSid);

	string[] QueryUserSidDynamicByRoleId(string roleId);

	string CreateRole(Role newRole);

	string CopyRole(Role newRole, string sourceRoleId);

	void UpdateRole(string roleId, string name, string description, bool? dualAuthorizationRequired, bool? showPtzReservedNameAsAnonymous, bool? allowMobileClientLogOn, bool? allowSmartClientLogOn, bool? allowWebClientLogOn, Guid sessionId);

	void DeleteRole(string roleId, Guid sessionId);

	void AddIdentityToRole(string roleId, Identity identity, bool isDynamic, Guid sessionId);

	void RemoveIdentityFromRole(string roleId, string memberSid, bool removeDynamic, Guid sessionId);

	void AddClaimToRole(string roleId, string claimProvider, string claimName, string claimValue, Guid sessionId);

	void RemoveClaimFromRole(string roleId, string claimProvider, string claimName, string claimValue, Guid sessionId);

	Claim[] QueryClaimsForRole(string roleId);

	Guid[] QueryRolesByClaim(Guid claimProvider, string claimName, string claimValue);

	UserUpdateInfo[] UserUpdatedCacheQueryWithCleanup();

	void UserUpdatedCacheCreateOrUpdate(Guid userId, DateTime lastModified);

	void UpdateClientLoginLegacyPermission(string roleId, string timeProfileId, bool isDefaultTimeProfile, Guid sessionId);

	string QueryClientLoginLegacyPermission(string roleId);

	string[] QueryRoleIdsByClientProfileId(string parentId);

	string QueryDefaultTimeProfileId(string roleId);

	void UpdateDefaultTimeProfileId(string roleId, string timeProfileId, Guid sessionId);

	Tuple<string, bool> QueryClientLoginLegacyPermissionId(string roleId);
}
