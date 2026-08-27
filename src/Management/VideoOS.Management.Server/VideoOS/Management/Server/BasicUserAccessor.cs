using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class BasicUserAccessor : IdentityAccessor
{
	public BasicUserAccessor(string sid)
		: base(sid, null, null, AccountType.SidTypeUnknown)
	{
	}

	public override Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers)
	{
		return MapUserToIdentity(requestContext.GetService<IdpClientService>().CreateUserIdpComponent().GetVmsUserById(_sid));
	}

	public static Identity MapUserToIdentity(BasicUser basicUser)
	{
		return new Identity
		{
			Type = IdentityType.BasicUser,
			Sid = basicUser.Sid,
			AccountName = basicUser.Name,
			DisplayName = basicUser.Name,
			Domain = "[BASIC]",
			DistinguishedName = string.Empty,
			Description = basicUser.Description
		};
	}
}
