using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public abstract class PermissionSetBase
{
	public BitArray PermissionStorage { get; private set; }

	protected abstract int NumberOfElements { get; }

	protected abstract string PermissionEnumToString(int value);

	public abstract PermissionSetBase CreateFromActionId(string[] actionIdArray);

	public PermissionSetBase()
	{
		PermissionStorage = new BitArray(NumberOfElements);
	}

	public PermissionSetBase(PermissionSetBase permissionSet)
	{
		if (permissionSet == null)
		{
			throw new ArgumentNullException("permissionSet");
		}
		PermissionStorage = (BitArray)permissionSet.PermissionStorage.Clone();
	}

	public void SetBitAt(int index)
	{
		PermissionStorage[index] = true;
	}

	public AccessControlEntry[] CreateAccessControlEntries(string[] sidArray, bool accessDeny)
	{
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		for (int i = 0; i < NumberOfElements; i++)
		{
			if (PermissionStorage[i])
			{
				foreach (string sid in sidArray)
				{
					AccessControlEntry item = new AccessControlEntry
					{
						Sid = sid,
						ActionId = PermissionEnumToString(i),
						AccessDeny = accessDeny
					};
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	public bool HasAnyPermissionSet(PermissionSetBase permissions)
	{
		if (permissions == null)
		{
			throw new ArgumentNullException("permissions");
		}
		if (GetType() != permissions.GetType())
		{
			throw new ArgumentException("The instance of the PermissionsSetBase is not of the same type as this instance.", "permissions");
		}
		for (int i = 0; i < NumberOfElements; i++)
		{
			if (permissions.PermissionStorage[i] && PermissionStorage[i])
			{
				return true;
			}
		}
		return false;
	}

	public bool HasAllPermissionsSet(PermissionSetBase permissions)
	{
		if (permissions == null)
		{
			throw new ArgumentNullException("permissions");
		}
		if (GetType() != permissions.GetType())
		{
			throw new ArgumentException("The instance of the PermissionsSetBase is not of the same type as this instance.", "permissions");
		}
		for (int i = 0; i < NumberOfElements; i++)
		{
			if (permissions.PermissionStorage[i] && !PermissionStorage[i])
			{
				return false;
			}
		}
		return true;
	}

	public bool Equals(PermissionSetBase other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		for (int i = 0; i < NumberOfElements; i++)
		{
			if (PermissionStorage[i] != other.PermissionStorage[i])
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return NumberOfElements.GetHashCode();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		for (int i = 0; i < PermissionStorage.Count; i++)
		{
			if (PermissionStorage[i])
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(PermissionEnumToString(i));
			}
		}
		return stringBuilder.ToString();
	}

	public string[] ToArray()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < PermissionStorage.Count; i++)
		{
			if (PermissionStorage[i])
			{
				list.Add(PermissionEnumToString(i));
			}
		}
		return list.ToArray();
	}

	public T AdjustForSupportedPermissionsAllow<T>(T supportedPermissions) where T : PermissionSetBase
	{
		for (int i = 0; i < supportedPermissions.PermissionStorage.Count; i++)
		{
			if (!supportedPermissions.PermissionStorage[i])
			{
				PermissionStorage[i] = true;
			}
		}
		return (T)this;
	}

	public T AdjustForSupportedPermissionsDeny<T>(T supportedPermissions) where T : PermissionSetBase
	{
		for (int i = 0; i < supportedPermissions.PermissionStorage.Count; i++)
		{
			if (!supportedPermissions.PermissionStorage[i])
			{
				PermissionStorage[i] = false;
			}
		}
		return (T)this;
	}
}
