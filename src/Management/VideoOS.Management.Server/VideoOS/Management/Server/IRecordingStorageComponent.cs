using System;

namespace VideoOS.Management.Server;

public interface IRecordingStorageComponent : IDisposable
{
	void CreateRecordingStorage(RecordingStorage recordingStorage, Guid sessionId);

	RecordingStorage QueryRecordingStorage(string recordingStorageId);

	RecordingStorage QueryRecordingStorage(string recordingServerId, string recordingStorageId);

	RecordingStorage[] QueryRecordingStorages(string[] ids);

	RecordingStorage[] QueryRecordingStorages(string recordingServerId);

	void UpdateRecordingStorage(RecordingStorage recordingStorage, Guid sessionId);

	string GetRecordingServerId(string recordingStorageId);

	void SetRecordingStorageAsDefault(string recordingStorageId);

	string QueryEncryptionPassword(string recordingStorageId);

	StorageDiskInformationInternal QueryRecordingStorageDiskUsage(string recordingStorageId);

	string[] QueryRecordingStorageIds(string recordingServerId);

	string[] QueryRecordingStorageIdsByDeviceId(string deviceId);

	string GetIdFromArchive(string archiveStorageId);
}
