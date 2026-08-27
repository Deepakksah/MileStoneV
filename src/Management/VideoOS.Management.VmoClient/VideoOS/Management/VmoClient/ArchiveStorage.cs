using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ArchiveStorage")]
public class ArchiveStorage : VmoObjectGuidBased, IUpdatable, IDeletable, ICreatable
{
	private Schedule _archiveSchedule;

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

	public string Path => GetPropertyValue<string>("Path");

	public int RetainMinutes
	{
		get
		{
			return GetPropertyValue<int>("RetainMinutes");
		}
		set
		{
			SetPropertyValue("RetainMinutes", value);
		}
	}

	public long MaxSize
	{
		get
		{
			return GetPropertyValue<long>("MaxSize");
		}
		set
		{
			SetPropertyValue("MaxSize", value);
		}
	}

	public bool IsDefault => GetPropertyValue<bool>("IsDefault");

	public double TargetFramerate
	{
		get
		{
			return GetPropertyValue<double>("TargetFramerate");
		}
		set
		{
			SetPropertyValue("TargetFramerate", value);
		}
	}

	public Schedule ArchiveSchedule
	{
		get
		{
			if (_archiveSchedule == null)
			{
				_archiveSchedule = Schedule.FromXml(GetPropertyValue<string>("ArchiveSchedule"));
				_archiveSchedule.ScheduleChanged += ScheduleChanged;
			}
			return _archiveSchedule;
		}
	}

	public bool FramerateReductionEnabled => GetPropertyValue<bool>("FramerateReductionEnabled");

	public string RecordingServerId => GetPropertyValue<string>("RecordingServerId");

	public RecordingServer RecordingServer => GetObjectById<RecordingServer>(RecordingServerId);

	public string RecordingStorageId => GetPropertyValue<string>("RecordingStorageId");

	public RecordingStorage RecordingStorage => GetObjectById<RecordingStorage>(RecordingStorageId);

	public ArchiveStorage(RecordingStorage recordingStorage, string path)
		: this(recordingStorage.Repositories, recordingStorage.Id.ToString(), recordingStorage.RecordingServerId, path, framerateReductionEnabled: false)
	{
	}

	public ArchiveStorage(RecordingStorage recordingStorage, string path, bool framerateReductionEnabled)
		: this(recordingStorage.Repositories, recordingStorage.Id.ToString(), recordingStorage.RecordingServerId, path, framerateReductionEnabled)
	{
	}

	public ArchiveStorage(IVmoRepositories repositories, string recordingStorageId, string recordingServerId, string path, bool framerateReductionEnabled)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: false)
	{
		SetPropertyValue("Path", path);
		SetPropertyValue("FramerateReductionEnabled", framerateReductionEnabled);
		SetPropertyValue("RecordingStorageId", recordingStorageId);
		SetPropertyValue("RecordingServerId", recordingServerId);
		SetPropertyValue("ArchiveSchedule", new Schedule().ToXml());
		RetainMinutes = 2880;
		MaxSize = 1024L;
		TargetFramerate = 5.0;
	}

	public ArchiveStorage(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	private void ScheduleChanged(object sender, EventArgs e)
	{
		SetPropertyValue("ArchiveSchedule", _archiveSchedule.ToXml());
	}

	protected override void PropertiesLoaded()
	{
		_archiveSchedule = null;
	}

	public StorageInformation GetStorageInformation()
	{
		ThrowIfNotCreatedOrDeleted();
		VMOWebserviceObject archiveStorageInformation = base.Repositories.ServerCommunication.GetArchiveStorageInformation(base.InternalId);
		if (archiveStorageInformation == null)
		{
			return null;
		}
		return new StorageInformation(archiveStorageInformation.Properties);
	}

	public MoveArchiveTask Move(string newPath)
	{
		ThrowIfNotCreatedOrDeleted();
		MoveArchiveTask result = MoveArchiveTask.StartNew(base.Repositories, base.Id, newPath);
		Refresh();
		return result;
	}

	public void EnableFramerateReduction()
	{
		EnableFramerateReduction(enable: true);
	}

	public void DisableFramerateReduction()
	{
		EnableFramerateReduction(enable: false);
	}

	private void EnableFramerateReduction(bool enable)
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.EnableFramerateReductionArchiveStorage(base.InternalId, enable);
		Refresh();
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

	public DeleteArchiveTask Delete(bool deleteIfOffline)
	{
		ThrowIfNotCreatedOrDeleted();
		return DeleteArchiveTask.StartNew(base.Repositories, base.Id, deleteIfOffline);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}
}
