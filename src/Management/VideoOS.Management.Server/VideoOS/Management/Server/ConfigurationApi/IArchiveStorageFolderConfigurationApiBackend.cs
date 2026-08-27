namespace VideoOS.Management.Server.ConfigurationApi;

public interface IArchiveStorageFolderConfigurationApiBackend
{
	ArchiveStorageDataContract[] QueryArchiveStorages(IGeminiRequestContext requestContext, string recordingStorageId);

	RecordingStorage QueryRecordingStorage(IGeminiRequestContext requestContext, string recordingStorageId);

	ArchiveStorageDataContract CreateArchiveStorage(IGeminiRequestContext requestContext, ArchiveStorageDataContract archiveStorage);

	void DeleteArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId, bool deleteIfOffline);
}
