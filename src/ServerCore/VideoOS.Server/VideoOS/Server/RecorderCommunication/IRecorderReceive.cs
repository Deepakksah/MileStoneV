using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Server.RecorderCommunication;

public interface IRecorderReceive
{
	bool IsConfigurationCompatible { get; }

	ConfigurationVersionDto GetCurrentConfigurationVersions();

	CompleteConfigurationDto GetCompleteConfigurationDto(bool configDto, bool securityDto, bool tokensDto);

	ServerTaskDto[] GetRecorderTasks(long previousSequenceNumber);

	ServerTaskDto[] GetRecorderTasksWithPendingStop();

	void ProcessReceivedCommand(CommandBase command, ICallContextInfo callContextInfo);

	void ProcessReceivedResponse(ResponseBase response);

	SecurityTokenWithServerTimeDto2 GetSecurityToken(string token);

	ServerResponseStatusDto SetRecorderSpecificConfiguration(Guid targetRecorderId, RecorderSpecificConfigurationDto recorderSpecificConfiguration, bool forceUpdate);

	void Log(LogEntry[] logEntries);

	void RemoveRecorderTask(long taskId);

	void UpdateRecorderTaskProgress(long taskId, int percentComplete, string details, int status, DateTime endTime, ICallContextInfo callContext);

	void StopRecorderTask(long taskId, ICallContextInfo callContext);

	void PromoteNewEncryptionPassword(Guid recordingStorageId);

	Guid[] FilterTrackIdsToThoseThatShouldNotHaveMediaOnRecorder(Guid recorderId, Guid[] trackIds);

	IDictionary<Guid, int> UpdateDriverNumbers(Guid recorderId, IDictionary<Guid, int> driverNumbers);
}
