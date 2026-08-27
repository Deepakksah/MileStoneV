using System;
using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

public sealed class PublicViewGroupPermission : IEquatable<PublicViewGroupPermission>
{
	public PublicViewGroupPermissionSetValue Value { get; private set; }

	public static PublicViewGroupPermission Read
	{
		[DebuggerStepThrough]
		get
		{
			return new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Read);
		}
	}

	public static PublicViewGroupPermission Write
	{
		[DebuggerStepThrough]
		get
		{
			return new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Edit);
		}
	}

	public static PublicViewGroupPermission Delete
	{
		[DebuggerStepThrough]
		get
		{
			return new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Delete);
		}
	}

	public static PublicViewGroupPermission Operate
	{
		[DebuggerStepThrough]
		get
		{
			return new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Operate);
		}
	}

	public static PublicViewGroupPermission AdministrateSecurity
	{
		[DebuggerStepThrough]
		get
		{
			return new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.AdministrateSecurity);
		}
	}

	public PublicViewGroupPermission(PublicViewGroupPermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static PublicViewGroupPermissionSet Add(PublicViewGroupPermission permissionLeft, PublicViewGroupPermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static PublicViewGroupPermissionSet operator +(PublicViewGroupPermission permissionLeft, PublicViewGroupPermission permissionRight)
	{
		PublicViewGroupPermissionSet publicViewGroupPermissionSet = new PublicViewGroupPermissionSet(permissionLeft);
		publicViewGroupPermissionSet.SetBit(permissionRight);
		return publicViewGroupPermissionSet;
	}

	public static PublicViewGroupPermissionSet BitwiseOr(PublicViewGroupPermission permissionLeft, PublicViewGroupPermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static PublicViewGroupPermissionSet operator |(PublicViewGroupPermission permissionLeft, PublicViewGroupPermission permissionRight)
	{
		PublicViewGroupPermissionSet publicViewGroupPermissionSet = new PublicViewGroupPermissionSet(permissionLeft);
		publicViewGroupPermissionSet.SetBit(permissionRight);
		return publicViewGroupPermissionSet;
	}

	public static bool Equals(PublicViewGroupPermission permissionLeft, PublicViewGroupPermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(PublicViewGroupPermission permissionLeft, PublicViewGroupPermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(PublicViewGroupPermission permissionLeft, PublicViewGroupPermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static PublicViewGroupPermissionSet ToPublicViewGroupPermissionSet(PublicViewGroupPermission permission)
	{
		return new PublicViewGroupPermissionSet(permission);
	}

	public static implicit operator PublicViewGroupPermissionSet(PublicViewGroupPermission permission)
	{
		return new PublicViewGroupPermissionSet(permission);
	}

	public bool Equals(PublicViewGroupPermission other)
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
		if (!(obj is PublicViewGroupPermission other))
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
