using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("SpeakerDevice")]
public class SpeakerDevice : RecordingDevice
{
	public SpeakerDevice(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (SpeakerDevicePermissionSet)SpeakerDevicePermissionSet.CreateFromActionId(actionId);
	}

	public SpeakerDevicePermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<SpeakerDevicePermissionInfo>();
	}

	public SpeakerDevicePermissionInfo[] EnumPermissions(SpeakerDevicePermissionSet permissions)
	{
		return EnumPermissions<SpeakerDevicePermissionInfo>(permissions);
	}

	public SpeakerDevicePermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<SpeakerDevicePermissionInfo>(identity, SpeakerDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public SpeakerDevicePermissionInfo[] EnumPermissions(string identity, SpeakerDevicePermissionSet permissions)
	{
		return EnumPermissions<SpeakerDevicePermissionInfo>(identity, permissions);
	}

	public SpeakerDevicePermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, SpeakerDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public SpeakerDevicePermissionSet EffectivePermissions(string identity, SpeakerDevicePermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(SpeakerDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Allow(permissions, new string[1] { text });
	}

	public void Allow(SpeakerDevicePermissionSet permissions, Role[] roles)
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

	public void Revoke(SpeakerDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Revoke(permissions, new string[1] { text });
	}

	public void Revoke(SpeakerDevicePermissionSet permissions, Role[] roles)
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
