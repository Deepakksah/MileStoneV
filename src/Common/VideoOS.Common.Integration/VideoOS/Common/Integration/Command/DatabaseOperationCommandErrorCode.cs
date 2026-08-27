namespace VideoOS.Common.Integration.Command;

public enum DatabaseOperationCommandErrorCode
{
	RecorderNotFound,
	DatabaseCommunicationError,
	NotSupportedInFailover,
	PathNotSpecified,
	PathNotValid,
	DeviceStorageOffline,
	StorageOffline,
	ArchiveOffline
}
