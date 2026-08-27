using System;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("RecordingDevice")]
public abstract class RecordingDevice : Device
{
	public string RecordingStorageId => GetPropertyValue<string>("RecordingStorageId");

	public RecordingStorage RecordingStorage => GetObjectById<RecordingStorage>(RecordingStorageId);

	public bool RecordingEnabled
	{
		get
		{
			return GetPropertyValue<bool>("RecordingEnabled");
		}
		set
		{
			SetPropertyValue("RecordingEnabled", value);
		}
	}

	public bool PrebufferEnabled
	{
		get
		{
			return GetPropertyValue<bool>("PrebufferEnabled");
		}
		set
		{
			SetPropertyValue("PrebufferEnabled", value);
		}
	}

	public bool PrebufferInMemory
	{
		get
		{
			return GetPropertyValue<bool>("PrebufferInMemory");
		}
		set
		{
			SetPropertyValue("PrebufferInMemory", value);
		}
	}

	public int PrebufferSeconds
	{
		get
		{
			return GetPropertyValue<int>("PrebufferSeconds");
		}
		set
		{
			SetPropertyValue("PrebufferSeconds", value);
		}
	}

	public bool EdgeStorageEnabled
	{
		get
		{
			return GetPropertyValue<bool>("EdgeStorageEnabled");
		}
		set
		{
			SetPropertyValue("EdgeStorageEnabled", value);
		}
	}

	public bool EdgeStoragePlaybackEnabled
	{
		get
		{
			return GetPropertyValue<bool>("EdgeStoragePlaybackEnabled");
		}
		set
		{
			SetPropertyValue("EdgeStoragePlaybackEnabled", value);
		}
	}

	public bool ManualRecordingTimeoutEnabled
	{
		get
		{
			return GetPropertyValue<bool>("ManualRecordingTimeoutEnabled");
		}
		set
		{
			SetPropertyValue("ManualRecordingTimeoutEnabled", value);
		}
	}

	public int ManualRecordingTimeoutMinutes
	{
		get
		{
			return GetPropertyValue<int>("ManualRecordingTimeoutMinutes");
		}
		set
		{
			SetPropertyValue("ManualRecordingTimeoutMinutes", value);
		}
	}

	public RecordingDevice(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public TruncateDeviceStorageTask TruncateStorage()
	{
		ThrowIfNotCreatedOrDeleted();
		return TruncateDeviceStorageTask.StartNew(base.Repositories, base.Id);
	}

	public TruncateDeviceStorageTask TruncateStorage(Guid recordingServerId)
	{
		ThrowIfNotCreatedOrDeleted();
		return TruncateDeviceStorageTask.StartNew(base.Repositories, base.Id, recordingServerId);
	}

	public Guid[] QueryOtherRecordingServersWithMedia()
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		return (from o in base.Repositories.ServerCommunication.Execute("QueryOtherRecordingServersWithMedia", vmoPropertyCollection)
			select new Guid(o.Id)).ToArray();
	}

	public MoveDeviceTask ChangeRecordingStorage(Guid targetStorageId)
	{
		ThrowIfNotCreatedOrDeleted();
		MoveDeviceTask result = MoveDeviceTask.StartNew(base.Repositories, base.Id, targetStorageId);
		Refresh();
		return result;
	}

	public MoveDeviceTask ChangeRecordingStorage(Guid targetStorageId, bool moveData)
	{
		ThrowIfNotCreatedOrDeleted();
		MoveDeviceTask result = MoveDeviceTask.StartNew(base.Repositories, base.Id, targetStorageId);
		Refresh();
		return result;
	}

	public RequestEdgeStorageTask RequestEdgeStorageRetrieval(DateTime startTime, DateTime endTime, string metadata)
	{
		ThrowIfNotCreatedOrDeleted();
		return RequestEdgeStorageTask.StartNew(base.Repositories, base.Id, startTime, endTime, metadata);
	}
}
