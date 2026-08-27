using VideoOS.Management.Common;

namespace VideoOS.Server.Admin.Utilities;

public class RoleAccessor : IdentityAccessor
{
	private Role _role;

	private string[] _members;

	public RoleAccessor(string sid, Role role, string[] members)
		: base(sid, null, null, AccountType.SidTypeUnknown)
	{
		_role = role;
		_members = members;
	}

	public override Identity QueryIdentity(bool includeMembers)
	{
		string sid = _sid;
		Identity identity = new Identity();
		identity.Type = IdentityType.Role;
		identity.Sid = sid.ToString();
		identity.AccountName = string.Empty;
		identity.DisplayName = _role.Name;
		identity.Domain = string.Empty;
		identity.DistinguishedName = string.Empty;
		identity.Description = _role.Description;
		if (includeMembers)
		{
			identity.Members = new string[_members.Length];
			_members.CopyTo(identity.Members, 0);
		}
		return identity;
	}
}
