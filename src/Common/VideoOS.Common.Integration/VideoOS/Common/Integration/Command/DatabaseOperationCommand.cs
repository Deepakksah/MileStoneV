using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DatabaseOperationCommand : CommonCommandBase
{
	[DataMember]
	private Guid[] _deviceIds;

	[DataMember]
	private Guid[] _trackIds;

	[DataMember]
	public DatabaseOperation DatabaseOperationType { get; private set; }

	[DataMember]
	public Guid RecorderId { get; set; }

	[DataMember]
	public Guid StorageId { get; set; }

	[DataMember]
	public Guid ArchiveId { get; set; }

	[DataMember]
	public string Path { get; set; }

	[DataMember]
	public bool NewPath { get; set; }

	[DataMember]
	public string ArchiveDefinition { get; set; }

	[DataMember]
	public Guid DeviceId { get; private set; }

	[DataMember]
	public bool Recursive { get; private set; }

	public IEnumerable<Guid> DeviceIds
	{
		[DebuggerStepThrough]
		get
		{
			IEnumerable<Guid> deviceIds = _deviceIds;
			return deviceIds ?? Enumerable.Empty<Guid>();
		}
	}

	public IEnumerable<Guid> TrackIds
	{
		[DebuggerStepThrough]
		get
		{
			IEnumerable<Guid> trackIds = _trackIds;
			return trackIds ?? Enumerable.Empty<Guid>();
		}
	}

	public DatabaseOperationCommand()
	{
	}

	public DatabaseOperationCommand(DatabaseOperation databaseOperationType, Guid recorderId, Guid storageId, Guid archiveId, string path, bool newPath, string archiveDefinition, Guid deviceId, Guid[] deviceIds, Guid[] trackIds, bool recursive)
	{
		DatabaseOperationType = databaseOperationType;
		RecorderId = recorderId;
		StorageId = storageId;
		ArchiveId = archiveId;
		Path = path;
		NewPath = newPath;
		ArchiveDefinition = archiveDefinition;
		DeviceId = deviceId;
		_deviceIds = deviceIds;
		_trackIds = trackIds;
		Recursive = recursive;
	}

	public void ReplaceDeviceIds(Guid[] deviceIds)
	{
		_deviceIds = deviceIds;
	}

	public void VerifyPath(Guid recorderId, Guid storageId, string path, bool newPath)
	{
		DatabaseOperationType = DatabaseOperation.VerifyPath;
		RecorderId = recorderId;
		StorageId = storageId;
		Path = path;
		NewPath = newPath;
	}

	public void VerifyNewArchivePath(Guid recorderId, Guid archiveId, string path, bool newPath)
	{
		DatabaseOperationType = DatabaseOperation.VerifyNewArchivePath;
		RecorderId = recorderId;
		ArchiveId = archiveId;
		Path = path;
		NewPath = newPath;
	}

	public void VerifyStorageAndArchiveOnline(Guid recorderId, Guid deviceId, Guid storageId, Guid archiveId)
	{
		DatabaseOperationType = DatabaseOperation.VerifyStorageAndArchiveOnline;
		RecorderId = recorderId;
		DeviceId = deviceId;
		StorageId = storageId;
		ArchiveId = archiveId;
	}

	public void VerifyAllStoragesOnline(Guid recorderId)
	{
		DatabaseOperationType = DatabaseOperation.VerifyStorageAndArchiveOnline;
		RecorderId = recorderId;
		DeviceId = Guid.Empty;
		StorageId = Guid.Empty;
		ArchiveId = Guid.Empty;
	}

	public void GetDiskUsageInformationDevicesPerStorage(Guid recorderId, IEnumerable<Guid> deviceIds)
	{
		DatabaseOperationType = DatabaseOperation.DiskUsageInformationTablesPerStorage;
		RecorderId = recorderId;
		if (deviceIds != null)
		{
			_deviceIds = deviceIds.ToArray();
		}
	}

	public void FilterTrackIdsToThoseWithoutMediaOnRecorder(Guid recorderId, Guid[] trackIds)
	{
		DatabaseOperationType = DatabaseOperation.FilterTrackIdsToThoseWithoutMediaOnRecorder;
		RecorderId = recorderId;
		_trackIds = trackIds;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
