using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Management.Server;

public class GeminiSecurityNamespace : IGeminiSecurityNamespace
{
	private const string _area = "Security";

	private const string _layer = "SecurityNamespace";

	private SecurityNamespaceInfo _description;

	private bool _tokenStoreLoaded;

	private ITokenStore<AccessControlList> _tokenStore;

	private ReaderWriterLock _accessLock = new ReaderWriterLock();

	private string _securityEvaluatorKey;

	public SecurityNamespaceInfo NamespaceInfo => _description;

	public bool IsHierachical => _description.Structure == 1;

	public GeminiSecurityNamespace(IGeminiRequestContext requestContext, SecurityNamespaceInfo description)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "GeminiSecurityNamespace");
		try
		{
			_description = description;
			_securityEvaluatorKey = _description.NamespaceId;
			_tokenStoreLoaded = false;
			switch ((SecurityNamespaceStructure)_description.Structure)
			{
			case SecurityNamespaceStructure.Flat:
				_tokenStore = new FlatStore<AccessControlList>(StringComparison.OrdinalIgnoreCase);
				break;
			case SecurityNamespaceStructure.Hierarchical:
				_tokenStore = new HierarchicalStore<AccessControlList>(_description.Separator, StringComparison.OrdinalIgnoreCase);
				break;
			default:
				throw new ArgumentException("Invalid namespace structure");
			}
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "GeminiSecurityNamespace");
		}
	}

	private void EnsureNamespaceLoaded(IGeminiRequestContext requestContext)
	{
		if (_tokenStoreLoaded)
		{
			return;
		}
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "EnsureNamespaceLoaded");
		try
		{
			_accessLock.AcquireWriterLock(-1);
			if (_tokenStoreLoaded)
			{
				return;
			}
			if (requestContext.Items.ContainsKey(_securityEvaluatorKey))
			{
				requestContext.Items.Remove(_securityEvaluatorKey);
			}
			List<string> list = new List<string>();
			List<AccessControlEntry2> items;
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				ResultCollection resultCollection = securityComponent.QuerySecurityAces(_description.NamespaceId, IsHierachical);
				items = resultCollection.GetCurrent<AccessControlEntry2>().Items;
				if (IsHierachical)
				{
					resultCollection.NextResult();
					list = resultCollection.GetCurrent<string>().Items;
				}
			}
			Dictionary<string, AccessControlList> dictionary = new Dictionary<string, AccessControlList>();
			foreach (AccessControlEntry2 item in items)
			{
				AccessControlEntryDetails accessControlEntryDetails = item as AccessControlEntryDetails;
				if (!dictionary.TryGetValue(accessControlEntryDetails.Token, out var value))
				{
					bool inheritPermissions = IsHierachical && !list.Contains(accessControlEntryDetails.Token);
					value = new AccessControlList(accessControlEntryDetails.Token, inheritPermissions);
					dictionary.Add(accessControlEntryDetails.Token, value);
				}
				value.SetAccessControlEntry(item, merge: false);
			}
			_tokenStore.Clear();
			foreach (string key in dictionary.Keys)
			{
				_tokenStore[key] = dictionary[key];
			}
			_tokenStoreLoaded = true;
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			if (_accessLock.IsWriterLockHeld)
			{
				_accessLock.ReleaseWriterLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "EnsureNamespaceLoaded");
		}
	}

	public static void EvaluatePermissions(IGeminiRequestContext requestContext, IIdentityService identityService, IdentityDescriptor descriptor, AccessControlList accessControlList, out ulong effectiveAllow, out ulong effectiveDeny)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "EvaluatePermissions");
		try
		{
			effectiveAllow = 0uL;
			effectiveDeny = 0uL;
			foreach (AccessControlEntry2 accessControlEntry in accessControlList.AccessControlEntries)
			{
				if (IdentityDescriptorComparer.Instance.Equals(accessControlEntry.Descriptor, descriptor))
				{
					effectiveAllow |= accessControlEntry.Allow;
					effectiveDeny |= accessControlEntry.Deny;
				}
				else if (identityService.IsMember(requestContext, accessControlEntry.Descriptor, descriptor))
				{
					effectiveAllow |= accessControlEntry.Allow;
					effectiveDeny |= accessControlEntry.Deny;
				}
			}
			effectiveAllow &= ~effectiveDeny;
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, TraceLevel.Error, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "EvaluatePermissions");
		}
	}

	public void CheckPermission(IGeminiRequestContext requestContext, string token, string requestedPermission)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "CheckPermission");
		try
		{
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			SecurityManagerService.GetLegacyInformation(_description.NamespaceGuid, requestedPermission, out var legacySecurityArea, out var legacySecurityAttributeId);
			SecurityManagerService.MapLegacyAction(_description.NamespaceGuid, ref requestedPermission);
			securityComponent.HasPermission(_description.NamespaceGuid, requestContext.AuthenticatedUserSid, requestedPermission, token, legacySecurityArea, legacySecurityAttributeId);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "CheckPermission");
		}
	}

	public bool HasPermission(IGeminiRequestContext requestContext, string token, ulong requestedPermission, bool checkAdministrator)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "HasPermission");
		try
		{
			EnsureNamespaceLoaded(requestContext);
			ISecurityEvaluator tokenStoreEvaluator = GetTokenStoreEvaluator(requestContext, _tokenStore);
			token = FormatToken(token);
			return tokenStoreEvaluator.HasPermission(requestContext, token, requestedPermission, checkAdministrator);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "HasPermission");
		}
	}

	public bool HasPermission(IGeminiRequestContext requestContext, string identitySid, string actionId, string objectId, bool checkAdministrator)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "HasPermission");
		try
		{
			EnsureNamespaceLoaded(requestContext);
			ISecurityEvaluator tokenStoreEvaluator = GetTokenStoreEvaluator(requestContext, _tokenStore);
			objectId = FormatToken(objectId);
			return tokenStoreEvaluator.HasPermission(requestedPermission: GetPermissionBitFromActionId(actionId), requestContext: requestContext, token: objectId, descriptor: new IdentityDescriptor(identitySid), alwaysAllowAdministrator: checkAdministrator);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "HasPermission");
		}
	}

	public AccessControlEntry2 QueryPermissionMask(IGeminiRequestContext requestContext, string token)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "QueryPermissionMask");
		try
		{
			EnsureNamespaceLoaded(requestContext);
			ISecurityEvaluator tokenStoreEvaluator = GetTokenStoreEvaluator(requestContext, _tokenStore);
			token = FormatToken(token);
			return tokenStoreEvaluator.QueryEffectivePermissions(requestContext, token, requestContext.UserContext);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "QueryPermissionMask");
		}
	}

	public AccessControlEntry2 QueryPermissionMaskForOther(IGeminiRequestContext requestContext, string token, string userSid)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "QueryPermissionMask");
		try
		{
			IdentityDescriptor identity = new IdentityDescriptor(userSid);
			EnsureNamespaceLoaded(requestContext);
			ISecurityEvaluator tokenStoreEvaluator = GetTokenStoreEvaluator(requestContext, _tokenStore);
			token = FormatToken(token);
			return tokenStoreEvaluator.QueryEffectivePermissions(requestContext, token, identity);
		}
		catch (Exception ex)
		{
			requestContext.RequestTracer.TraceException(0, "Security", "SecurityNamespace", ex);
			throw;
		}
		finally
		{
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "QueryPermissionMask");
		}
	}

	public void ForceNamespaceReload(IGeminiRequestContext requestContext)
	{
		requestContext.RequestTracer.TraceEnter(0, "Security", "SecurityNamespace", "ForceNamespaceReload");
		try
		{
			_accessLock.AcquireReaderLock(-1);
			_tokenStoreLoaded = false;
		}
		finally
		{
			if (_accessLock.IsReaderLockHeld)
			{
				_accessLock.ReleaseReaderLock();
			}
			requestContext.RequestTracer.TraceLeave(0, "Security", "SecurityNamespace", "ForceNamespaceReload");
		}
	}

	private ulong GetPermissionBitFromActionId(string actionId)
	{
		List<ActionDefinition> actions = _description.Actions;
		ActionDefinition actionDefinition = actions.FirstOrDefault((ActionDefinition e) => e.Name.Equals(actionId, StringComparison.OrdinalIgnoreCase));
		if (actionDefinition == null)
		{
			LegacySecurityActionInformation legacyInfo = LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(actionId);
			if (legacyInfo != null)
			{
				actionDefinition = actions.FirstOrDefault((ActionDefinition e) => e.Name.Equals(legacyInfo.SecurityAction, StringComparison.OrdinalIgnoreCase));
			}
		}
		if (actionDefinition == null)
		{
			throw new SecurityActionDoesNotExistException(actionId);
		}
		return actionDefinition.PermissionBit;
	}

	public bool HasAction(string actionId)
	{
		ActionDefinition actionDefinition = _description.Actions.FirstOrDefault((ActionDefinition e) => e.Name.Equals(actionId, StringComparison.OrdinalIgnoreCase));
		if (actionDefinition == null)
		{
			LegacySecurityActionInformation legacyInfo = LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(actionId);
			if (legacyInfo != null)
			{
				actionDefinition = _description.Actions.FirstOrDefault((ActionDefinition e) => e.Name.Equals(legacyInfo.SecurityAction, StringComparison.OrdinalIgnoreCase));
			}
		}
		if (actionDefinition != null)
		{
			return true;
		}
		return false;
	}

	private static string FormatToken(string token)
	{
		if (string.IsNullOrEmpty(token) || token.Equals("XPCO", StringComparison.OrdinalIgnoreCase))
		{
			return token;
		}
		token = token.Insert(0, (token[0] == '/') ? "XPCO" : "XPCO/");
		token = token.TrimEnd('/');
		return token;
	}

	private ISecurityEvaluator GetTokenStoreEvaluator(IGeminiRequestContext requestContext, ITokenStore<AccessControlList> tokenStore)
	{
		object obj = null;
		IIdentityService identityService = requestContext.GetIdentityService();
		switch ((SecurityNamespaceStructure)_description.Structure)
		{
		case SecurityNamespaceStructure.Flat:
			obj = new FlatTokenStoreEvaluator((FlatStore<AccessControlList>)tokenStore, identityService);
			break;
		case SecurityNamespaceStructure.Hierarchical:
			obj = new HierarchicalTokenStoreEvaluator((HierarchicalStore<AccessControlList>)tokenStore, this, identityService);
			break;
		}
		return (ISecurityEvaluator)obj;
	}
}
