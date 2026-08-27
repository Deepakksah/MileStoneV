using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class SecurityManagerService : ISecurityManagerService, IGeminiServerService
{
	private enum CacheStatus
	{
		NotLoaded,
		SecurityNotReloaded,
		UpToDate
	}

	private const string _area = "Security";

	private const string _layer = "SecurityManagerService";

	private Dictionary<Guid, IGeminiSecurityNamespace> _securityNamespaces;

	private ReaderWriterLock _namespaceLock;

	private int _cacheStatus;

	public SecurityManagerService()
	{
		_securityNamespaces = new Dictionary<Guid, IGeminiSecurityNamespace>();
		_namespaceLock = new ReaderWriterLock();
		_cacheStatus = 0;
	}

	public IGeminiSecurityNamespace GetSecurityNamespace(IGeminiRequestContext requestContext, Guid namespaceId)
	{
		Validation.CheckRequestContext(requestContext);
		IGeminiSecurityNamespace value = null;
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "GetSecurityNamespace");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			_namespaceLock.AcquireReaderLock(-1);
			_securityNamespaces.TryGetValue(namespaceId, out value);
			return value;
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsReaderLockHeld)
			{
				_namespaceLock.ReleaseReaderLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "GetSecurityNamespace");
		}
	}

	public IEnumerable<IGeminiSecurityNamespace> GetSecurityNamespaces(IGeminiRequestContext requestContext)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "GetSecurityNamespaces");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			List<IGeminiSecurityNamespace> list = new List<IGeminiSecurityNamespace>();
			_namespaceLock.AcquireReaderLock(-1);
			foreach (IGeminiSecurityNamespace value in _securityNamespaces.Values)
			{
				list.Add(value);
			}
			return list;
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsReaderLockHeld)
			{
				_namespaceLock.ReleaseReaderLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "GetSecurityNamespaces");
		}
	}

	public IGeminiSecurityNamespace GetSecurityNamespaceFromToken(IGeminiRequestContext requestContext, string securityToken)
	{
		Validation.CheckRequestContext(requestContext);
		IGeminiSecurityNamespace geminiSecurityNamespace = null;
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "GetSecurityNamespaceFromToken");
		try
		{
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			string securityNamespaceIdFromToken = securityComponent.GetSecurityNamespaceIdFromToken(securityToken);
			if (securityNamespaceIdFromToken == null)
			{
				throw new SecurityObjectDoesNotExistException(securityToken);
			}
			return GetSecurityNamespace(requestContext, new Guid(securityNamespaceIdFromToken));
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "GetSecurityNamespaceFromToken");
		}
	}

	private static void ValidateRequestContext(IGeminiRequestContext requestContext)
	{
		GeminiValidation.CheckForNull(requestContext, "requestContext");
	}

	private void EnsureNamespacesLoaded(IGeminiRequestContext requestContext)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "EnsureNamespacesLoaded");
		try
		{
			int num = Interlocked.CompareExchange(ref _cacheStatus, 0, 0);
			if (num == 2)
			{
				return;
			}
			_namespaceLock.AcquireWriterLock(-1);
			switch (Interlocked.CompareExchange(ref _cacheStatus, 0, 0))
			{
			case 0:
			{
				Dictionary<Guid, IGeminiSecurityNamespace> dictionary = new Dictionary<Guid, IGeminiSecurityNamespace>();
				SecurityNamespaceInfo[] namespaceFromDatabase = GetNamespaceFromDatabase(requestContext, string.Empty);
				if (namespaceFromDatabase != null)
				{
					SecurityNamespaceInfo[] array = namespaceFromDatabase;
					foreach (SecurityNamespaceInfo securityNamespaceInfo in array)
					{
						dictionary[securityNamespaceInfo.NamespaceGuid] = new GeminiSecurityNamespace(requestContext, securityNamespaceInfo);
					}
				}
				_securityNamespaces = dictionary;
				break;
			}
			default:
				foreach (IGeminiSecurityNamespace value in _securityNamespaces.Values)
				{
					value.ForceNamespaceReload(requestContext);
				}
				break;
			case 2:
				return;
			}
			Interlocked.Exchange(ref _cacheStatus, 2);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsWriterLockHeld)
			{
				_namespaceLock.ReleaseWriterLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "EnsureNamespacesLoaded");
		}
	}

	private static SecurityNamespaceInfo[] GetNamespaceFromDatabase(IGeminiRequestContext requestContext, string namespaceId)
	{
		IEnumerable<SecurityNamespaceInfo> enumerable = null;
		IEnumerable<ActionDefinition> enumerable2 = null;
		using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
		{
			ResultCollection resultCollection = securityComponent.QuerySecurityNamespaces(namespaceId);
			enumerable = resultCollection.GetCurrent<SecurityNamespaceInfo>().Items;
			resultCollection.NextResult();
			enumerable2 = resultCollection.GetCurrent<ActionDefinition>().Items;
		}
		Dictionary<Guid, SecurityNamespaceInfo> dictionary = null;
		if (enumerable != null)
		{
			dictionary = new Dictionary<Guid, SecurityNamespaceInfo>();
			foreach (SecurityNamespaceInfo item in enumerable)
			{
				dictionary.Add(item.NamespaceGuid, item);
			}
		}
		if (enumerable2 != null && dictionary != null)
		{
			foreach (ActionDefinition item2 in enumerable2)
			{
				if ((!(item2.NamespaceGuid == SecurityNamespaceIdentifier.ManagementServer) || !item2.Name.Equals("AUTHORIZE_USER", StringComparison.OrdinalIgnoreCase) || GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "DualAuthorization")) && dictionary.TryGetValue(item2.NamespaceGuid, out var value))
				{
					value.Actions.Add(item2);
				}
			}
		}
		return dictionary.Values.ToArray();
	}

	public void ForceReloadOfAllNamespaces(IGeminiRequestContext requestContext)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "ForceReloadOfAllNamespaces");
		try
		{
			_namespaceLock.AcquireReaderLock(-1);
			Interlocked.Exchange(ref _cacheStatus, 1);
		}
		finally
		{
			if (_namespaceLock.IsReaderLockHeld)
			{
				_namespaceLock.ReleaseReaderLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "ForceReloadOfAllNamespaces");
		}
	}

	public void CreateSecurityNamespace(IGeminiRequestContext requestContext, SecurityNamespaceInfo description, string orderAfterNamespaceId)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "CreateSecurityNamespace");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			ValidateSecurityActions(description);
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				securityComponent.CreateSecurityNamespace(description, orderAfterNamespaceId);
			}
			SecurityNamespaceInfo[] namespaceFromDatabase = GetNamespaceFromDatabase(requestContext, description.NamespaceId);
			_namespaceLock.AcquireWriterLock(-1);
			_securityNamespaces[description.NamespaceGuid] = new GeminiSecurityNamespace(requestContext, namespaceFromDatabase[0]);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsWriterLockHeld)
			{
				_namespaceLock.ReleaseWriterLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "CreateSecurityNamespace");
		}
	}

	public void UpdateSecurityNamespace(IGeminiRequestContext requestContext, SecurityNamespaceInfo description)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "UpdateSecurityNamespace");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			if (!requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, new Guid(description.NamespaceId)).NamespaceInfo.UserDefined)
			{
				throw new NotSupportedException();
			}
			ValidateSecurityActions(description);
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				securityComponent.UpdateSecurityNamespace(description);
			}
			SecurityNamespaceInfo[] namespaceFromDatabase = GetNamespaceFromDatabase(requestContext, description.NamespaceId);
			_namespaceLock.AcquireWriterLock(-1);
			_securityNamespaces[description.NamespaceGuid] = new GeminiSecurityNamespace(requestContext, namespaceFromDatabase[0]);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsWriterLockHeld)
			{
				_namespaceLock.ReleaseWriterLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "UpdateSecurityNamespace");
		}
	}

	public void DeleteSecurityNamespace(IGeminiRequestContext requestContext, Guid namespaceId)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "DeleteSecurityNamespace");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			if (!requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).NamespaceInfo.UserDefined)
			{
				throw new NotSupportedException();
			}
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				securityComponent.DeleteSecurityNamespace(namespaceId);
			}
			_namespaceLock.AcquireWriterLock(-1);
			if (_securityNamespaces.ContainsKey(namespaceId))
			{
				_securityNamespaces.Remove(namespaceId);
			}
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsWriterLockHeld)
			{
				_namespaceLock.ReleaseWriterLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "DeleteSecurityNamespace");
		}
	}

	public void RegisterSecurityObject(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string parentObjectId)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "RegisterSecurityObject");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			if (!requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).NamespaceInfo.UserDefined)
			{
				throw new NotSupportedException();
			}
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				if (securityComponent.RegisterObject(namespaceId, objectId, parentObjectId))
				{
					requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).ForceNamespaceReload(requestContext);
				}
			}
			if (!string.IsNullOrEmpty(parentObjectId))
			{
				PrivateViewGroupTokenCache.Clear();
			}
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "RegisterSecurityObject");
		}
	}

	public void UnregisterSecurityObject(IGeminiRequestContext requestContext, Guid namespaceId, string objectId)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "UnregisterSecurityObject");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			if (!requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).NamespaceInfo.UserDefined)
			{
				throw new NotSupportedException();
			}
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				if (securityComponent.UnregisterObject(namespaceId, objectId))
				{
					requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).ForceNamespaceReload(requestContext);
				}
			}
			PrivateViewGroupTokenCache.Clear();
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "UnregisterSecurityObject");
		}
	}

	public AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "QueryPermissions");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			return securityComponent.QueryPermissions(namespaceId, securityToken).GetCurrent<AccessControlEntry>().Items.ToArray();
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "QueryPermissions");
		}
	}

	public AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string identity)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "QueryPermissions");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			List<AccessControlEntry> items = securityComponent.QueryPermissions(namespaceId, securityToken).GetCurrent<AccessControlEntry>().Items;
			if (namespaceId == SecurityNamespaceIdentifier.Matrix && Guid.TryParse(securityToken, out var _) && securityComponent.QueryMatrixPermission(identity, securityToken))
			{
				items.Add(new AccessControlEntry("GENERIC_READ", identity, accessDeny: false));
			}
			return items.FindAll((AccessControlEntry e) => e.Sid.Equals(identity, StringComparison.OrdinalIgnoreCase)).ToArray();
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "QueryPermissions");
		}
	}

	public AccessControlEntry[] QueryPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string[] identity)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "QueryPermissions");
		try
		{
			if (requestContext is GeminiRequestContext requestContext2)
			{
				ValidateRequestContext(requestContext2);
				EnsureNamespacesLoaded(requestContext2);
			}
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			return securityComponent.QueryPermissions(namespaceId, securityToken).GetCurrent<AccessControlEntry>().Items.FindAll((AccessControlEntry e) => identity.Contains(e.Sid, StringComparer.OrdinalIgnoreCase)).ToArray();
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "QueryPermissions");
		}
	}

	public bool AddPermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "AddPermissions");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			return UpdatePermissions(requestContext, namespaceId, securityToken, objectName, entries, replace: false);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "AddPermissions");
		}
	}

	public bool ReplacePermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] entries)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "ReplacePermissions");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			return UpdatePermissions(requestContext, namespaceId, securityToken, objectName, entries, replace: true);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "ReplacePermissions");
		}
	}

	public bool RemovePermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] removeEntries)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "RemovePermissions");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			Guid sessionId = Guid.NewGuid();
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			bool flag = ((namespaceId == SecurityNamespaceIdentifier.Matrix && securityToken != "/") ? securityComponent.RemoveMatrixPermissions(securityToken, objectName, removeEntries, sessionId) : securityComponent.RemovePermissions(namespaceId, securityToken, objectName, removeEntries, sessionId));
			if (flag)
			{
				requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).ForceNamespaceReload(requestContext);
				using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
				{
					vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				}
				Integration.TryNotifyServerAsync();
				HashSet<string> hashSet = new HashSet<string>();
				foreach (AccessControlEntry accessControlEntry in removeEntries)
				{
					hashSet.Add(accessControlEntry.Sid);
				}
				Identity[] array = IdentityCache.ReadIdentitiesFromCache(requestContext, hashSet.ToArray(), QueryMembership.None);
				foreach (Identity identity in array)
				{
					if (identity != null && identity.Type == IdentityType.Role)
					{
						Integration.FireConfigurationChanged(requestContext, "Role", identity.Sid, EventAction.Updated, string.Empty);
					}
				}
			}
			return flag;
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "RemovePermissions");
		}
	}

	public bool AddRemovePermissions(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string objectName, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "RemovePermissions");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			bool flag = false;
			Guid sessionId = Guid.NewGuid();
			IGeminiSecurityNamespace securityNamespace = requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId);
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				flag = securityComponent.RemovePermissions(namespaceId, securityToken, objectName, removeEntries, sessionId);
			}
			if (flag)
			{
				try
				{
					UpdatePermissions(requestContext, namespaceId, securityToken, objectName, addEntries, replace: false);
				}
				finally
				{
					securityNamespace.ForceNamespaceReload(requestContext);
					using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
					{
						vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
					}
					Integration.TryNotifyServerAsync();
					HashSet<string> hashSet = new HashSet<string>();
					AccessControlEntry[] array = addEntries;
					foreach (AccessControlEntry accessControlEntry in array)
					{
						hashSet.Add(accessControlEntry.Sid);
					}
					array = removeEntries;
					foreach (AccessControlEntry accessControlEntry2 in array)
					{
						hashSet.Add(accessControlEntry2.Sid);
					}
					Identity[] array2 = IdentityCache.ReadIdentitiesFromCache(requestContext, hashSet.ToArray(), QueryMembership.None);
					foreach (Identity identity in array2)
					{
						if (identity != null && identity.Type == IdentityType.Role)
						{
							Integration.FireConfigurationChanged(requestContext, "Role", identity.Sid, EventAction.Updated, string.Empty);
						}
					}
				}
			}
			return flag;
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "RemovePermissions");
		}
	}

	public static bool UpdatePermissions(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string objectName, AccessControlEntry[] entries, bool replace)
	{
		List<string> list = new List<string>();
		foreach (AccessControlEntry accessControlEntry in entries)
		{
			if (!list.Contains(accessControlEntry.Sid))
			{
				list.Add(accessControlEntry.Sid);
			}
		}
		using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
		Identity[] array = IdentityCache.ReadIdentitiesFromCache(requestContext, list.ToArray(), QueryMembership.None);
		for (int j = 0; j < list.Count; j++)
		{
			if (array[j] == null)
			{
				array[j] = IdentityAccessor.ReadIdentityFromSource(requestContext, list[j]);
			}
			if (array[j] == null)
			{
				throw VmoExceptionFactory.VMO60154_ActiveDirectoryAccessException();
			}
			if (string.IsNullOrEmpty(array[j].Domain) && array[j].Type != IdentityType.Role)
			{
				throw new InvalidDomainException();
			}
		}
		Guid sessionId = Guid.NewGuid();
		bool flag = ((namespaceId == SecurityNamespaceIdentifier.Matrix && objectId != "/") ? securityComponent.AddMatrixPermission(objectId, objectName, entries, replace, sessionId) : securityComponent.AddPermission(namespaceId, objectId, objectName, entries, replace, sessionId));
		Identity[] array2;
		if (flag)
		{
			requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).ForceNamespaceReload(requestContext);
			Integration.TryNotifyServerAsync();
			using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
			{
				vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			HashSet<string> hashSet = new HashSet<string>();
			array2 = array;
			foreach (Identity identity in array2)
			{
				if (identity.Type == IdentityType.Role)
				{
					hashSet.Add(identity.Sid);
				}
			}
			foreach (string item in hashSet)
			{
				Integration.FireConfigurationChanged(requestContext, "Role", item, EventAction.Updated, string.Empty);
			}
		}
		array2 = array;
		foreach (Identity identity2 in array2)
		{
			if (identity2.Type == IdentityType.WindowsGroup)
			{
				requestContext.GetGeminiIdentityService().SynchronizeWindowsGroup(identity2);
			}
		}
		return flag;
	}

	public List<AccessControlDeviceTimeProfileEntry> QueryDeviceTimeProfile(IGeminiRequestContext requestContext, string userId, string deviceId)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "QueryDeviceTimeProfile");
		try
		{
			if (requestContext is GeminiRequestContext requestContext2)
			{
				ValidateRequestContext(requestContext2);
				EnsureNamespacesLoaded(requestContext2);
			}
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			return securityComponent.QueryDeviceTimeProfile(userId, deviceId);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "QueryDeviceTimeProfile");
		}
	}

	public void UpdateDeviceTimeProfilePermissions(IGeminiRequestContext requestContext, Guid namespaceId, List<(AccessControlDeviceTimeProfileEntry, string)> entries)
	{
		Guid sessionId = Guid.NewGuid();
		HashSet<Guid> hashSet = new HashSet<Guid>();
		using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
		{
			foreach (var entry in entries)
			{
				securityComponent.UpdateDeviceTimeProfile(entry.Item1, entry.Item2, sessionId);
				hashSet.Add(entry.Item1.RoleId);
			}
		}
		requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).ForceNamespaceReload(requestContext);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.TryNotifyServerAsync();
		foreach (Guid item in hashSet)
		{
			Integration.FireConfigurationChanged(requestContext, "Role", item.ToString(), EventAction.Updated, string.Empty);
		}
	}

	public void SetInheritance(IGeminiRequestContext requestContext, Guid namespaceId, string securityToken, string parentObjectId)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "SetInheritance");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			securityComponent.SetInheritance(namespaceId, securityToken, parentObjectId);
			requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, namespaceId).ForceNamespaceReload(requestContext);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "SetInheritance");
		}
	}

	public void CheckPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId)
	{
		if (!HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, "/"))
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
	}

	public void CheckPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId)
	{
		if (!HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, objectId))
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
	}

	public void CheckPermission(IGeminiRequestContext requestContext, Guid namespaceId, string[] actionIds, string objectId)
	{
		if (actionIds == null)
		{
			throw new ArgumentNullException("actionIds");
		}
		if (actionIds.Length == 0)
		{
			throw new ArgumentException("At least one action ID must be provided.", "actionIds");
		}
		foreach (string actionId in actionIds)
		{
			if (HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, objectId))
			{
				return;
			}
		}
		throw new UnauthorizedAccessException(VmoResources.VMO61008);
	}

	public void CheckApplicationReadPermission(IGeminiRequestContext requestContext)
	{
		CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_READ");
	}

	public void CheckApplicationWritePermission(IGeminiRequestContext requestContext)
	{
		CheckPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_WRITE");
	}

	public bool HasPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId)
	{
		return HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, "/");
	}

	public bool HasPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId)
	{
		return HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, objectId);
	}

	public bool HasPermission(IGeminiRequestContext requestContext, Guid namespaceId, string userSid, string actionId, string objectId)
	{
		return HasPermission(requestContext, namespaceId, userSid, actionId, objectId, checkAdministrator: true);
	}

	public bool HasPermission(IGeminiRequestContext requestContext, Guid namespaceId, string userSid, string actionId, string objectId, bool checkAdministrator)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "HasPermission");
		string text = null;
		try
		{
			GeminiRequestContext geminiRequestContext = requestContext as GeminiRequestContext;
			if (geminiRequestContext != null)
			{
				ValidateRequestContext(geminiRequestContext);
				EnsureNamespacesLoaded(geminiRequestContext);
			}
			MapLegacyAction(namespaceId, ref actionId);
			if ((namespaceId == SecurityNamespaceIdentifier.Hardware || namespaceId == SecurityNamespaceIdentifier.RecordingServer) && actionId == "GENERIC_READ")
			{
				return true;
			}
			string text2 = null;
			string text3 = null;
			text2 = GetVideoWallIdIfExist(requestContext, namespaceId, objectId);
			if (text2 == null)
			{
				text3 = GetViewGroupIdIfExist(requestContext, objectId);
			}
			text = ((text2 != null) ? text2 : ((text3 == null) ? objectId : text3));
			return (GetSecurityNamespace(geminiRequestContext, namespaceId) ?? throw VmoExceptionFactory.VMO60325_VmoServerServiceException()).HasPermission(geminiRequestContext, userSid, actionId, text, checkAdministrator);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "HasPermission");
		}
	}

	private static string GetVideoWallIdIfExist(IGeminiRequestContext requestContext, Guid namespaceId, string objectId)
	{
		if (namespaceId.Equals(SecurityNamespaceIdentifier.SmartWall) && !objectId.Equals("/", StringComparison.OrdinalIgnoreCase))
		{
			string idFromMonitorOrPreset = VideoWallCache.GetIdFromMonitorOrPreset(objectId);
			if (idFromMonitorOrPreset != null)
			{
				return idFromMonitorOrPreset;
			}
			using (IVideoWallComponent videoWallComponent = requestContext.CreateVideoWallComponent())
			{
				VideoWall videoWall = videoWallComponent.QueryVideoWall(objectId);
				if (videoWall != null)
				{
					VideoWallCache.AddToCache(objectId, videoWall.Id);
					return videoWall.Id;
				}
			}
			using (IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent())
			{
				Monitor monitor = monitorComponent.QueryMonitor(objectId);
				if (monitor != null)
				{
					VideoWallCache.AddToCache(objectId, monitor.VideoWallId);
					return monitor.VideoWallId;
				}
			}
			using IVideoWallPresetComponent videoWallPresetComponent = requestContext.CreateVideoWallPresetComponent();
			VideoWallPreset videoWallPreset = videoWallPresetComponent.QueryVideoWallPreset(objectId);
			if (videoWallPreset != null)
			{
				VideoWallCache.AddToCache(objectId, videoWallPreset.VideoWallId);
				return videoWallPreset.VideoWallId;
			}
		}
		return null;
	}

	private static string GetViewGroupIdIfExist(IGeminiRequestContext requestContext, string objectId)
	{
		if (PrivateViewGroupTokenCache.Count == 0)
		{
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			foreach (ObjectToken item in securityComponent.QueryPrivateViewGroupTokens().GetCurrent<ObjectToken>())
			{
				PrivateViewGroupTokenCache.AddToCache(item.ObjectId, item.Token);
			}
		}
		return PrivateViewGroupTokenCache.GetPrivateViewGroupToken(objectId);
	}

	public bool HasApplicationReadPermission(IGeminiRequestContext requestContext)
	{
		return HasPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, "GENERIC_READ");
	}

	public void CheckObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId)
	{
		if (!HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, objectId))
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
	}

	public static void ParseIdentityName(string name, out string domain, out string accountName, out bool isRole, out bool isBasicUser)
	{
		domain = null;
		accountName = null;
		isRole = false;
		isBasicUser = false;
		int num = name.IndexOfAny(new char[2] { '\\', '/' });
		if (num < 0)
		{
			accountName = name;
			return;
		}
		domain = name.Substring(0, num).Trim();
		accountName = name.Substring(num + 1);
		if (domain.Equals("[BASIC]"))
		{
			isBasicUser = true;
		}
		else if (domain.Length > 2 && domain[0] == '[' && domain[domain.Length - 1] == ']')
		{
			isRole = true;
		}
	}

	public static void MapLegacyAction(Guid namespaceId, ref string actionId)
	{
		if (namespaceId == SecurityNamespaceIdentifier.UserDefinedEvent)
		{
			if (actionId.Equals(PermissionObjectActions.TriggerEvent))
			{
				actionId = "TRIGGER";
			}
		}
		else if (namespaceId == SecurityNamespaceIdentifier.ViewGroup)
		{
			if (actionId.Equals("CREATE_CHILDREN", StringComparison.OrdinalIgnoreCase))
			{
				actionId = "OPERATE";
			}
		}
		else if (namespaceId == SecurityNamespaceIdentifier.SmartWall)
		{
			if (actionId.Equals("APPLYLAYOUT", StringComparison.OrdinalIgnoreCase))
			{
				actionId = "OPERATE";
			}
			else if (actionId.Equals("ACTIVATE", StringComparison.OrdinalIgnoreCase))
			{
				actionId = "OPERATE";
			}
		}
		else
		{
			LegacySecurityActionInformation securityActionInformation = LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(actionId);
			if (securityActionInformation != null)
			{
				actionId = securityActionInformation.SecurityAction;
			}
		}
	}

	private static void ValidateSecurityActions(SecurityNamespaceInfo description)
	{
		if (description.Actions != null)
		{
			if (!description.Actions.Any((ActionDefinition e) => e.Name.Equals("GENERIC_READ", StringComparison.Ordinal)))
			{
				GeminiTrace.Error("SecurityNamespaceInfo", "Missing security action: GENERIC_READ in namespace: " + description.Name + "\n.... Actions.Count=" + description.Actions.Count + ", list=" + string.Join(",", description.Actions));
				throw new SecurityActionDoesNotExistException("GENERIC_READ");
			}
			if (!description.Actions.Any((ActionDefinition e) => e.Name.Equals("ADMIN_SECURITY", StringComparison.Ordinal)))
			{
				throw new SecurityActionDoesNotExistException("ADMIN_SECURITY");
			}
		}
	}

	public AccessControlTimeProfileEntry QueryAccessControlTimeProfile(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string identity, string actionId)
	{
		EnsureTimeBasedAction(namespaceId, actionId);
		using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
		return securityComponent.QueryAccessTimeProfileEntry(namespaceId, objectId, identity, actionId);
	}

	public AccessControlTimeProfileEntry AddUpdateAccessControlTimeProfileEntry(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string objectName, string identity, string actionId, string timeProfileId, bool isDefaultTimeProfile)
	{
		EnsureTimeBasedAction(namespaceId, actionId);
		Guid sessionId = Guid.NewGuid();
		using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
		securityComponent.CreateUpdateAccessTimeProfileEntry(namespaceId, objectId, objectName, identity, actionId, timeProfileId, isDefaultTimeProfile, sessionId);
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Identity identity2 = IdentityCache.ReadIdentityFromCache(requestContext, identity, QueryMembership.None);
		if (identity2 != null && identity2.Type == IdentityType.Role)
		{
			Integration.FireConfigurationChanged(requestContext, "Role", identity2.Sid, EventAction.Updated, string.Empty);
		}
		return securityComponent.QueryAccessTimeProfileEntry(namespaceId, objectId, identity, actionId);
	}

	public void DeleteAccessControlTimeProfileEntry(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string objectName, string identity, string actionId)
	{
		EnsureTimeBasedAction(namespaceId, actionId);
		Guid sessionId = Guid.NewGuid();
		using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
		{
			securityComponent.DeleteAccessTimeProfileEntry(namespaceId, objectId, objectName, identity, actionId, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Identity identity2 = IdentityCache.ReadIdentityFromCache(requestContext, identity, QueryMembership.None);
		if (identity2 != null && identity2.Type == IdentityType.Role)
		{
			Integration.FireConfigurationChanged(requestContext, "Role", identity2.Sid, EventAction.Updated, string.Empty);
		}
	}

	public void EnsureTimeBasedAction(Guid namespaceId, string actionId)
	{
		if ((namespaceId.Equals(SecurityNamespaceIdentifier.SmartWall) && (actionId == "OPERATE" || actionId == "PLAYBACK")) || (namespaceId.Equals(SecurityNamespaceIdentifier.UserDefinedEvent) && actionId == "TRIGGER"))
		{
			return;
		}
		throw new InvalidOperationException("Not time-based action");
	}

	public void CheckTimeBasedPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights") || TimeBasedSecurityIsPermitted(requestContext, namespaceId, objectId, actionId))
		{
			return;
		}
		throw new UnauthorizedAccessException(VmoResources.VMO61008);
	}

	public List<Tuple<AccessControlEntry, AccessControlTimeProfile>> QueryAccessEntryAndTimeProfile(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string roleId)
	{
		AccessControlEntry[] array = requestContext.GetSecurityManagerService().QueryPermissions(requestContext, namespaceId, objectId, roleId);
		List<Tuple<AccessControlEntry, AccessControlTimeProfile>> list = new List<Tuple<AccessControlEntry, AccessControlTimeProfile>>();
		using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
		AccessControlEntry[] array2 = array;
		foreach (AccessControlEntry accessControlEntry in array2)
		{
			AccessControlTimeProfile item = securityComponent.QueryAccessTimeProfileEntryIsPermitted(namespaceId, objectId, roleId, accessControlEntry.ActionId);
			list.Add(new Tuple<AccessControlEntry, AccessControlTimeProfile>(accessControlEntry, item));
		}
		return list;
	}

	public bool TimeBasedSecurityIsPermitted(IGeminiRequestContext requestContext, Guid namespaceId, string objectId, string actionId)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "TimeControlledAccessRights"))
		{
			return true;
		}
		if (HasPermission(requestContext, namespaceId, actionId))
		{
			return true;
		}
		EnsureTimeBasedAction(namespaceId, actionId);
		AccessControlEntry[] array = requestContext.GetSecurityManagerService().QueryPermissions(requestContext, namespaceId, objectId);
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		AccessControlEntry[] array2 = array;
		foreach (AccessControlEntry accessControlEntry in array2)
		{
			if (accessControlEntry.ActionId == actionId && !accessControlEntry.AccessDeny)
			{
				if (accessControlEntry.Sid == requestContext.AuthenticatedUserSid)
				{
					return true;
				}
				if (Guid.TryParse(accessControlEntry.Sid, out var _) && IdentityCache.IsMember(requestContext, requestContext.AuthenticatedUserSid, accessControlEntry.Sid))
				{
					list.Add(accessControlEntry);
				}
			}
		}
		using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
		return securityComponent.TimeBasedSecurityIsPermitted(namespaceId, objectId, actionId, list.ToArray());
	}

	public bool HasApplicationLoginPermission(IGeminiRequestContext requestContext, out DateTime evaluationTime, out string[] timeProfileIds)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "HasApplicationLoginPermission");
		try
		{
			bool flag = false;
			bool result = false;
			List<string> list = new List<string>();
			evaluationTime = DateTime.UtcNow;
			timeProfileIds = new string[0];
			Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded);
			if (identity == null)
			{
				return false;
			}
			string[] array = identity.MemberOfRoles.Select((Guid f) => f.ToString()).ToArray();
			if (array.Length == 0)
			{
				return false;
			}
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				string[] array2 = array;
				foreach (string roleId in array2)
				{
					if (securityComponent.TimeBasedSecurityIsPermitted(SecurityNamespaceIdentifier.ManagementServer, string.Empty, roleId, "APPLICATIONACCESSCLIENTLOGIN", evaluationTime, out var timeProfileId))
					{
						result = true;
						if (timeProfileId == null)
						{
							flag = true;
							break;
						}
					}
					if (!list.Contains(timeProfileId))
					{
						list.Add(timeProfileId);
					}
				}
			}
			if (flag)
			{
				list = new List<string> { null };
			}
			timeProfileIds = list.ToArray();
			return result;
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "HasApplicationLoginPermission");
		}
	}

	public void RemoveIdentity(IGeminiRequestContext requestContext, string userSid)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "RemoveIdentity");
		try
		{
			ValidateRequestContext(requestContext);
			Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, userSid, QueryMembership.None);
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				securityComponent.RemoveUser(userSid);
			}
			if (identity != null)
			{
				HttpContext.Current.Cache.Remove(BasicAuthenticationModule.CachePrefix + identity.AccountName.ToUpperInvariant());
				IdentityCache.ClearMemoryCache();
				requestContext.GetSecurityManagerService().ForceReloadOfAllNamespaces(requestContext);
			}
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "RemoveIdentity");
		}
	}

	void ISecurityManagerService.CheckApplicationPermission(IGeminiRequestContext requestContext, string actionId)
	{
		Validation.CheckRequestContext(requestContext);
		CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, actionId, "/");
	}

	void ISecurityManagerService.CheckOverallPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId)
	{
		Validation.CheckRequestContext(requestContext);
		CheckObjectPermission(requestContext, namespaceId, actionId, "/");
	}

	bool ISecurityManagerService.HasApplicationPermission(IGeminiRequestContext requestContext, string userSid, string actionId)
	{
		Validation.CheckRequestContext(requestContext);
		return HasPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, userSid, actionId, "/");
	}

	bool ISecurityManagerService.HasApplicationPermission(IGeminiRequestContext requestContext, string actionId)
	{
		Validation.CheckRequestContext(requestContext);
		return HasPermission(requestContext, SecurityNamespaceIdentifier.ManagementServer, requestContext.AuthenticatedUserSid, actionId, "/");
	}

	void ISecurityManagerService.CheckObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId)
	{
		Validation.CheckRequestContext(requestContext);
		CheckObjectPermission(requestContext, namespaceId, actionId, objectId);
	}

	bool ISecurityManagerService.HasObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string userSid, string actionId, string objectId)
	{
		Validation.CheckRequestContext(requestContext);
		return HasPermission(requestContext, namespaceId, userSid, actionId, objectId);
	}

	bool ISecurityManagerService.HasObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId)
	{
		Validation.CheckRequestContext(requestContext);
		return HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, objectId);
	}

	bool ISecurityManagerService.HasObjectPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId, bool checkAdministrator)
	{
		Validation.CheckRequestContext(requestContext);
		return HasPermission(requestContext, namespaceId, requestContext.AuthenticatedUserSid, actionId, objectId, checkAdministrator);
	}

	void ISecurityManagerService.CheckTimeBasedPermission(IGeminiRequestContext requestContext, Guid namespaceId, string actionId, string objectId)
	{
		Validation.CheckRequestContext(requestContext);
		CheckTimeBasedPermission(requestContext, namespaceId, actionId, objectId);
	}

	public string[] QueryObjectClasses(IGeminiRequestContext requestContext)
	{
		IEnumerable<string> source = null;
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "QueryObjectClasses");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			_namespaceLock.AcquireReaderLock(-1);
			source = _securityNamespaces.Values.Select((IGeminiSecurityNamespace e) => e.NamespaceInfo.Name);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsReaderLockHeld)
			{
				_namespaceLock.ReleaseReaderLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "QueryObjectClasses");
		}
		return source.ToArray();
	}

	public SecurityAction[] QueryObjectClassActions(IGeminiRequestContext requestContext, string objectClassId)
	{
		List<SecurityAction> list = new List<SecurityAction>();
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "QueryObjectClassActions");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			IGeminiSecurityNamespace geminiSecurityNamespace = null;
			_namespaceLock.AcquireReaderLock(-1);
			geminiSecurityNamespace = _securityNamespaces.Values.FirstOrDefault((IGeminiSecurityNamespace e) => e.NamespaceInfo.Name.Equals(objectClassId, StringComparison.OrdinalIgnoreCase));
			if (geminiSecurityNamespace == null)
			{
				return null;
			}
			foreach (ActionDefinition action in geminiSecurityNamespace.NamespaceInfo.Actions)
			{
				SecurityAction securityAction = new SecurityAction();
				securityAction.ActionId = action.Name;
				securityAction.DisplayName = action.DisplayName;
				list.Add(securityAction);
			}
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsReaderLockHeld)
			{
				_namespaceLock.ReleaseReaderLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "QueryObjectClassActions");
		}
		return list.ToArray();
	}

	public void CreateObjectClass(IGeminiRequestContext requestContext, string objectClassId, string[] classActions)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "CreateObjectClass");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			SecurityNamespaceInfo securityNamespaceInfo = new SecurityNamespaceInfo();
			securityNamespaceInfo.NamespaceId = objectClassId;
			securityNamespaceInfo.Name = objectClassId;
			securityNamespaceInfo.DisplayName = objectClassId;
			securityNamespaceInfo.Separator = '/';
			securityNamespaceInfo.Structure = 1;
			securityNamespaceInfo.UserDefined = true;
			List<ActionDefinition> list = new List<ActionDefinition>();
			foreach (string text in classActions)
			{
				ActionDefinition actionDefinition = new ActionDefinition();
				actionDefinition.NamespaceId = securityNamespaceInfo.NamespaceId;
				actionDefinition.Name = text;
				actionDefinition.DisplayName = text;
				actionDefinition.Visible = true;
				list.Add(actionDefinition);
			}
			securityNamespaceInfo.Actions = list;
			ValidateSecurityActions(securityNamespaceInfo);
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				securityComponent.CreateSecurityNamespace(securityNamespaceInfo, Guid.Empty.ToString());
			}
			_namespaceLock.AcquireWriterLock(-1);
			_securityNamespaces[securityNamespaceInfo.NamespaceGuid] = new GeminiSecurityNamespace(requestContext, securityNamespaceInfo);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsWriterLockHeld)
			{
				_namespaceLock.ReleaseWriterLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "CreateObjectClass");
		}
	}

	public void DeleteObjectClass(IGeminiRequestContext requestContext, string objectClassId)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "DeleteObjectClass");
		try
		{
			ValidateRequestContext(requestContext);
			EnsureNamespacesLoaded(requestContext);
			IGeminiSecurityNamespace geminiSecurityNamespace = null;
			_namespaceLock.AcquireReaderLock(-1);
			geminiSecurityNamespace = _securityNamespaces.Values.FirstOrDefault((IGeminiSecurityNamespace e) => e.NamespaceInfo.Name == objectClassId);
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			securityComponent.DeleteSecurityNamespace(geminiSecurityNamespace.NamespaceInfo.NamespaceGuid);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			if (_namespaceLock.IsReaderLockHeld)
			{
				_namespaceLock.ReleaseReaderLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "DeleteObjectClass");
		}
	}

	public void StartService(IGeminiRequestContext systemRequestContext)
	{
	}

	public void StopService(IGeminiRequestContext systemRequestContext)
	{
	}

	public static void GetLegacyInformation(Guid namespaceId, string actionId, out string legacySecurityArea, out string legacySecurityAttributeId)
	{
		LegacySecurityActionInformation securityActionInformation = LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(namespaceId, actionId);
		if (securityActionInformation != null)
		{
			legacySecurityArea = securityActionInformation.LegacySecurityArea;
			legacySecurityAttributeId = securityActionInformation.LegacySecurityAttribute;
		}
		else
		{
			legacySecurityArea = null;
			legacySecurityAttributeId = null;
		}
	}

	public bool IsAdministrator(IGeminiRequestContext requestContext)
	{
		Validation.CheckRequestContext(requestContext);
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityManagerService", "IsAdministrator");
		try
		{
			Identity identity = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded);
			return identity != null && identity.MemberOfRoles != null && identity.MemberOfRoles.Any((Guid f) => f == SecurityManager.AdministratorRoleId);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityManagerService", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityManagerService", "IsAdministrator");
		}
	}
}
