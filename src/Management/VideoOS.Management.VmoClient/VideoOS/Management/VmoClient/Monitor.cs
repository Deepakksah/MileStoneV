using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("Monitor")]
public class Monitor : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	private Location _monitorLocation;

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

	public AspectRatio AspectRatio
	{
		get
		{
			return GetPropertyValue<AspectRatio>("AspectRatio");
		}
		set
		{
			SetPropertyValue("AspectRatio", value);
		}
	}

	public float MonitorSize
	{
		get
		{
			return GetPropertyValue<float>("MonitorSize");
		}
		set
		{
			SetPropertyValue("MonitorSize", value);
		}
	}

	public PresetLayoutMethod EmptyViewItems
	{
		get
		{
			return GetPropertyValue<PresetLayoutMethod>("EmptyViewItems");
		}
		set
		{
			SetPropertyValue("EmptyViewItems", value);
		}
	}

	public ElementInsertionMethod InsertionMethod
	{
		get
		{
			return GetPropertyValue<ElementInsertionMethod>("InsertionMethod");
		}
		set
		{
			SetPropertyValue("InsertionMethod", value);
		}
	}

	public Location Location
	{
		get
		{
			if (_monitorLocation == null)
			{
				_monitorLocation = new Location(this);
			}
			return _monitorLocation;
		}
	}

	public PresetLayoutMethod NoLayout
	{
		get
		{
			return GetPropertyValue<PresetLayoutMethod>("NoLayout");
		}
		set
		{
			SetPropertyValue("NoLayout", value);
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

	public string ViewId => GetPropertyValue<string>("ViewId");

	public string State => GetPropertyValue<string>("MonitorState");

	public View View => GetObjectById<View>(ViewId);

	public MonitorPresetCollection MonitorPresets { get; private set; }

	public Monitor(VideoWall videoWall)
		: this(videoWall.Repositories, videoWall.Id)
	{
	}

	public Monitor(VideoWall videoWall, Guid customId)
		: this(videoWall.Repositories, videoWall.Id, customId)
	{
	}

	public Monitor(IVmoRepositories repositories, Guid videoWallId, Guid customId)
		: this(repositories, videoWallId)
	{
		SetPropertyValue("Id", customId.ToString());
	}

	public Monitor(IVmoRepositories repositories, Guid videoWallId)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		SetPropertyValue("VideoWallId", videoWallId.ToString());
		AspectRatio = AspectRatio.Aspect4x3;
		MonitorSize = 20f;
		EmptyViewItems = PresetLayoutMethod.Preserve;
		InsertionMethod = ElementInsertionMethod.Independent;
		NoLayout = PresetLayoutMethod.Preserve;
	}

	public Monitor(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		MonitorPresets = new MonitorPresetCollection(LoadMonitorPresets, this, isInitialized: false);
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

	public void ActivateCameras(Guid[] cameraIds, Guid[] cameraGroupsIds, int fromPosition, Guid layoutId)
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ApplyCamerasAndLayoutToMonitorView(base.Repositories, this, cameraIds, cameraGroupsIds, fromPosition, layoutId);
	}

	public void ActivateCameras(Guid[] cameraIds, Guid[] cameraGroupsIds, int fromPosition)
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ApplyCamerasToMonitorView(base.Repositories, this, cameraIds, cameraGroupsIds, fromPosition);
	}

	public void RemoveCameras(Guid[] cameraIds, Guid[] cameraGroupsIds)
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.RemoveCamerasFromMonitorView(base.Repositories, this, cameraIds, cameraGroupsIds);
	}

	public void ApplyViewItemsXml(string[] viewItemsXml, int fromPosition)
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ApplyViewItemsXmlToMonitorView(base.Repositories, this, viewItemsXml, fromPosition);
	}

	public void ApplyMonitorState(string xmlState)
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ApplyMonitorStateToMonitor(base.Repositories, this, xmlState);
		SetPropertyValue("MonitorState", xmlState);
	}

	public void ApplyLayout(Guid layoutId)
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ApplyLayout(base.Repositories, layoutId, this);
	}

	private MonitorPreset[] LoadMonitorPresets(bool forced)
	{
		return base.Repositories.GetObjectsByParentId<Monitor, MonitorPreset>(base.Id, forced);
	}

	public void DisplayText(string message, int viewPosition, Guid layoutId)
	{
		ThrowIfNotCreatedOrDeleted();
		GenericRepositoryFunctions.ApplyTextAndLayoutToMonitorView(base.Repositories, this, message, viewPosition, layoutId);
	}
}
