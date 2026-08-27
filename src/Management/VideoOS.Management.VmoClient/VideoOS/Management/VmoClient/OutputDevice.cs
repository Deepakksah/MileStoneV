using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("OutputDevice")]
public class OutputDevice : Device
{
	public OutputDevice(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (OutputDevicePermissionSet)OutputDevicePermissionSet.CreateFromActionId(actionId);
	}

	public OutputDevicePermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<OutputDevicePermissionInfo>();
	}

	public OutputDevicePermissionInfo[] EnumPermissions(OutputDevicePermissionSet permissions)
	{
		return EnumPermissions<OutputDevicePermissionInfo>(permissions);
	}

	public OutputDevicePermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<OutputDevicePermissionInfo>(identity, OutputDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public OutputDevicePermissionInfo[] EnumPermissions(string identity, OutputDevicePermissionSet permissions)
	{
		return EnumPermissions<OutputDevicePermissionInfo>(identity, permissions);
	}

	public OutputDevicePermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, OutputDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public OutputDevicePermissionSet EffectivePermissions(string identity, OutputDevicePermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(OutputDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Allow(permissions, new string[1] { text });
	}

	public void Allow(OutputDevicePermissionSet permissions, Role[] roles)
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

	public void Revoke(OutputDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Revoke(permissions, new string[1] { text });
	}

	public void Revoke(OutputDevicePermissionSet permissions, Role[] roles)
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
