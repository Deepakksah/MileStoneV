using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class IdleAccessor : IdentityAccessor
{
	public IdleAccessor(string sid)
		: base(sid, null, null, AccountType.SidTypeUnknown)
	{
	}

	public override Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers)
	{
		return null;
	}
}
