namespace VideoOS.Management.Server;

public class FlatTokenStoreEvaluator : SecurityEvaluatorBase
{
	private FlatStore<AccessControlList> _aclStore;

	public FlatTokenStoreEvaluator(FlatStore<AccessControlList> aclStore, IIdentityService identityService)
	{
		_aclStore = aclStore;
		base.IdentityService = identityService;
	}

	public override AccessControlEntry2 QueryEffectivePermissions(IGeminiRequestContext requestContext, string token, IdentityDescriptor descriptor)
	{
		if (_aclStore.TryGetValue(token, out var referencedObject))
		{
			GeminiSecurityNamespace.EvaluatePermissions(requestContext, base.IdentityService, descriptor, referencedObject, out var effectiveAllow, out var effectiveDeny);
			return new AccessControlEntry2(descriptor, effectiveAllow, effectiveDeny);
		}
		return new AccessControlEntry2(descriptor, 0uL, 0uL);
	}
}
