using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class InputDevicePermissionSet : PermissionSetBase, IEquatable<InputDevicePermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Edit => base.PermissionStorage[1];

	public bool AdministrateSecurity => base.PermissionStorage[2];

	protected override int NumberOfElements => 3;

	public InputDevicePermissionSet()
	{
	}

	public InputDevicePermissionSet(InputDevicePermission permission)
	{
		SetBit(permission);
	}

	public InputDevicePermissionSet(InputDevicePermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public InputDevicePermissionSet Add(InputDevicePermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(InputDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(InputDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static InputDevicePermissionSet SupportedPermissions(ServerVersion version)
	{
		InputDevicePermissionSet inputDevicePermissionSet = new InputDevicePermissionSet();
		inputDevicePermissionSet.SetBit(InputDevicePermission.Read);
		inputDevicePermissionSet.SetBit(InputDevicePermission.Edit);
		inputDevicePermissionSet.SetBit(InputDevicePermission.AdministrateSecurity);
		return inputDevicePermissionSet;
	}

	public static InputDevicePermissionSet Add(InputDevicePermissionSet permissionSetLeft, InputDevicePermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static InputDevicePermissionSet operator +(InputDevicePermissionSet permissionSetLeft, InputDevicePermission permissionRight)
	{
		InputDevicePermissionSet inputDevicePermissionSet = new InputDevicePermissionSet(permissionSetLeft);
		inputDevicePermissionSet.SetBit(permissionRight);
		return inputDevicePermissionSet;
	}

	public static InputDevicePermissionSet Subtract(InputDevicePermissionSet permissionSetLeft, InputDevicePermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static InputDevicePermissionSet operator -(InputDevicePermissionSet permissionSetLeft, InputDevicePermission permissionRight)
	{
		InputDevicePermissionSet inputDevicePermissionSet = new InputDevicePermissionSet(permissionSetLeft);
		inputDevicePermissionSet.ResetBit(permissionRight);
		return inputDevicePermissionSet;
	}

	public static bool Equals(InputDevicePermissionSet permissionSetLeft, InputDevicePermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(InputDevicePermissionSet permissionSetLeft, InputDevicePermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(InputDevicePermissionSet permissionSetLeft, InputDevicePermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(InputDevicePermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is InputDevicePermissionSet other))
		{
			return false;
		}
		return Equals(other);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	protected override string PermissionEnumToString(int value)
	{
		return value switch
		{
			0 => "GENERIC_READ", 
			1 => "GENERIC_WRITE", 
			2 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static InputDevicePermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new InputDevicePermission(InputDevicePermissionSetValue.Read), 
			"GENERIC_WRITE" => new InputDevicePermission(InputDevicePermissionSetValue.Edit), 
			"ADMIN_SECURITY" => new InputDevicePermission(InputDevicePermissionSetValue.AdministrateSecurity), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		InputDevicePermissionSet result = new InputDevicePermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			InputDevicePermission inputDevicePermission = CreateFromActionId(actionIdArray[i]);
			result += inputDevicePermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(InputDevicePermissionSetValue value)
	{
		return value switch
		{
			InputDevicePermissionSetValue.Read => "GENERIC_READ", 
			InputDevicePermissionSetValue.Edit => "GENERIC_WRITE", 
			InputDevicePermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
