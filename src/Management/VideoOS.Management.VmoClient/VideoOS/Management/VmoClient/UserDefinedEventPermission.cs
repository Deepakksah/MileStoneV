using System;
using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

public sealed class UserDefinedEventPermission : IEquatable<UserDefinedEventPermission>
{
	public UserDefinedEventPermissionSetValue Value { get; private set; }

	public static UserDefinedEventPermission Read
	{
		[DebuggerStepThrough]
		get
		{
			return new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Read);
		}
	}

	public static UserDefinedEventPermission Write
	{
		[DebuggerStepThrough]
		get
		{
			return new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Edit);
		}
	}

	public static UserDefinedEventPermission Delete
	{
		[DebuggerStepThrough]
		get
		{
			return new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Delete);
		}
	}

	public static UserDefinedEventPermission Trigger
	{
		[DebuggerStepThrough]
		get
		{
			return new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Trigger);
		}
	}

	public static UserDefinedEventPermission AdministrateSecurity
	{
		[DebuggerStepThrough]
		get
		{
			return new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.AdministrateSecurity);
		}
	}

	public UserDefinedEventPermission(UserDefinedEventPermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static UserDefinedEventPermissionSet Add(UserDefinedEventPermission permissionLeft, UserDefinedEventPermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static UserDefinedEventPermissionSet operator +(UserDefinedEventPermission permissionLeft, UserDefinedEventPermission permissionRight)
	{
		UserDefinedEventPermissionSet userDefinedEventPermissionSet = new UserDefinedEventPermissionSet(permissionLeft);
		userDefinedEventPermissionSet.SetBit(permissionRight);
		return userDefinedEventPermissionSet;
	}

	public static UserDefinedEventPermissionSet BitwiseOr(UserDefinedEventPermission permissionLeft, UserDefinedEventPermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static UserDefinedEventPermissionSet operator |(UserDefinedEventPermission permissionLeft, UserDefinedEventPermission permissionRight)
	{
		UserDefinedEventPermissionSet userDefinedEventPermissionSet = new UserDefinedEventPermissionSet(permissionLeft);
		userDefinedEventPermissionSet.SetBit(permissionRight);
		return userDefinedEventPermissionSet;
	}

	public static bool Equals(UserDefinedEventPermission permissionLeft, UserDefinedEventPermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(UserDefinedEventPermission permissionLeft, UserDefinedEventPermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(UserDefinedEventPermission permissionLeft, UserDefinedEventPermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static UserDefinedEventPermissionSet ToUserDefinedEventPermissionSet(UserDefinedEventPermission permission)
	{
		return new UserDefinedEventPermissionSet(permission);
	}

	public static implicit operator UserDefinedEventPermissionSet(UserDefinedEventPermission permission)
	{
		return new UserDefinedEventPermissionSet(permission);
	}

	public bool Equals(UserDefinedEventPermission other)
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
		if (!(obj is UserDefinedEventPermission other))
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
