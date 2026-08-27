using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MetadataDevice")]
public class MetadataDevice : RecordingDevice
{
	public MetadataDevice(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (MetadataDevicePermissionSet)MetadataDevicePermissionSet.CreateFromActionId(actionId);
	}

	public MetadataDevicePermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<MetadataDevicePermissionInfo>();
	}

	public MetadataDevicePermissionInfo[] EnumPermissions(MetadataDevicePermissionSet permissions)
	{
		return EnumPermissions<MetadataDevicePermissionInfo>(permissions);
	}

	public MetadataDevicePermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<MetadataDevicePermissionInfo>(identity, MetadataDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public MetadataDevicePermissionInfo[] EnumPermissions(string identity, MetadataDevicePermissionSet permissions)
	{
		return EnumPermissions<MetadataDevicePermissionInfo>(identity, permissions);
	}

	public MetadataDevicePermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, MetadataDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public MetadataDevicePermissionSet EffectivePermissions(string identity, MetadataDevicePermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(MetadataDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Allow(permissions, new string[1] { text });
	}

	public void Allow(MetadataDevicePermissionSet permissions, Role[] roles)
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

	public void Revoke(MetadataDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Revoke(permissions, new string[1] { text });
	}

	public void Revoke(MetadataDevicePermissionSet permissions, Role[] roles)
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
