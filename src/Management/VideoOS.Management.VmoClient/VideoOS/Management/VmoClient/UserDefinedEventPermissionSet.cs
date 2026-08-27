using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class UserDefinedEventPermissionSet : PermissionSetBase, IEquatable<UserDefinedEventPermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Write => base.PermissionStorage[1];

	public bool Delete => base.PermissionStorage[2];

	public bool TriggerEvent => base.PermissionStorage[3];

	public bool AdministrateSecurity => base.PermissionStorage[4];

	protected override int NumberOfElements => 5;

	public UserDefinedEventPermissionSet()
	{
	}

	public UserDefinedEventPermissionSet(UserDefinedEventPermission permission)
	{
		SetBit(permission);
	}

	public UserDefinedEventPermissionSet(UserDefinedEventPermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public UserDefinedEventPermissionSet Add(UserDefinedEventPermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(UserDefinedEventPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(UserDefinedEventPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static UserDefinedEventPermissionSet SupportedPermissions(ServerVersion version)
	{
		UserDefinedEventPermissionSet userDefinedEventPermissionSet = new UserDefinedEventPermissionSet();
		userDefinedEventPermissionSet.PermissionStorage.SetAll(value: true);
		return userDefinedEventPermissionSet;
	}

	public static UserDefinedEventPermissionSet Add(UserDefinedEventPermissionSet permissionSetLeft, UserDefinedEventPermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static UserDefinedEventPermissionSet operator +(UserDefinedEventPermissionSet permissionSetLeft, UserDefinedEventPermission permissionRight)
	{
		UserDefinedEventPermissionSet userDefinedEventPermissionSet = new UserDefinedEventPermissionSet(permissionSetLeft);
		userDefinedEventPermissionSet.SetBit(permissionRight);
		return userDefinedEventPermissionSet;
	}

	public static UserDefinedEventPermissionSet Subtract(UserDefinedEventPermissionSet permissionSetLeft, UserDefinedEventPermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static UserDefinedEventPermissionSet operator -(UserDefinedEventPermissionSet permissionSetLeft, UserDefinedEventPermission permissionRight)
	{
		UserDefinedEventPermissionSet userDefinedEventPermissionSet = new UserDefinedEventPermissionSet(permissionSetLeft);
		userDefinedEventPermissionSet.ResetBit(permissionRight);
		return userDefinedEventPermissionSet;
	}

	public static bool Equals(UserDefinedEventPermissionSet permissionSetLeft, UserDefinedEventPermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(UserDefinedEventPermissionSet permissionSetLeft, UserDefinedEventPermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(UserDefinedEventPermissionSet permissionSetLeft, UserDefinedEventPermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(UserDefinedEventPermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is UserDefinedEventPermissionSet other))
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
			2 => "DELETE", 
			3 => "TRIGGER", 
			4 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static UserDefinedEventPermission CreateFromActionId(string actionId)
	{
		return actionId switch
		{
			"GENERIC_READ" => new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Read), 
			"GENERIC_WRITE" => new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Edit), 
			"DELETE" => new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Delete), 
			"TRIGGER" => new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.Trigger), 
			"ADMIN_SECURITY" => new UserDefinedEventPermission(UserDefinedEventPermissionSetValue.AdministrateSecurity), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		UserDefinedEventPermissionSet result = new UserDefinedEventPermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			UserDefinedEventPermission userDefinedEventPermission = CreateFromActionId(actionIdArray[i]);
			result += userDefinedEventPermission;
		}
		return result;
	}

	public static string PermissionEnumToActionId(UserDefinedEventPermissionSetValue value)
	{
		return value switch
		{
			UserDefinedEventPermissionSetValue.Read => "GENERIC_READ", 
			UserDefinedEventPermissionSetValue.Edit => "GENERIC_WRITE", 
			UserDefinedEventPermissionSetValue.Delete => "DELETE", 
			UserDefinedEventPermissionSetValue.Trigger => "TRIGGER", 
			UserDefinedEventPermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.InvariantCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}
}
