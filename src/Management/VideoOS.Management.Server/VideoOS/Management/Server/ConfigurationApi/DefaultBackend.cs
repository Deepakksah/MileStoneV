namespace VideoOS.Management.Server.ConfigurationApi;

public class DefaultBackend : IArchiveStorageFolderConfigurationApiBackend
{
	public ArchiveStorageDataContract[] QueryArchiveStorages(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		return ArchiveStorage.QueryArchiveStorages(requestContext, recordingStorageId);
	}

	public RecordingStorage QueryRecordingStorage(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		return RecordingStorage.QueryRecordingStorage(requestContext, recordingStorageId);
	}

	public ArchiveStorageDataContract CreateArchiveStorage(IGeminiRequestContext requestContext, ArchiveStorageDataContract archiveStorage)
	{
		return ArchiveStorage.CreateArchiveStorage(requestContext, archiveStorage);
	}

	public void DeleteArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId, bool deleteIfOffline)
	{
		Task.DeleteArchiveStorage(requestContext, archiveStorageId, deleteIfOffline);
	}
}
