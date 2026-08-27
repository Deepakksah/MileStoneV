using System;

namespace VideoOS.Management.Server;

public interface ITaskComponent : IDisposable
{
	Task QueryTask(string taskId);

	Task[] QueryTasksByIds(string[] taskIds);

	Task[] QueryTasksByRecordingServer(string recordingServerId);

	Task[] QueryTasksByParentRecordingServer(string recordingServerId);

	Task[] QueryTasksForManagementServer();

	long[] QueryTasksIds(string recordingServerId);

	long[] QueryTasksIds(Task.TaskType taskType, string recordingServerId);

	long[] QueryAllTasksIds();

	void CleanupTask(string taskId);

	void DeleteCompletedTasksByDeviceIdBeforeDt(string deviceId, DateTime dt);

	void RequestStopTask(string taskId);

	void CleanupExpiredTasks(Task.TaskType taskType, int retainMinutes);

	long AddHardware(string recordingServerId, string address, string userName, string password, int driverNumber, string definitionXml, string customData);

	long HardwareScan(string recordingServerId, string address, string userName, string password, bool useDefaultCredentials, string selectedDriversXml, string prioritizedDriversXml, string addressDriverPairsXml);

	long MoveHardware(string hardwareId, string destinationRecorderId, string destinationStorageId, bool ignoreSourceRecordingServer, Guid sessionId);

	long DeleteHardware(string hardwareId);

	long DeleteRecordingServer(string recorderId, bool userAcceptedToDeleteMarkedData);

	long UpdateHardware(string hardwareId, Guid sessionId, long sessionDataId);

	long ReplaceHardware(string hardwareId, string hardwareAddress, int port, bool useHttps, int httpsPort, string userName, string password, int driverNumber, string customDriverData, bool allowDeletingDisabledDevices, string actingUserSid, string actingUserDomainAccountName, string actingUserLocationIPAddress);

	long ChangeHardwarePassword(string hardwareId, string userName, string password, Guid sessionId);

	long ActivateLicense(string userName, string password);

	long FirmwareUpgrade(string hardwareId, string fileId, Guid sessionId);

	long ChangeDeviceRecordingStorage(string deviceId, string targetStorageId, Guid sessionId);

	long ChangeRecordingStorageEncryptionMethod(string recordingStorageId, EncryptionMethod encryptionMethod, Guid sessionId);

	long ChangeRecordingStorageEncryptionPassword(string recordingStorageId, string newPassword, Guid sessionId);

	long DeleteArchiveStorage(string archiveStorageId, Guid sessionId);

	long DeleteRecordingStorage(string recordingStorageId, Guid sessionId);

	long[] DisableRecordingStorageEncryption(string recordingStorageId, Guid sessionId);

	long[] EnableRecordingStorageEncryption(string recordingStorageId, EncryptionMethod encryptionMethod, string password, Guid sessionId);

	long MountArchiveStorage(string archiveStorageId, Guid sessionId);

	long UnmountArchiveStorage(string archiveStorageId, Guid sessionId);

	long MoveArchiveStorage(string archiveStorageId, string newPath, Guid sessionId);

	long MoveRecordingStorage(string recordingStorageId, string newPath, Guid sessionId);

	long RequestEdgeStorageRetrieval(string deviceId, DateTime startTime, DateTime endTime, string metadata, Guid sessionId);

	long TruncateDeviceStorage(string deviceId, string recordingServerId, Guid sessionId);

	long HardwareScanExpress(string recordingServerId, string credentialsXml, bool useDefaultCredentials, bool useSecurePort, string prioritizedDriversXml);

	long GetDevicePresets(string recordingServerId, string deviceId, long sessionDataId);

	long AddIdpAdmin(string id);

	long DeleteIdpAdmin(string id);

	long DeleteIdpUser(string id);
}
