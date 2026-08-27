using System;

namespace VideoOS.Management.VmoClient;

public sealed class InputDevicePermission : IEquatable<InputDevicePermission>
{
	public InputDevicePermissionSetValue Value { get; private set; }

	public static InputDevicePermission Read => new InputDevicePermission(InputDevicePermissionSetValue.Read);

	public static InputDevicePermission Edit => new InputDevicePermission(InputDevicePermissionSetValue.Edit);

	public static InputDevicePermission AdministrateSecurity => new InputDevicePermission(InputDevicePermissionSetValue.AdministrateSecurity);

	public InputDevicePermission(InputDevicePermissionSetValue permissionValue)
	{
		Value = permissionValue;
	}

	public static InputDevicePermissionSet Add(InputDevicePermission permissionLeft, InputDevicePermission permissionRight)
	{
		return permissionLeft + permissionRight;
	}

	public static InputDevicePermissionSet operator +(InputDevicePermission permissionLeft, InputDevicePermission permissionRight)
	{
		InputDevicePermissionSet inputDevicePermissionSet = new InputDevicePermissionSet(permissionLeft);
		inputDevicePermissionSet.SetBit(permissionRight);
		return inputDevicePermissionSet;
	}

	public static InputDevicePermissionSet BitwiseOr(InputDevicePermission permissionLeft, InputDevicePermission permissionRight)
	{
		return permissionLeft | permissionRight;
	}

	public static InputDevicePermissionSet operator |(InputDevicePermission permissionLeft, InputDevicePermission permissionRight)
	{
		InputDevicePermissionSet inputDevicePermissionSet = new InputDevicePermissionSet(permissionLeft);
		inputDevicePermissionSet.SetBit(permissionRight);
		return inputDevicePermissionSet;
	}

	public static bool Equals(InputDevicePermission permissionLeft, InputDevicePermission permissionRight)
	{
		return permissionLeft == permissionRight;
	}

	public static bool operator ==(InputDevicePermission permissionLeft, InputDevicePermission permissionRight)
	{
		return object.Equals(permissionLeft, permissionRight);
	}

	public static bool operator !=(InputDevicePermission permissionLeft, InputDevicePermission permissionRight)
	{
		return !(permissionLeft == permissionRight);
	}

	public static InputDevicePermissionSet ToInputDevicePermissionSet(InputDevicePermission permission)
	{
		return new InputDevicePermissionSet(permission);
	}

	public static implicit operator InputDevicePermissionSet(InputDevicePermission permission)
	{
		return new InputDevicePermissionSet(permission);
	}

	public bool Equals(InputDevicePermission other)
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
		if (!(obj is InputDevicePermission other))
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
