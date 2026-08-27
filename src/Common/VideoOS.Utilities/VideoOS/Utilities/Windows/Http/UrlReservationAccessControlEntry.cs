using System;
using System.Diagnostics;
using System.Security.Principal;
using VideoOS.Utilities.Windows.Http.Internal;

namespace VideoOS.Utilities.Windows.Http;

[DebuggerDisplay("{PermissionFlags} : {Identity}")]
public class UrlReservationAccessControlEntry
{
	public SecurityIdentifier Identity { get; }

	public AccessMask PermissionFlags{ get; }

	public bool HasListenPermission
	{
		get
		{
			if ((PermissionFlags & AccessMask.GENERIC_EXECUTE) != AccessMask.GENERIC_EXECUTE)
			{
				return (PermissionFlags & AccessMask.GENERIC_ALL) == AccessMask.GENERIC_ALL;
			}
			return true;
		}
	}

	public bool HasDelegatePermission
	{
		get
		{
			if ((PermissionFlags & AccessMask.GENERIC_WRITE) != AccessMask.GENERIC_WRITE)
			{
				return (PermissionFlags & AccessMask.GENERIC_ALL) == AccessMask.GENERIC_ALL;
			}
			return true;
		}
	}

	public UrlReservationAccessControlEntry(SecurityIdentifier identity, AccessMask permissionFlags)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		Identity = identity;
		PermissionFlags = permissionFlags;
	}

	public UrlReservationAccessControlEntry(SecurityIdentifier identity, bool allowListen, bool allowDelegate)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		Identity = identity;
		AccessMask accessMask = AccessMask.None;
		if (allowDelegate)
		{
			accessMask |= AccessMask.GENERIC_WRITE;
		}
		if (allowListen)
		{
			accessMask |= AccessMask.GENERIC_EXECUTE;
		}
		PermissionFlags = accessMask;
	}
}
