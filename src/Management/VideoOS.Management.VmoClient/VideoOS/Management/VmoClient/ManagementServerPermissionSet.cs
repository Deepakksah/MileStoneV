using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class ManagementServerPermissionSet : PermissionSetBase, IEquatable<ManagementServerPermissionSet>
{
	public bool Connect => base.PermissionStorage[0];

	public bool Read => base.PermissionStorage[1];

	public bool Write => base.PermissionStorage[2];

	public bool AdministrateSecurity => base.PermissionStorage[3];

	public bool StatusApi => base.PermissionStorage[4];

	public bool AdministrateFederatedSiteHierarchy => base.PermissionStorage[5];

	public bool BackupConfiguration => base.PermissionStorage[6];

	public bool AuthorizeUser => base.PermissionStorage[7];

	protected override int NumberOfElements => 8;

	public ManagementServerPermissionSet()
	{
	}

	public ManagementServerPermissionSet(ManagementServerPermission permission)
	{
		SetBit(permission);
	}

	public ManagementServerPermissionSet(ManagementServerPermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public ManagementServerPermissionSet Add(ManagementServerPermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(ManagementServerPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(ManagementServerPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static ManagementServerPermissionSet SupportedPermissions(ServerVersion version)
	{
		ManagementServerPermissionSet managementServerPermissionSet = new ManagementServerPermissionSet();
		managementServerPermissionSet.PermissionStorage.SetAll(value: true);
		return managementServerPermissionSet;
	}

	public static ManagementServerPermissionSet Add(ManagementServerPermissionSet permissionSetLeft, ManagementServerPermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static ManagementServerPermissionSet operator +(ManagementServerPermissionSet permissionSetLeft, ManagementServerPermission permissionRight)
	{
		ManagementServerPermissionSet managementServerPermissionSet = new ManagementServerPermissionSet(permissionSetLeft);
		managementServerPermissionSet.SetBit(permissionRight);
		return managementServerPermissionSet;
	}

	public static ManagementServerPermissionSet Subtract(ManagementServerPermissionSet permissionSetLeft, ManagementServerPermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static ManagementServerPermissionSet operator -(ManagementServerPermissionSet permissionSetLeft, ManagementServerPermission permissionRight)
	{
		ManagementServerPermissionSet managementServerPermissionSet = new ManagementServerPermissionSet(permissionSetLeft);
		managementServerPermissionSet.ResetBit(permissionRight);
		return managementServerPermissionSet;
	}

	public static bool operator ==(ManagementServerPermissionSet permissionSetLeft, ManagementServerPermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(ManagementServerPermissionSet permissionSetLeft, ManagementServerPermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(ManagementServerPermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is ManagementServerPermissionSet other))
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
			0 => "CONNECT", 
			1 => "GENERIC_READ", 
			2 => "GENERIC_WRITE", 
			3 => "ADMIN_SECURITY", 
			4 => "STATUS_API", 
			5 => "ADMIN_FEDERATED_HIERARCHY", 
			6 => "BACKUP_CONFIGURATION", 
			7 => "AUTHORIZE_USER", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static ManagementServerPermission CreateFromActionId(string actionId)
	{
		return actionId switch
		{
			"CONNECT" => new ManagementServerPermission(ManagementServerPermissionSetValue.Connect), 
			"GENERIC_READ" => new ManagementServerPermission(ManagementServerPermissionSetValue.Read), 
			"GENERIC_WRITE" => new ManagementServerPermission(ManagementServerPermissionSetValue.Edit), 
			"ADMIN_SECURITY" => new ManagementServerPermission(ManagementServerPermissionSetValue.AdministrateSecurity), 
			"STATUS_API" => new ManagementServerPermission(ManagementServerPermissionSetValue.StatusApi), 
			"ADMIN_FEDERATED_HIERARCHY" => new ManagementServerPermission(ManagementServerPermissionSetValue.AdministrateFederatedSiteHierarchy), 
			"BACKUP_CONFIGURATION" => new ManagementServerPermission(ManagementServerPermissionSetValue.BackupConfiguration), 
			"AUTHORIZE_USER" => new ManagementServerPermission(ManagementServerPermissionSetValue.AuthorizeUser), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		ManagementServerPermissionSet result = new ManagementServerPermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			ManagementServerPermission managementServerPermission = CreateFromActionId(actionIdArray[i]);
			result += managementServerPermission;
		}
		return result;
	}
}
