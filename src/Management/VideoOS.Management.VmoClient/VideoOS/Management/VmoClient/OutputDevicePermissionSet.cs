using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class OutputDevicePermissionSet : PermissionSetBase, IEquatable<OutputDevicePermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Edit => base.PermissionStorage[1];

	public bool Activate => base.PermissionStorage[2];

	public bool AdministrateSecurity => base.PermissionStorage[3];

	protected override int NumberOfElements => 4;

	public OutputDevicePermissionSet()
	{
	}

	public OutputDevicePermissionSet(OutputDevicePermission permission)
	{
		SetBit(permission);
	}

	public OutputDevicePermissionSet(OutputDevicePermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public OutputDevicePermissionSet Add(OutputDevicePermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(OutputDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(OutputDevicePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static OutputDevicePermissionSet SupportedPermissions(ServerVersion version)
	{
		OutputDevicePermissionSet outputDevicePermissionSet = new OutputDevicePermissionSet();
		outputDevicePermissionSet.SetBit(OutputDevicePermission.Read);
		outputDevicePermissionSet.SetBit(OutputDevicePermission.Edit);
		outputDevicePermissionSet.SetBit(OutputDevicePermission.Activate);
		outputDevicePermissionSet.SetBit(OutputDevicePermission.AdministrateSecurity);
		return outputDevicePermissionSet;
	}

	public static OutputDevicePermissionSet Add(OutputDevicePermissionSet permissionSetLeft, OutputDevicePermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static OutputDevicePermissionSet operator +(OutputDevicePermissionSet permissionSetLeft, OutputDevicePermission permissionRight)
	{
		OutputDevicePermissionSet outputDevicePermissionSet = new OutputDevicePermissionSet(permissionSetLeft);
		outputDevicePermissionSet.SetBit(permissionRight);
		return outputDevicePermissionSet;
	}

	public static OutputDevicePermissionSet Subtract(OutputDevicePermissionSet permissionSetLeft, OutputDevicePermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static OutputDevicePermissionSet operator -(OutputDevicePermissionSet permissionSetLeft, OutputDevicePermission permissionRight)
	{
		OutputDevicePermissionSet outputDevicePermissionSet = new OutputDevicePermissionSet(permissionSetLeft);
		outputDevicePermissionSet.ResetBit(permissionRight);
		return outputDevicePermissionSet;
	}

	public static bool Equals(OutputDevicePermissionSet permissionSetLeft, OutputDevicePermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(OutputDevicePermissionSet permissionSetLeft, OutputDevicePermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(OutputDevicePermissionSet permissionSetLeft, OutputDevicePermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(OutputDevicePermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is OutputDevicePermissionSet other))
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
			2 => "ACTIVATE", 
			3 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static OutputDevicePermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new OutputDevicePermission(OutputDevicePermissionSetValue.Read), 
			"GENERIC_WRITE" => new OutputDevicePermission(OutputDevicePermissionSetValue.Edit), 
			"ACTIVATE" => new OutputDevicePermission(OutputDevicePermissionSetValue.Activate), 
			"ADMIN_SECURITY" => new OutputDevicePermission(OutputDevicePermissionSetValue.AdministrateSecurity), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		OutputDevicePermissionSet result = new OutputDevicePermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			OutputDevicePermission outputDevicePermission = CreateFromActionId(actionIdArray[i]);
			result += outputDevicePermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(OutputDevicePermissionSetValue value)
	{
		return value switch
		{
			OutputDevicePermissionSetValue.Read => "GENERIC_READ", 
			OutputDevicePermissionSetValue.Edit => "GENERIC_WRITE", 
			OutputDevicePermissionSetValue.Activate => "ACTIVATE", 
			OutputDevicePermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
