using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class HierarchicalTokenStoreEvaluator : SecurityEvaluatorBase
{
	private class QueryEffectivePermissionsCache
	{
		private class EnumParentsState
		{
			public IGeminiRequestContext RequestContext { get; set; }

			public string Token { get; set; }

			public string TokenToCache { get; set; }

			public IdentityDescriptor Identity { get; set; }

			public ulong Allow { get; set; }

			public ulong Deny { get; set; }

			public void Prepare(IGeminiRequestContext requestContext, string token, IdentityDescriptor identity)
			{
				RequestContext = requestContext;
				Token = token;
				Identity = identity;
				TokenToCache = null;
				Allow = 0uL;
				Deny = 0uL;
			}
		}

		private class EffectivePermissionsCacheEntry
		{
			public AccessControlEntry2 EffectivePermissions { get; set; }

			public string Token { get; set; }

			public EffectivePermissionsCacheEntry(EnumParentsState state, AccessControlEntry2 effectivePermissions)
			{
				Token = state.TokenToCache;
				EffectivePermissions = effectivePermissions;
			}
		}

		private List<EffectivePermissionsCacheEntry> _cache;

		private HierarchicalStore<AccessControlList> _aclStore;

		private GeminiSecurityNamespace _securityNamespace;

		private EnumParentsState _enumParentsState;

		private IIdentityService _identityService;

		private const int _cacheSize = 2;

		public QueryEffectivePermissionsCache(HierarchicalStore<AccessControlList> aclStore, GeminiSecurityNamespace securityNamespace, IIdentityService identityService)
		{
			_aclStore = aclStore;
			_securityNamespace = securityNamespace;
			_identityService = identityService;
			_enumParentsState = new EnumParentsState();
			_cache = new List<EffectivePermissionsCacheEntry>(2);
		}

		public AccessControlEntry2 QueryEffectivePermissions(IGeminiRequestContext requestContext, string token, IdentityDescriptor identity)
		{
			if (requestContext.IsSystemContext || !TryCache(requestContext, token, identity, out var effectivePermissions))
			{
				_enumParentsState.Prepare(requestContext, token, identity);
				_aclStore.EnumParents(token, EnumParentPermission, _enumParentsState);
				effectivePermissions = new AccessControlEntry2(identity, _enumParentsState.Allow, _enumParentsState.Deny);
				if (_enumParentsState.TokenToCache != null)
				{
					CacheToken(_enumParentsState, effectivePermissions);
				}
			}
			return effectivePermissions;
		}

		private bool EnumParentPermission(AccessControlList accessControlList, object param)
		{
			EnumParentsState enumParentsState = (EnumParentsState)param;
			ulong effectiveAllow = 0uL;
			ulong effectiveDeny = 0uL;
			GeminiSecurityNamespace.EvaluatePermissions(enumParentsState.RequestContext, _identityService, enumParentsState.Identity, accessControlList, out effectiveAllow, out effectiveDeny);
			enumParentsState.Allow |= effectiveAllow;
			enumParentsState.Allow &= ~effectiveDeny;
			enumParentsState.Deny |= effectiveDeny;
			return !accessControlList.InheritPermissions;
		}

		private bool TryCache(IGeminiRequestContext requestContext, string token, IdentityDescriptor identity, out AccessControlEntry2 effectivePermissions)
		{
			for (int num = _cache.Count - 1; num >= 0; num--)
			{
				EffectivePermissionsCacheEntry effectivePermissionsCacheEntry = _cache[num];
				if (IdentityDescriptorComparer.Instance.Equals(identity, effectivePermissionsCacheEntry.EffectivePermissions.Descriptor) && _aclStore.IsSubItem(token, effectivePermissionsCacheEntry.Token))
				{
					_cache.RemoveAt(num);
				}
			}
			effectivePermissions = null;
			return false;
		}

		private void CacheToken(EnumParentsState state, AccessControlEntry2 effectivePermissions)
		{
			if (_cache.Count >= 2)
			{
				_cache.RemoveAt(0);
			}
			_cache.Add(new EffectivePermissionsCacheEntry(state, effectivePermissions));
		}
	}

	private HierarchicalStore<AccessControlList> _aclStore;

	private QueryEffectivePermissionsCache _queryEffectivePermissionsCache;

	public HierarchicalTokenStoreEvaluator(HierarchicalStore<AccessControlList> aclStore, GeminiSecurityNamespace securityNamespace, IIdentityService identityService)
	{
		_aclStore = aclStore;
		base.SecurityNamespace = securityNamespace;
		base.IdentityService = identityService;
		_queryEffectivePermissionsCache = new QueryEffectivePermissionsCache(_aclStore, securityNamespace, identityService);
	}

	public override AccessControlEntry2 QueryEffectivePermissions(IGeminiRequestContext requestContext, string token, IdentityDescriptor identity)
	{
		return _queryEffectivePermissionsCache.QueryEffectivePermissions(requestContext, token, identity);
	}
}
