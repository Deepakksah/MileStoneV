using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class GenericIdentityAccessor : IdentityAccessor
{
	public GenericIdentityAccessor(string sid, string domain, string userName, AccountType accountType)
		: base(sid, domain, userName, accountType)
	{
	}

	public override Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers)
	{
		Identity identity = new Identity();
		identity.AccountName = _userName;
		identity.DisplayName = _userName;
		identity.DistinguishedName = string.Empty;
		identity.Description = string.Empty;
		identity.Domain = _domain;
		identity.Sid = _sid;
		switch (_accountType)
		{
		case AccountType.SidTypeUser:
			identity.Type = IdentityType.WindowsUser;
			break;
		case AccountType.SidTypeGroup:
		case AccountType.SidTypeWellKnownGroup:
			identity.Type = IdentityType.WindowsGroup;
			break;
		default:
			identity.Type = IdentityType.UnknownIdentityType;
			break;
		}
		return identity;
	}
}
