using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class AccessControlList
{
	private Dictionary<IdentityDescriptor, AccessControlEntry2> _accessControlEntries;

	public string Token { get; set; }

	public IEnumerable<AccessControlEntry2> AccessControlEntries => _accessControlEntries.Values;

	public bool InheritPermissions { get; set; }

	public AccessControlList(string token, bool inheritPermissions)
		: this(token, inheritPermissions, null)
	{
	}

	public AccessControlList(string token, bool inheritPermissions, IEnumerable<AccessControlEntry2> accessControlEntries)
	{
		Token = token;
		InheritPermissions = inheritPermissions;
		_accessControlEntries = new Dictionary<IdentityDescriptor, AccessControlEntry2>(IdentityDescriptorComparer.Instance);
		if (accessControlEntries != null)
		{
			SetAccessControlEntries(accessControlEntries, merge: false);
		}
	}

	public void SetAccessControlEntry(AccessControlEntry2 accessControlEntry, bool merge)
	{
		SetAccessControlEntries(new AccessControlEntry2[1] { accessControlEntry }, merge);
	}

	private void SetAccessControlEntries(IEnumerable<AccessControlEntry2> accessControlEntries, bool merge)
	{
		if (accessControlEntries == null)
		{
			throw new ArgumentNullException("accessControlEntries");
		}
		foreach (AccessControlEntry2 accessControlEntry in accessControlEntries)
		{
			if (_accessControlEntries.TryGetValue(accessControlEntry.Descriptor, out var value))
			{
				if (merge)
				{
					ulong updatedAllow = 0uL;
					ulong updatedDeny = 0uL;
					MergePermissions(value.Allow, value.Deny, accessControlEntry.Allow, accessControlEntry.Deny, 0uL, out updatedAllow, out updatedDeny);
					value.Allow = updatedAllow;
					value.Deny = updatedDeny;
				}
				else
				{
					value.Allow = accessControlEntry.Allow;
					value.Deny = accessControlEntry.Deny;
				}
			}
			else
			{
				_accessControlEntries[accessControlEntry.Descriptor] = new AccessControlEntry2(accessControlEntry.Descriptor, accessControlEntry.Allow & ~accessControlEntry.Deny, accessControlEntry.Deny);
			}
		}
	}

	private static void MergePermissions(ulong existingAllow, ulong existingDeny, ulong newAllow, ulong newDeny, ulong remove, out ulong updatedAllow, out ulong updatedDeny)
	{
		updatedAllow = ((existingAllow & ~remove) | newAllow) & ~newDeny;
		updatedDeny = ((existingDeny & ~remove) | newDeny) & ~updatedAllow;
	}
}
