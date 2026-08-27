using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public static class ClaimToRoleMapper
{
	private static Guid dummyUserIdToSaveLastModified = Guid.Empty;

	private static volatile bool _userCacheLoaded = false;

	private static object _userCacheLoadLock = new object();

	public static void CheckForPossibleRoleMappings(IGeminiRequestContext requestContext)
	{
		if (requestContext.IsExternalVmsUser && Guid.TryParse(requestContext.AuthenticatedUserSid, out var result) && requestContext.UserUpdatedAt > UserUpdatedCache.Instance.FindUserLastModified(result))
		{
			CheckUsersForChanges(requestContext, new Guid[1] { result }, ignoreLastModified: false, clearCacheOnChanges: true);
		}
	}

	public static void ClaimAddedToRole(IGeminiRequestContext requestContext, Guid loginProviderId, string claimName)
	{
		CheckUsersWithClaimsForChanges(requestContext, loginProviderId, claimName, null);
	}

	public static void RegisteredClaimNameChanged(IGeminiRequestContext requestContext, Guid loginProviderId, string claimNameBefore, string claimNameAfter)
	{
		CheckUsersWithClaimsForChanges(requestContext, loginProviderId, claimNameBefore, claimNameAfter);
	}

	private static void CheckUsersWithClaimsForChanges(IGeminiRequestContext requestContext, Guid loginProviderId, string claimName, string claimName2OrNull)
	{
		BasicUser[] source = null;
		try
		{
			using IUserIdpComponent userIdpComponent = requestContext.GetService<IdpClientService>().CreateUserIdpComponent();
			source = (from u in userIdpComponent.GetAllVmsUsers()
				where u.IsExternal
				select u).ToArray();
			if (!source.Any())
			{
				return;
			}
		}
		catch (GenericIdpException ex)
		{
			GeminiTrace.Error(TraceType.Security, "Unable to get users from the IDP to check for changes in role mapping (claim added)");
			GeminiTrace.TraceException(ex);
			return;
		}
		catch (VmoServiceException ex2)
		{
			GeminiTrace.Error(TraceType.Security, "Unable to get users from the IDP to check for changes in role mapping (claim added)");
			GeminiTrace.TraceException(ex2);
			return;
		}
		Guid[] array = (from usr in source
			where usr.Claims.Any((Claim c) => (c.Name.Equals(claimName, StringComparison.Ordinal) || c.Name.Equals(claimName2OrNull, StringComparison.Ordinal)) && c.LoginProviderId == loginProviderId)
			select new Guid(usr.Id)).ToArray();
		if (array.Any())
		{
			CheckUsersForChanges(requestContext, array, ignoreLastModified: true, clearCacheOnChanges: true);
		}
	}

	public static void ClaimRemovedFromRole(IGeminiRequestContext requestContext, string roleId)
	{
		string[] source = Array.Empty<string>();
		try
		{
			using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
			source = roleComponent.QueryUserSidDynamicByRoleId(roleId);
		}
		catch (Exception ex)
		{
			GeminiTrace.Error(TraceType.Security, "An error occurred getting the dynamic users for role: {0}", roleId);
			GeminiTrace.TraceException(ex);
			if (!HandleException(ex))
			{
				throw;
			}
		}
		if (source.Any())
		{
			CheckUsersForChanges(requestContext, source.Select((string usr) => new Guid(usr)).ToArray(), ignoreLastModified: true, clearCacheOnChanges: true);
		}
	}

	public static bool PeriodicCheckForChanges(IGeminiRequestContext requestContext, bool clearCacheOnChanges)
	{
		CheckForUserCacheLoaded(requestContext);
		DateTime dateTime = UserUpdatedCache.Instance.FindUserLastModified(dummyUserIdToSaveLastModified);
		Guid[] array = null;
		bool result = false;
		try
		{
			using IUserIdpComponent userIdpComponent = requestContext.GetService<IdpClientService>().CreateUserIdpComponent();
			BasicUser[] vmsUsersModifiedAfter = userIdpComponent.GetVmsUsersModifiedAfter(dateTime);
			if (!vmsUsersModifiedAfter.Any())
			{
				return false;
			}
			dateTime = vmsUsersModifiedAfter.Max((BasicUser u) => u.LastModified);
			array = (from usr in vmsUsersModifiedAfter
				where usr.IsExternal && usr.LastModified > UserUpdatedCache.Instance.FindUserLastModified(new Guid(usr.Id))
				select new Guid(usr.Id)).ToArray();
		}
		catch (GenericIdpException ex)
		{
			GeminiTrace.Error(TraceType.Security, "Unable to get users from the IDP to check for changes in role mapping (claim added)");
			GeminiTrace.TraceException(ex);
			return false;
		}
		catch (VmoServiceException ex2)
		{
			GeminiTrace.Error(TraceType.Security, "Unable to get users from the IDP to check for changes in role mapping");
			GeminiTrace.TraceException(ex2);
			return false;
		}
		if (array.Any())
		{
			result = CheckUsersForChanges(requestContext, array, ignoreLastModified: false, clearCacheOnChanges);
		}
		try
		{
			UserUpdateInfo userUpdateInfo = UserUpdatedCache.Instance.FindOrCreateUserInfo(dummyUserIdToSaveLastModified);
			if (dateTime > userUpdateInfo.LastModified)
			{
				using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
				userUpdateInfo.LastModified = dateTime;
				roleComponent.UserUpdatedCacheCreateOrUpdate(userUpdateInfo.Id, userUpdateInfo.LastModified);
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex4)
		{
			GeminiTrace.Error(TraceType.Security, "An error occurred updating 'user updated cache' with last search time");
			GeminiTrace.TraceException(ex4);
			if (!HandleException(ex4))
			{
				throw;
			}
		}
		return result;
	}

	private static bool CheckUsersForChanges(IGeminiRequestContext requestContext, Guid[] extUserIds, bool ignoreLastModified, bool clearCacheOnChanges)
	{
		CheckForUserCacheLoaded(requestContext);
		RegisteredClaimData[] array;
		try
		{
			using IRegisteredClaimComponent registeredClaimComponent = requestContext.CreateRegisteredClaimComponent();
			array = registeredClaimComponent.QueryAll();
			if (!array.Any())
			{
				return false;
			}
		}
		catch (ThreadAbortException)
		{
			GeminiTrace.Info(TraceType.Security, "CheckOneUserForChanges: Thread is aborted");
			return false;
		}
		catch (Exception ex2)
		{
			GeminiTrace.Error(TraceType.Security, "An error occurred getting the registered claims from SQL");
			GeminiTrace.TraceException(ex2);
			if (!HandleException(ex2))
			{
				throw;
			}
			return false;
		}
		bool notifyAboutAdmin = false;
		bool somethingHasChanged = false;
		using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
		{
			using IUserIdpComponent idpUserComponent = requestContext.GetService<IdpClientService>().CreateUserIdpComponent();
			foreach (Guid extUserId in extUserIds)
			{
				CheckOneUserForChanges(array, idpUserComponent, roleComponent, extUserId, ignoreLastModified, ref notifyAboutAdmin, ref somethingHasChanged);
			}
		}
		if (somethingHasChanged)
		{
			if (clearCacheOnChanges)
			{
				IdentityCache.ClearMemoryCache();
				requestContext.GetSecurityManagerService().ForceReloadOfAllNamespaces(requestContext);
			}
			if (notifyAboutAdmin)
			{
				Integration.TryNotifyServerAsync();
			}
			return true;
		}
		return false;
	}

	public static void CheckOneUserForChanges(RegisteredClaimData[] registeredClaims, IUserIdpComponent idpUserComponent, IRoleComponent roleComponent, Guid extUserId, bool ignoreLastModified, ref bool notifyAboutAdmin, ref bool somethingHasChanged)
	{
		UserUpdateInfo userUpdateInfo = UserUpdatedCache.Instance.FindOrCreateUserInfo(extUserId);
		lock (userUpdateInfo)
		{
			BasicUser basicUser = FindExtUserFromIDP(idpUserComponent, extUserId);
			if (basicUser == null || !(basicUser.LastModified > userUpdateInfo.LastModified || ignoreLastModified))
			{
				return;
			}
			try
			{
				Identity identity = BasicUserAccessor.MapUserToIdentity(basicUser);
				HashSet<Guid> hashSet = new HashSet<Guid>();
				Claim[] array = basicUser.Claims.Where((Claim c) => registeredClaims.Any((RegisteredClaimData ci) => ci.Name.Equals(c.Name, StringComparison.Ordinal) && ci.LoginProviderId == c.LoginProviderId)).ToArray();
				foreach (Claim claim in array)
				{
					Guid[] array2 = roleComponent.QueryRolesByClaim(claim.LoginProviderId, claim.Name, claim.Value);
					foreach (Guid item in array2)
					{
						hashSet.Add(item);
					}
				}
				List<Guid> list = new List<Guid>(roleComponent.QueryRoleIdsByUserSidDynamic(identity.Sid));
				foreach (Guid item2 in hashSet)
				{
					if (!list.Contains(item2))
					{
						GeminiTrace.Verbose(TraceType.Security, "Adding dynamic role mapping of user {0} to role {1}", basicUser.Id, item2);
						roleComponent.AddIdentityToRole(item2.ToString(), identity, isDynamic: true, Guid.Empty);
						somethingHasChanged = true;
						if (item2.Equals(SecurityManager.AdministratorRoleId))
						{
							notifyAboutAdmin = true;
						}
					}
					else
					{
						list.Remove(item2);
					}
				}
				foreach (Guid item3 in list)
				{
					GeminiTrace.Verbose(TraceType.Security, "Removing dynamic role mapping of user {0} from role {1}", basicUser.Id, item3);
					roleComponent.RemoveIdentityFromRole(item3.ToString(), identity.Sid, removeDynamic: true, Guid.Empty);
					somethingHasChanged = true;
					if (item3.Equals(SecurityManager.AdministratorRoleId))
					{
						notifyAboutAdmin = true;
					}
				}
				if (basicUser.LastModified > userUpdateInfo.LastModified)
				{
					userUpdateInfo.LastModified = basicUser.LastModified;
					roleComponent.UserUpdatedCacheCreateOrUpdate(userUpdateInfo.Id, userUpdateInfo.LastModified);
				}
			}
			catch (ThreadAbortException)
			{
				GeminiTrace.Info(TraceType.Security, "CheckOneUserForChanges: Thread is aborted");
			}
			catch (Exception ex2)
			{
				GeminiTrace.Error(TraceType.Security, "An error occurred doing check for changes in role mapping for: {0} - {1}", basicUser.Name, basicUser.Id);
				GeminiTrace.TraceException(ex2);
				if (!HandleException(ex2))
				{
					throw;
				}
			}
		}
	}

	private static BasicUser FindExtUserFromIDP(IUserIdpComponent idpUserComponent, Guid extUserId)
	{
		BasicUser basicUser = null;
		try
		{
			basicUser = idpUserComponent.GetVmsUserById(extUserId.ToString());
			if (basicUser == null)
			{
				GeminiTrace.Error(TraceType.Security, "Unable to get user {0} from the IDP to do possible role mapping (no user found)", extUserId);
			}
		}
		catch (GenericIdpException ex)
		{
			GeminiTrace.Error(TraceType.Security, "Unable to get users from the IDP to check for changes in role mapping (claim added)");
			GeminiTrace.TraceException(ex);
		}
		catch (VmoServiceException ex2)
		{
			GeminiTrace.Error(TraceType.Security, "Unable to get user {0} from the IDP to do possible role mapping", extUserId);
			GeminiTrace.TraceException(ex2);
		}
		return basicUser;
	}

	private static void CheckForUserCacheLoaded(IGeminiRequestContext requestContext)
	{
		if (_userCacheLoaded)
		{
			return;
		}
		lock (_userCacheLoadLock)
		{
			if (_userCacheLoaded)
			{
				return;
			}
			try
			{
				using (IRoleComponent roleComponent = requestContext.CreateRoleComponent())
				{
					UserUpdateInfo[] userUpdateInfos = roleComponent.UserUpdatedCacheQueryWithCleanup();
					UserUpdatedCache.Instance.Initialize(userUpdateInfos);
				}
				_userCacheLoaded = true;
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex2)
			{
				GeminiTrace.Error(TraceType.Security, "An error occurred getting 'user updated cache' loaded");
				GeminiTrace.TraceException(ex2);
				if (!HandleException(ex2))
				{
					throw;
				}
			}
		}
	}

	private static bool HandleException(Exception ex)
	{
		if (!(ex is VmoServerException) && !(ex is DatabaseConnectionException) && !(ex is DatabaseException))
		{
			return ex is IOException;
		}
		return true;
	}
}
