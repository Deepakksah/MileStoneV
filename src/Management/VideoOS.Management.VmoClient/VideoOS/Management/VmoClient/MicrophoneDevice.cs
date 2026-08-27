using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MicrophoneDevice")]
public class MicrophoneDevice : RecordingDevice
{
	public MicrophoneDevice(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (MicrophoneDevicePermissionSet)MicrophoneDevicePermissionSet.CreateFromActionId(actionId);
	}

	public MicrophoneDevicePermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<MicrophoneDevicePermissionInfo>();
	}

	public MicrophoneDevicePermissionInfo[] EnumPermissions(MicrophoneDevicePermissionSet permissions)
	{
		return EnumPermissions<MicrophoneDevicePermissionInfo>(permissions);
	}

	public MicrophoneDevicePermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<MicrophoneDevicePermissionInfo>(identity, MicrophoneDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public MicrophoneDevicePermissionInfo[] EnumPermissions(string identity, MicrophoneDevicePermissionSet permissions)
	{
		return EnumPermissions<MicrophoneDevicePermissionInfo>(identity, permissions);
	}

	public MicrophoneDevicePermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, MicrophoneDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public MicrophoneDevicePermissionSet EffectivePermissions(string identity, MicrophoneDevicePermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(MicrophoneDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Allow(permissions, new string[1] { text });
	}

	public void Allow(MicrophoneDevicePermissionSet permissions, Role[] roles)
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

	public void Revoke(MicrophoneDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Revoke(permissions, new string[1] { text });
	}

	public void Revoke(MicrophoneDevicePermissionSet permissions, Role[] roles)
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
