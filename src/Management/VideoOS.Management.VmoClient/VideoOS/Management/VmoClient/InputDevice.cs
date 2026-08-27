using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("InputDevice")]
public class InputDevice : Device
{
	public InputDevice(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (InputDevicePermissionSet)InputDevicePermissionSet.CreateFromActionId(actionId);
	}

	public InputDevicePermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<InputDevicePermissionInfo>();
	}

	public InputDevicePermissionInfo[] EnumPermissions(InputDevicePermissionSet permissions)
	{
		return EnumPermissions<InputDevicePermissionInfo>(permissions);
	}

	public InputDevicePermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<InputDevicePermissionInfo>(identity, InputDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public InputDevicePermissionInfo[] EnumPermissions(string identity, InputDevicePermissionSet permissions)
	{
		return EnumPermissions<InputDevicePermissionInfo>(identity, permissions);
	}

	public InputDevicePermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, InputDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public InputDevicePermissionSet EffectivePermissions(string identity, InputDevicePermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(InputDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Allow(permissions, new string[1] { text });
	}

	public void Allow(InputDevicePermissionSet permissions, Role[] roles)
	{
		if (roles == null)
		{
			throw new ArgumentNullException("roles");
		}
		List<string> list = new List<string>();
		foreach (Role role in roles)
		{
			if (role == null)
			{
				throw new ArgumentException("Array can't contain null elements", "roles");
			}
			string item = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
			list.Add(item);
		}
		Allow(permissions, list.ToArray());
	}

	public void Revoke(InputDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Revoke(permissions, new string[1] { text });
	}

	public void Revoke(InputDevicePermissionSet permissions, Role[] roles)
	{
		if (roles == null)
		{
			throw new ArgumentNullException("roles");
		}
		List<string> list = new List<string>();
		foreach (Role role in roles)
		{
			if (role == null)
			{
				throw new ArgumentException("Array can't contain null elements", "roles");
			}
			string item = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
			list.Add(item);
		}
		Revoke(permissions, list.ToArray());
	}
}
