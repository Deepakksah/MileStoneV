using System;
using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

public sealed class ManagementServerPermission : IEquatable<ManagementServerPermission>
{
	public ManagementServerPermissionSetValue Value { get; private set; }

	public static ManagementServerPermission Connect
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.Connect);
		}
	}

	public static ManagementServerPermission Read
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.Read);
		}
	}

	public static ManagementServerPermission Edit
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.Edit);
		}
	}

	public static ManagementServerPermission AdministrateSecurity
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.AdministrateSecurity);
		}
	}

	public static ManagementServerPermission StatusApi
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.StatusApi);
		}
	}

	public static ManagementServerPermission AdministrateFederatedSiteHierarchy
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.AdministrateFederatedSiteHierarchy);
		}
	}

	public static ManagementServerPermission BackupConfiguration
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.BackupConfiguration);
		}
	}

	public static ManagementServerPermission AuthorizeUser
	{
		[DebuggerStepThrough]
		get
		{
			return new ManagementServerPermission(ManagementServerPermissionSetValue.AuthorizeUser);
		}
	}

	public ManagementServerPermission(ManagementServerPermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static ManagementServerPermissionSet Add(ManagementServerPermission permissionLeft, ManagementServerPermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static ManagementServerPermissionSet operator +(ManagementServerPermission permissionLeft, ManagementServerPermission permissionRight)
	{
		ManagementServerPermissionSet managementServerPermissionSet = new ManagementServerPermissionSet(permissionLeft);
		managementServerPermissionSet.SetBit(permissionRight);
		return managementServerPermissionSet;
	}

	public static ManagementServerPermissionSet BitwiseOr(ManagementServerPermission permissionLeft, ManagementServerPermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static ManagementServerPermissionSet operator |(ManagementServerPermission permissionLeft, ManagementServerPermission permissionRight)
	{
		ManagementServerPermissionSet managementServerPermissionSet = new ManagementServerPermissionSet(permissionLeft);
		managementServerPermissionSet.SetBit(permissionRight);
		return managementServerPermissionSet;
	}

	public static bool Equals(ManagementServerPermission permissionLeft, ManagementServerPermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(ManagementServerPermission permissionLeft, ManagementServerPermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(ManagementServerPermission permissionLeft, ManagementServerPermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static ManagementServerPermissionSet ToManagementServerPermissionSet(ManagementServerPermission permission)
	{
		return new ManagementServerPermissionSet(permission);
	}

	public static implicit operator ManagementServerPermissionSet(ManagementServerPermission permission)
	{
		return new ManagementServerPermissionSet(permission);
	}

	public bool Equals(ManagementServerPermission other)
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
		if (!(obj is ManagementServerPermission other))
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
