using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("VideoWallPreset")]
public class VideoWallPreset : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
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

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public VideoWall VideoWall
	{
		get
		{
			string propertyValue = GetPropertyValue<string>("VideoWallId");
			return GetObjectById<VideoWall>(propertyValue);
		}
	}

	public string VideoWallId => GetPropertyValue<string>("VideoWallId");

	public MonitorPresetCollection MonitorPresets { get; private set; }

	public VideoWallPreset(VideoWall videoWall)
		: this(videoWall.Repositories, videoWall.Id)
	{
	}

	public VideoWallPreset(VideoWall videoWall, Guid customId)
		: this(videoWall.Repositories, videoWall.Id, customId)
	{
	}

	public VideoWallPreset(IVmoRepositories repositories, Guid videoWallId, Guid customId)
		: this(repositories, videoWallId)
	{
		SetPropertyValue("Id", customId.ToString());
	}

	public VideoWallPreset(IVmoRepositories repositories, Guid videoWallId)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		SetPropertyValue("VideoWallId", videoWallId.ToString());
	}

	public VideoWallPreset(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		MonitorPresets = new MonitorPresetCollection(LoadMonitorPresets, this, isInitialized: false);
	}

	public void Activate()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.ActivateVideoWallPreset(base.Id.ToString());
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

	private MonitorPreset[] LoadMonitorPresets(bool forced)
	{
		return base.Repositories.GetObjectsByParentId<VideoWallPreset, MonitorPreset>(base.Id, forced);
	}
}
