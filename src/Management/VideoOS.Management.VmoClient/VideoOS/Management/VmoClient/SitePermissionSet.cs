using System;
using System.Globalization;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

public sealed class SitePermissionSet : PermissionSetBase, IEquatable<SitePermissionSet>
{
	public bool Read => base.PermissionStorage[0];

	protected override int NumberOfElements => 1;

	public SitePermissionSet()
	{
	}

	public SitePermissionSet(SitePermission permission)
	{
		SetBit(permission);
	}

	public SitePermissionSet(SitePermissionSet permissionSet)
		: base(permissionSet)
	{
	}

	public void Add(SitePermission permission)
	{
		SetBit(permission);
	}

	public void Subtract(SitePermission permission)
	{
		ResetBit(permission);
	}

	public bool Equals(SitePermissionSet other)
	{
		return Equals((PermissionSetBase)other);
	}

	public sealed override bool Equals(object obj)
	{
		if (!(obj is SitePermissionSet other))
		{
			return false;
		}
		return Equals(other);
	}

	public void SetBit(SitePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = true;
	}

	public void ResetBit(SitePermission permission)
	{
		base.PermissionStorage[(int)permission.Value] = false;
	}

	public static SitePermissionSet SupportedPermissions(ServerVersion version)
	{
		SitePermissionSet sitePermissionSet = new SitePermissionSet();
		sitePermissionSet.PermissionStorage.SetAll(value: true);
		return sitePermissionSet;
	}

	public static SitePermissionSet operator +(SitePermissionSet permissionSetLeft, SitePermission permissionRight)
	{
		SitePermissionSet sitePermissionSet = new SitePermissionSet(permissionSetLeft);
		sitePermissionSet.SetBit(permissionRight);
		return sitePermissionSet;
	}

	public static SitePermissionSet operator -(SitePermissionSet permissionSetLeft, SitePermission permissionRight)
	{
		SitePermissionSet sitePermissionSet = new SitePermissionSet(permissionSetLeft);
		sitePermissionSet.ResetBit(permissionRight);
		return sitePermissionSet;
	}

	public static bool operator ==(SitePermissionSet permissionSetLeft, SitePermissionSet permissionSetRight)
	{
		return object.Equals(permissionSetLeft, permissionSetRight);
	}

	public static bool operator !=(SitePermissionSet permissionSetLeft, SitePermissionSet permissionSetRight)
	{
		return !(permissionSetLeft == permissionSetRight);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	protected override string PermissionEnumToString(int value)
	{
		if (value == 0)
		{
			return "GENERIC_READ";
		}
		throw new InternalVmoException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidArgumentValue, "value", value));
	}

	public static SitePermission CreateFromActionId(string actionId)
	{
		if (actionId == "GENERIC_READ")
		{
			return new SitePermission(SitePermissionSetValue.Read);
		}
		throw new InternalVmoException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidArgumentValue, "actionId", actionId));
	}

	public override PermissionSetBase CreateFromActionId(string[] actionIdArray)
	{
		SitePermissionSet result = new SitePermissionSet();
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			SitePermission sitePermission = CreateFromActionId(actionIdArray[i]);
			result += sitePermission;
		}
		return result;
	}
}
