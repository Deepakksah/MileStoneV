using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Server.RecorderCommunication;

public interface IServerComm
{
	void ProcessReceivedResponse(Guid serviceId, ResponseBase response);

	bool IsConfigurationCompatible(Guid serviceId);

	ConfigurationVersionDto GetCurrentConfigurationVersions(Guid serviceId);

	CompleteConfigurationDto GetCompleteConfigurationDto(Guid serviceId, bool configDto, bool securityDto, bool tokensDto);

	SecurityTokenWithServerTimeDto2 GetSecurityToken(Guid serviceId, string token);

	ServerResponseStatusDto SetRecorderSpecificConfiguration(Guid serviceId, Guid targetRecorderId, RecorderSpecificConfigurationDto recorderSpecificConfiguration, bool forceUpdate);

	void Log(Guid serviceId, LogEntry[] logEntries);

	void ProcessReceivedCommand(Guid serviceId, CommandBase command);

	void UpdateEventSubscriptionState(Guid serviceId, bool state);

	ServerTaskDto[] GetRecorderTasks(Guid serviceId, long previousSequenceNumber);

	ServerTaskDto[] GetRecorderTasksWithPendingStop(Guid serviceId);

	void RemoveRecorderTask(Guid serviceId, long taskId);

	void UpdateRecorderTaskProgress(Guid serviceId, long taskId, int percentComplete, string details, int status, DateTime endTime);

	void StopRecorderTask(Guid serviceId, long taskId);

	void PromoteNewEncryptionPassword(Guid serviceId, Guid recordingStorageId);

	void UpdateRecorderStatusData(Guid serviceId, string infoType, string data);

	Guid[] FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(Guid serviceId, Guid targetRecorderId, Guid[] trackIds);

	IDictionary<Guid, int> UpdateDrivers(Guid serviceId, IDictionary<Guid, int> drivers);
}
