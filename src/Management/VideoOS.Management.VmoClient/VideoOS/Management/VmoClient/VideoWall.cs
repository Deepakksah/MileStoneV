using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

[EntityName("VideoWall")]
public class VideoWall : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	private static Lazy<Dictionary<VideoWallPermission, string>> _xlatPermissionToActionId = new Lazy<Dictionary<VideoWallPermission, string>>(() => new Dictionary<VideoWallPermission, string>
	{
		{
			VideoWallPermission.Operate,
			VideoWallPermissionSet.PermissionEnumToActionId(VideoWallPermissionSetValue.Operate)
		},
		{
			VideoWallPermission.Playback,
			VideoWallPermissionSet.PermissionEnumToActionId(VideoWallPermissionSetValue.Playback)
		}
	});

	public string Name
	{
		get
		{
			return GetPropertyValue<string>("Name");
		}
		set
		{
			SetPropertyValue("Name", value);
		}
	}

	public string Description
	{
		get
		{
			return GetPropertyValue<string>("Description");
		}
		set
		{
			SetPropertyValue("Description", value);
		}
	}

	public double Width
	{
		get
		{
			return GetPropertyValue<double>("Width");
		}
		set
		{
			SetPropertyValue("Width", value);
		}
	}

	public double Height
	{
		get
		{
			return GetPropertyValue<double>("Height");
		}
		set
		{
			SetPropertyValue("Height", value);
		}
	}

	public bool StatusText
	{
		get
		{
			return GetPropertyValue<bool>("StatusText");
		}
		set
		{
			SetPropertyValue("StatusText", value);
		}
	}

	public TitleBar TitleBar
	{
		get
		{
			return GetPropertyValue<TitleBar>("TitleBar");
		}
		set
		{
			SetPropertyValue("TitleBar", value);
		}
	}

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public MonitorCollection Monitors { get; private set; }

	public VideoWallPresetCollection VideoWallPresets { get; private set; }

	public VideoWall(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public VideoWall(ManagementServer managementServer, Guid customId)
		: this(managementServer.Repositories, customId)
	{
	}

	public VideoWall(IVmoRepositories repositories, Guid customId)
		: this(repositories)
	{
		SetPropertyValue("Id", customId.ToString());
	}

	public VideoWall(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		Width = 200.0;
		Height = 200.0;
	}

	public VideoWall(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		Monitors = new MonitorCollection(LoadMonitors, this, isInitialized);
		VideoWallPresets = new VideoWallPresetCollection(LoadVideoWallPresets, this, isInitialized);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	private Monitor[] LoadMonitors(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<VideoWall, Monitor>(base.Id, refreshCachedObjects);
	}

	private VideoWallPreset[] LoadVideoWallPresets(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<VideoWall, VideoWallPreset>(base.Id, refreshCachedObjects);
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (VideoWallPermissionSet)VideoWallPermissionSet.CreateFromActionId(actionId);
	}

	public VideoWallPermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<VideoWallPermissionInfo>();
	}

	public VideoWallPermissionInfo[] EnumPermissions(VideoWallPermissionSet permissions)
	{
		return EnumPermissions<VideoWallPermissionInfo>(permissions);
	}

	public VideoWallPermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<VideoWallPermissionInfo>(identity, VideoWallPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public VideoWallPermissionInfo[] EnumPermissions(string identity, VideoWallPermissionSet permissions)
	{
		return EnumPermissions<VideoWallPermissionInfo>(identity, permissions);
	}

	public VideoWallPermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, VideoWallPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public VideoWallPermissionSet EffectivePermissions(string identity, VideoWallPermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(VideoWallPermissionSet permissions, string identity)
	{
		Allow((PermissionSetBase)permissions, new string[1] { identity });
	}

	public void Allow(VideoWallPermissionSet permissions, string[] identities)
	{
		Allow((PermissionSetBase)permissions, identities);
	}

	public void Revoke(VideoWallPermissionSet permissions, string identity)
	{
		Revoke((PermissionSetBase)permissions, new string[1] { identity });
	}

	public void Revoke(VideoWallPermissionSet permissions, string[] identities)
	{
		Revoke((PermissionSetBase)permissions, identities);
	}

	public void DeleteTimeBasedSecurity(string identity, VideoWallPermission permission)
	{
		if (_xlatPermissionToActionId.Value.ContainsKey(permission))
		{
			base.Repositories.ServerCommunication.DeleteTimeBasedAccessEntry(identity, _xlatPermissionToActionId.Value[permission], base.InternalId);
		}
	}

	public bool SetTimeBasedSecurity(string identity, VideoWallPermission permission, string timeProfileId, bool isDefaultTimeProfile)
	{
		if (!_xlatPermissionToActionId.Value.ContainsKey(permission))
		{
			return false;
		}
		AccessControlTimeProfileEntry accessControlTimeProfileEntry = base.Repositories.ServerCommunication.CreateUpdateTimeBasedAccessEntry(identity, _xlatPermissionToActionId.Value[permission], base.InternalId, timeProfileId, isDefaultTimeProfile);
		if (accessControlTimeProfileEntry != null)
		{
			return !string.IsNullOrEmpty(accessControlTimeProfileEntry.ActionId);
		}
		return false;
	}

	public TimeProfilePermissionInfo QueryTimeBasedSecurity(string identity, VideoWallPermission permission)
	{
		if (!_xlatPermissionToActionId.Value.ContainsKey(permission))
		{
			return null;
		}
		AccessControlTimeProfileEntry accessControlTimeProfileEntry = base.Repositories.ServerCommunication.QueryTimeBasedAccessEntry(identity, _xlatPermissionToActionId.Value[permission], base.InternalId);
		if (accessControlTimeProfileEntry == null || string.IsNullOrEmpty(accessControlTimeProfileEntry.ActionId))
		{
			return null;
		}
		return new TimeProfilePermissionInfo(accessControlTimeProfileEntry);
	}
}
