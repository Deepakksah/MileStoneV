using System;
using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

public sealed class SitePermission : IEquatable<SitePermission>
{
	public SitePermissionSetValue Value { get; private set; }

	public static SitePermission Read
	{
		[DebuggerStepThrough]
		get
		{
			return new SitePermission(SitePermissionSetValue.Read);
		}
	}

	public SitePermission(SitePermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static SitePermissionSet operator +(SitePermission permissionLeft, SitePermission permissionRight)
	{
		return Add(permissionLeft, permissionRight);
	}

	public static SitePermissionSet Add(SitePermission permissionLeft, SitePermission permissionRight)
	{
		SitePermissionSet sitePermissionSet = new SitePermissionSet(permissionLeft);
		sitePermissionSet.SetBit(permissionRight);
		return sitePermissionSet;
	}

	public static SitePermissionSet operator |(SitePermission permissionLeft, SitePermission permissionRight)
	{
		return BitwiseOr(permissionLeft, permissionRight);
	}

	public static SitePermissionSet BitwiseOr(SitePermission permissionLeft, SitePermission permissionRight)
	{
		SitePermissionSet sitePermissionSet = new SitePermissionSet(permissionLeft);
		sitePermissionSet.SetBit(permissionRight);
		return sitePermissionSet;
	}

	public static bool operator ==(SitePermission permissionLeft, SitePermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(SitePermission permissionLeft, SitePermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static implicit operator SitePermissionSet(SitePermission permission)
	{
		return FromSitePermission(permission);
	}

	public static SitePermissionSet FromSitePermission(SitePermission permission)
	{
		return new SitePermissionSet(permission);
	}

	public bool Equals(SitePermission other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		return Value == other.Value;
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is SitePermission other))
		{
			return false;
		}
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return Value.GetHashCode();
	}
}
