using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[DebuggerDisplay("Read:{Read} Edit:{Edit} Delete:{Delete} Operate:{Operate} Administrate:{AdministrateSecurity}")]
public sealed class PublicViewGroupPermissionSet : PermissionSetBase, IEquatable<PublicViewGroupPermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	public bool Edit => base.PermissionStorage[1];

	public bool Delete => base.PermissionStorage[2];

	public bool Operate => base.PermissionStorage[3];

	public bool AdministrateSecurity => base.PermissionStorage[4];

	protected override int NumberOfElements => 5;

	public PublicViewGroupPermissionSet()
	{
	}

	public PublicViewGroupPermissionSet(PublicViewGroupPermission permission)
	{
		SetBit(permission);
	}

	public PublicViewGroupPermissionSet(PublicViewGroupPermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public PublicViewGroupPermissionSet Add(PublicViewGroupPermission permission)
	{
		SetBit(permission);
		return this;
	}

	public void SetBit(PublicViewGroupPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(PublicViewGroupPermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static PublicViewGroupPermissionSet SupportedPermissions(ServerVersion version)
	{
		PublicViewGroupPermissionSet publicViewGroupPermissionSet = new PublicViewGroupPermissionSet();
		publicViewGroupPermissionSet.PermissionStorage.SetAll(value: true);
		return publicViewGroupPermissionSet;
	}

	public static PublicViewGroupPermissionSet Add(PublicViewGroupPermissionSet permissionSetLeft, PublicViewGroupPermission permissionRight)
	{
		return permissionSetLeft + permissionRight;
	}

	public static PublicViewGroupPermissionSet operator +(PublicViewGroupPermissionSet permissionSetLeft, PublicViewGroupPermission permissionRight)
	{
		PublicViewGroupPermissionSet publicViewGroupPermissionSet = new PublicViewGroupPermissionSet(permissionSetLeft);
		publicViewGroupPermissionSet.SetBit(permissionRight);
		return publicViewGroupPermissionSet;
	}

	public static PublicViewGroupPermissionSet Subtract(PublicViewGroupPermissionSet permissionSetLeft, PublicViewGroupPermission permissionRight)
	{
		return permissionSetLeft - permissionRight;
	}

	public static PublicViewGroupPermissionSet operator -(PublicViewGroupPermissionSet permissionSetLeft, PublicViewGroupPermission permissionRight)
	{
		PublicViewGroupPermissionSet publicViewGroupPermissionSet = new PublicViewGroupPermissionSet(permissionSetLeft);
		publicViewGroupPermissionSet.ResetBit(permissionRight);
		return publicViewGroupPermissionSet;
	}

	public static bool Equals(PublicViewGroupPermissionSet permissionSetLeft, PublicViewGroupPermissionSet permissionSetRight)
	{
		return permissionSetLeft == permissionSetRight;
	}

	public static bool operator ==(PublicViewGroupPermissionSet permissionSetLeft, PublicViewGroupPermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(PublicViewGroupPermissionSet permissionSetLeft, PublicViewGroupPermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public bool Equals(PublicViewGroupPermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is PublicViewGroupPermissionSet other))
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
			3 => "OPERATE", 
			4 => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidArgumentValue, "value", value)), 
		};
	}

	public static PublicViewGroupPermission CreateFromActionId(string actionId)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException("actionId");
		}
		return actionId switch
		{
			"GENERIC_READ" => new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Read), 
			"GENERIC_WRITE" => new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Edit), 
			"DELETE" => new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Delete), 
			"OPERATE" => new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.Operate), 
			"ADMIN_SECURITY" => new PublicViewGroupPermission(PublicViewGroupPermissionSetValue.AdministrateSecurity), 
			_ => throw new InternalVmoException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidArgumentValue, "actionId", actionId)), 
		};
	}

	public static string PermissionEnumToActionId(PublicViewGroupPermissionSetValue publicViewGroupPermission)
	{
		return publicViewGroupPermission switch
		{
			PublicViewGroupPermissionSetValue.Read => "GENERIC_READ", 
			PublicViewGroupPermissionSetValue.Edit => "GENERIC_WRITE", 
			PublicViewGroupPermissionSetValue.Delete => "DELETE", 
			PublicViewGroupPermissionSetValue.Operate => "OPERATE", 
			PublicViewGroupPermissionSetValue.AdministrateSecurity => "ADMIN_SECURITY", 
			_ => throw new InternalVmoException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidArgumentValue, "publicViewGroupPermission", publicViewGroupPermission)), 
		};
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		PublicViewGroupPermissionSet result = new PublicViewGroupPermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			PublicViewGroupPermission publicViewGroupPermission = CreateFromActionId(actionIdArray[i]);
			result += publicViewGroupPermission;
		}
		return result;
	}
}
