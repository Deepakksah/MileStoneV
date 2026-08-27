using System;

namespace VideoOS.Management.Server;

public interface IArchiveStorageComponent : IDisposable
{
	void CreateArchiveStorage(string recordingStorageId, ArchiveStorageDataContract archiveStorage, Guid sessionId, int limit);

	ArchiveStorageDataContract QueryArchiveStorage(string archiveStorageId);

	ArchiveStorageDataContract[] QueryArchiveStorages(string recordingStorageId);

	ArchiveStorageDataContract[] QueryArchiveStorages(string[] ids);

	void UpdateArchiveStorage(ArchiveStorageDataContract archiveStorage, Guid sessionId);

	StorageDiskInformationInternal QueryArchiveStorageDiskUsage(string archiveStorageId);

	string[] QueryArchiveStorageIdsByDeviceId(string deviceId);

	string[] QueryArchiveStorageIdsByRecordingServerId(string recordingServerId);

	string[] QueryArchiveStorageIdsByRecordingStorageId(string recordingStorageId);

	void EnableFramerateReduction(string archiveStorageId, bool enable, Guid sessionId);

	string GetRecordingServerId(string archiveStorageId);

	string GetRecordingStorageId(string archiveStorageId);
}
