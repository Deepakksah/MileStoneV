using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("CameraDevice")]
public class CameraDevice : RecordingDevice
{
	public PatrollingProfileCollection PatrollingProfiles { get; private set; }

	public double RecordingFramerate
	{
		get
		{
			return GetPropertyValue<double>("RecordingFramerate");
		}
		set
		{
			SetPropertyValue("RecordingFramerate", value);
		}
	}

	public bool RecordKeyframesOnly
	{
		get
		{
			return GetPropertyValue<bool>("RecordKeyframesOnly");
		}
		set
		{
			SetPropertyValue("RecordKeyframesOnly", value);
		}
	}

	public bool RecordOnRelatedDevices
	{
		get
		{
			return GetPropertyValue<bool>("RecordOnRelatedDevices");
		}
		set
		{
			SetPropertyValue("RecordOnRelatedDevices", value);
		}
	}

	public PrivacyProtection PrivacyProtectionSettings => GetObjectById<PrivacyProtection>(InternalId);

	public MotionDetection MotionDetectionSettings => GetObjectById<MotionDetection>(InternalId);

	public CameraDevice(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		PatrollingProfiles = new PatrollingProfileCollection(LoadPatrollingProfiles, this, isInitialized);
	}

	private PatrollingProfile[] LoadPatrollingProfiles(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<CameraDevice, PatrollingProfile>(base.Id, refreshCachedObjects);
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (CameraDevicePermissionSet)CameraDevicePermissionSet.CreateFromActionId(actionId);
	}

	public CameraDevicePermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<CameraDevicePermissionInfo>();
	}

	public CameraDevicePermissionInfo[] EnumPermissions(CameraDevicePermissionSet permissions)
	{
		return EnumPermissions<CameraDevicePermissionInfo>(permissions);
	}

	public CameraDevicePermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<CameraDevicePermissionInfo>(identity, CameraDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public CameraDevicePermissionInfo[] EnumPermissions(string identity, CameraDevicePermissionSet permissions)
	{
		return EnumPermissions<CameraDevicePermissionInfo>(identity, permissions);
	}

	public CameraDevicePermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, CameraDevicePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public CameraDevicePermissionSet EffectivePermissions(string identity, CameraDevicePermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(CameraDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Allow(permissions, new string[1] { text });
	}

	public void Allow(CameraDevicePermissionSet permissions, Role[] roles)
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

	public void Revoke(CameraDevicePermissionSet permissions, Role role)
	{
		if (role == null)
		{
			throw new ArgumentNullException("role");
		}
		string text = string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", role.Name);
		Revoke(permissions, new string[1] { text });
	}

	public void Revoke(CameraDevicePermissionSet permissions, Role[] roles)
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
