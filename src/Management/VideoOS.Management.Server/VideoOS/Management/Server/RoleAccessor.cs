using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class RoleAccessor : IdentityAccessor
{
	public RoleAccessor(string sid)
		: base(sid, null, null, AccountType.SidTypeUnknown)
	{
	}

	public override Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers)
	{
		using IRoleComponent roleComponent = requestContext.CreateRoleComponent();
		string sid = _sid;
		Role role = roleComponent.QueryRoleById(sid);
		if (role == null)
		{
			return null;
		}
		Identity identity = new Identity();
		identity.Type = IdentityType.Role;
		identity.Sid = sid.ToString();
		identity.AccountName = string.Empty;
		identity.DisplayName = role.Name;
		identity.Domain = string.Empty;
		identity.DistinguishedName = string.Empty;
		identity.Description = role.Description;
		if (includeMembers)
		{
			string[] array = roleComponent.QueryRoleMembersWithDeleted(sid, includeDynamicallyAddedIdentities: true);
			identity.Members = new string[array.Length];
			array.CopyTo(identity.Members, 0);
		}
		return identity;
	}
}
