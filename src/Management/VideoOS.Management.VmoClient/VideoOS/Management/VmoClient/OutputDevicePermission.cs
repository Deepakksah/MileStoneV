using System;

namespace VideoOS.Management.VmoClient;

public sealed class OutputDevicePermission : IEquatable<OutputDevicePermission>
{
	public OutputDevicePermissionSetValue Value { get; private set; }

	public static OutputDevicePermission Read => new OutputDevicePermission(OutputDevicePermissionSetValue.Read);

	public static OutputDevicePermission Edit => new OutputDevicePermission(OutputDevicePermissionSetValue.Edit);

	public static OutputDevicePermission Activate => new OutputDevicePermission(OutputDevicePermissionSetValue.Activate);

	public static OutputDevicePermission AdministrateSecurity => new OutputDevicePermission(OutputDevicePermissionSetValue.AdministrateSecurity);

	public OutputDevicePermission(OutputDevicePermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static OutputDevicePermissionSet Add(OutputDevicePermission permissionLeft, OutputDevicePermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static OutputDevicePermissionSet operator +(OutputDevicePermission permissionLeft, OutputDevicePermission permissionRight)
	{
		OutputDevicePermissionSet outputDevicePermissionSet = new OutputDevicePermissionSet(permissionLeft);
		outputDevicePermissionSet.SetBit(permissionRight);
		return outputDevicePermissionSet;
	}

	public static OutputDevicePermissionSet BitwiseOr(OutputDevicePermission permissionLeft, OutputDevicePermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static OutputDevicePermissionSet operator |(OutputDevicePermission permissionLeft, OutputDevicePermission permissionRight)
	{
		OutputDevicePermissionSet outputDevicePermissionSet = new OutputDevicePermissionSet(permissionLeft);
		outputDevicePermissionSet.SetBit(permissionRight);
		return outputDevicePermissionSet;
	}

	public static bool Equals(OutputDevicePermission permissionLeft, OutputDevicePermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(OutputDevicePermission permissionLeft, OutputDevicePermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(OutputDevicePermission permissionLeft, OutputDevicePermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static OutputDevicePermissionSet ToOutputDevicePermissionSet(OutputDevicePermission permission)
	{
		return new OutputDevicePermissionSet(permission);
	}

	public static implicit operator OutputDevicePermissionSet(OutputDevicePermission permission)
	{
		return new OutputDevicePermissionSet(permission);
	}

	public bool Equals(OutputDevicePermission other)
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
		if (!(obj is OutputDevicePermission other))
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
