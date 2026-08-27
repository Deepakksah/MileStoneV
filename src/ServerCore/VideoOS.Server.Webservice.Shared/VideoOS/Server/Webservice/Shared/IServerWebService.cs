using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.Net.Security;
using System.ServiceModel;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Server.Webservice.Shared;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://schemas.videoos/ServerWebService/2012/11/01", ConfigurationName = "VideoOS.Server.Webservice.Shared.IServerWebService")]
public interface IServerWebService
{
	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ProcessPendingTasks", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ProcessPendingTasksResponse")]
	void ProcessPendingTasks(string recordingServerId);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/IsRecorderOnline", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/IsRecorderOnlineResponse")]
	bool IsRecorderOnline(string recordingServerId);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyRecordingStoragePath", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyRecordingStoragePathResponse")]
	[FaultContract(typeof(RecordingServerUnavailableFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyRecordingStoragePathRecordingServerUnavailableFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerUnavailableFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyRecordingStoragePathRecordingServerCommunicationTimeoutFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerCommunicationTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	bool VerifyRecordingStoragePath(string recordingServerId, string recordingStorageId, string path, bool newPath);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyArchiveStoragePath", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyArchiveStoragePathResponse")]
	[FaultContract(typeof(RecordingServerUnavailableFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyArchiveStoragePathRecordingServerUnavailableFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerUnavailableFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyArchiveStoragePathRecordingServerCommunicationTimeoutFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerCommunicationTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	bool VerifyArchiveStoragePath(string recordingServerId, string archiveStorageId, string path, bool newPath);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyStorageAndArchiveOnline", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyStorageAndArchiveOnlineResponse")]
	[FaultContract(typeof(RecordingServerUnavailableFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyStorageAndArchiveOnlineRecordingServerUnavailableFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerUnavailableFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/VerifyStorageAndArchiveOnlineRecordingServerCommunicationTimeoutFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerCommunicationTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	bool VerifyStorageAndArchiveOnline(string recordingServerId, string deviceId, string recordingStorageId, string archiveStorageId);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDrivesOnRecorder", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDrivesOnRecorderResponse")]
	[FaultContract(typeof(RecordingServerUnavailableFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDrivesOnRecorderRecordingServerUnavailableFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerUnavailableFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDrivesOnRecorderRecordingServerCommunicationTimeoutFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerCommunicationTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerOperationFailedFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDrivesOnRecorderRecordingServerOperationFailedFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerOperationFailedFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	string[] ReadDrivesOnRecorder(string recordingServerId);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDirectoriesOnRecorder", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDirectoriesOnRecorderResponse")]
	[FaultContract(typeof(RecordingServerUnavailableFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDirectoriesOnRecorderRecordingServerUnavailableFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerUnavailableFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDirectoriesOnRecorderRecordingServerCommunicationTimeoutFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerCommunicationTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerOperationFailedFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadDirectoriesOnRecorderRecordingServerOperationFailedFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerOperationFailedFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	string[] ReadDirectoriesOnRecorder(string recordingServerId, string path);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/CreateDirectoryOnRecorder", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/CreateDirectoryOnRecorderResponse")]
	[FaultContract(typeof(RecordingServerUnavailableFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/CreateDirectoryOnRecorderRecordingServerUnavailableFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerUnavailableFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/CreateDirectoryOnRecorderRecordingServerCommunicationTimeoutFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerCommunicationTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerOperationFailedFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/CreateDirectoryOnRecorderRecordingServerOperationFailedFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerOperationFailedFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	void CreateDirectoryOnRecorder(string recordingServerId, string path);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerUserDefinedEvent", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerUserDefinedEventResponse")]
	void TriggerUserDefinedEvent(string userDefinedEventId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerPluginEvent", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerPluginEventResponse")]
	void TriggerPluginEvent(Guid pluginEventId, bool isDeviceEvent, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerPluginEventBulk", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerPluginEventBulkResponse")]
	void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerBuiltInEventForDevice", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/TriggerBuiltInEventForDeviceResponse")]
	void TriggerBuiltInEventForDevice(string eventId, string deviceId, DateTime time, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServerName", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServerNameResponse")]
	string GetVmoServerName();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServerPort", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServerPortResponse")]
	int GetVmoServerPort();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServerSecurePort", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServerSecurePortResponse")]
	int GetVmoServerSecurePort();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/UpdateLicense", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/UpdateLicenseResponse")]
	[FaultContract(typeof(LicenseExceptionFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/UpdateLicenseLicenseExceptionFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "LicenseExceptionFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	void UpdateLicense(byte[] license, string ownerXml, string sitesInformation);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueueAutomaticActivation", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueueAutomaticActivationResponse")]
	void QueueAutomaticActivation();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLastAutomaticActivationError", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLastAutomaticActivationErrorResponse")]
	int GetLastAutomaticActivationError();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetAbsolutePtzPosition", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetAbsolutePtzPositionResponse")]
	[FaultContract(typeof(RecordingServerUnavailableFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetAbsolutePtzPositionRecordingServerUnavailableFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerUnavailableFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetAbsolutePtzPositionRecordingServerCommunicationTimeoutFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "RecordingServerCommunicationTimeoutFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	[FaultContract(typeof(ErrorResponseFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetAbsolutePtzPositionErrorResponseFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "ErrorResponseFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	GetAbsolutePtzPositionResponse GetAbsolutePtzPosition(string deviceId);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/UploadFileChunk", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/UploadFileChunkResponse")]
	[FaultContract(typeof(ErrorResponseFault), Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/UploadFileChunkErrorResponseFaultFault", ProtectionLevel = ProtectionLevel.None, Name = "ErrorResponseFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Integration")]
	ResponseBase UploadFileChunk(Guid transferId, byte[] chunkData, int offset, long fileSize, byte[] fileChecksum);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/SendCommand", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/SendCommandResponse")]
	[ServiceKnownType(typeof(CommonCommandBase))]
	[ServiceKnownType(typeof(RecorderAttachCommand))]
	[ServiceKnownType(typeof(AuditLogCommand))]
	[ServiceKnownType(typeof(BackupConfigCommand))]
	[ServiceKnownType(typeof(DatabaseOperationCommand))]
	[ServiceKnownType(typeof(HardwareScanCommand))]
	[ServiceKnownType(typeof(HardwareSetupInformationCommand))]
	[ServiceKnownType(typeof(GetAbsolutePtzPositionCommand))]
	[ServiceKnownType(typeof(PtzReserveSessionCommand))]
	[ServiceKnownType(typeof(PtzReleaseSessionCommand))]
	[ServiceKnownType(typeof(PtzStopSessionCommand))]
	[ServiceKnownType(typeof(RecorderStatusCommand))]
	[ServiceKnownType(typeof(ReplaceHardwareCommand))]
	[ServiceKnownType(typeof(ServerFileSystemOperationCommand))]
	[ServiceKnownType(typeof(SynchronizeDevicePresetsCommand))]
	[ServiceKnownType(typeof(UpdateConfigurationManagerCommand))]
	[ServiceKnownType(typeof(UpdateSecurityManagerCommand))]
	[ServiceKnownType(typeof(DeleteConfigurationItemCommand))]
	[ServiceKnownType(typeof(GetHardwareNetworkSettingsCommand))]
	[ServiceKnownType(typeof(SetHardwareNetworkSettingsCommand))]
	[ServiceKnownType(typeof(GetHardwareUsersCommand))]
	void SendCommand(Guid proxyId, CommandBase command, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetResponses", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetResponsesResponse")]
	ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReleaseGetResponses", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReleaseGetResponsesResponse")]
	void ReleaseGetResponses(Guid proxyId);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetSecurityConfigurationDtoByRole", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetSecurityConfigurationDtoByRoleResponse")]
	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByRole(Guid roleId);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetSecurityConfigurationDtoByTables", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetSecurityConfigurationDtoByTablesResponse")]
	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByTables(string[] tableNames);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/RefreshIdentityCache", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/RefreshIdentityCacheResponse")]
	void RefreshIdentityCache();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ServerRegistrationChanged", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ServerRegistrationChanged")]
	void ServerRegistrationChanged();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/MipItemPositionChanged", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/MipItemPositionChangedResponse")]
	void MipItemPositionChanged(string fqid, string newPosition);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/MipItemChanged", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/MipItemChangedResponse")]
	void MipItemChanged(string fqid, int changeType);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/Log", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/LogResponse")]
	[ServiceKnownType(typeof(LogEntrySystem))]
	[ServiceKnownType(typeof(LogEntryRule))]
	[ServiceKnownType(typeof(LogEntryAudit))]
	void Log(LogEntry logEntry);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/LogLogs", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/LogLogsResponse")]
	void LogLogs(LogEntry[] logEntries);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryLog", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryLogResponse")]
	LogQueryResult QueryLog(LogQuery logQuery);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryLogExport", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryLogExportResponse")]
	byte[] QueryLogExport(LogQuery logQuery, int codePage, CultureInfo culture);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryLogFilter", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryLogFilterResponse")]
	LogEntryField[] QueryLogFilter(LogFilter logFilter);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/LogServerChanged", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/LogServerChangedResponse")]
	void LogServerChanged();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLicenseRequestFromConfiguration", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLicenseRequestFromConfigurationResponse")]
	byte[] GetLicenseRequestFromConfiguration();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLicenseInformation", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLicenseInformationResponse")]
	string GetLicenseInformation();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLicenseByLicenseId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetLicenseByLicenseIdResponse")]
	ConfigurationManagerDataTransferObject GetLicenseByLicenseId(Guid licenseId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareDefinitionByDigest", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareDefinitionByDigestResponse")]
	ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(string digest, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByRecorderId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByRecorderIdResponse")]
	ConfigurationManagerDataTransferObject GetHardwareByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByHardwareId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByHardwareIdResponse")]
	ConfigurationManagerDataTransferObject GetHardwareByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByHardwareIds", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByHardwareIdsResponse")]
	ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(Guid[] hardwareIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByHostedHardwareId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByHostedHardwareIdResponse")]
	ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(int hostedHardwareId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByDeviceGroupId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHardwareByDeviceGroupIdResponse")]
	ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicesByHardwareId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicesByHardwareIdResponse")]
	ConfigurationManagerDataTransferObject GetDevicesByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicesByRecorderId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicesByRecorderIdResponse")]
	ConfigurationManagerDataTransferObject GetDevicesByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceByDeviceId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceByDeviceIdResponse")]
	ConfigurationManagerDataTransferObject GetDeviceByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicesByDeviceIds", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicesByDeviceIdsResponse")]
	ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupByDeviceGroupId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupByDeviceGroupIdResponse")]
	ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupsByParentDeviceGroupId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupsByParentDeviceGroupIdResponse")]
	ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupMembersByDeviceGroupId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupMembersByDeviceGroupIdResponse")]
	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupMembersByDeviceId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceGroupMembersByDeviceIdResponse")]
	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceRecorderPlugin", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceRecorderPluginResponse")]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceRecorderPlugins", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceRecorderPluginsResponse")]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceRecorderPluginsByDeviceId", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceRecorderPluginsByDeviceIdResponse")]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicePrivacyMaskByDeviceIds", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDevicePrivacyMaskByDeviceIdsResponse")]
	ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceTracksByDeviceIds", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetDeviceTracksByDeviceIdsResponse")]
	ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHostedHardwareByIds", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHostedHardwareByIdsResponse")]
	ConfigurationManagerDataTransferObject GetHostedHardwareByIds(int[] hostedHardwareIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHostingServersByIds", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetHostingServersByIdsResponse")]
	ConfigurationManagerDataTransferObject GetHostingServersByIds(int[] hostingServerIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetConfigurationDataSetTables", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetConfigurationDataSetTablesResponse")]
	ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(string[] tableNames, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryRolesForDefaultTimeProfile", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryRolesForDefaultTimeProfileResponse")]
	Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryRolesForPermissionsWithTimeProfile", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryRolesForPermissionsWithTimeProfileResponse")]
	Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadFile", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ReadFileResponse")]
	GetFileResult ReadFile(string fileName, long index, long count, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryHostedHardwareIdsNotAdded", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/QueryHostedHardwareIdsNotAddedResponse")]
	int[] QueryHostedHardwareIdsNotAdded(BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetSystemInformationXml", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetSystemInformationXmlResponse")]
	string GetSystemInformationXml();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetNavigationXml", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetNavigationXmlResponse")]
	string GetNavigationXml();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetInstalledCodecs", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetInstalledCodecsResponse")]
	AviCodec64[] GetInstalledCodecs();

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/SendMail", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/SendMailResponse")]
	string SendMail(MailGeneratorData mailGeneratorData);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/FireCustomEvent", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/FireCustomEventResponse")]
	void FireCustomEvent(string eventName, string eventData, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetUserInfo", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetUserInfoResponse")]
	UserInfo GetUserInfo(string token, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetUserIdentity", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetUserIdentityResponse")]
	Identity GetUserIdentity(string token);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/EventTriggerWithMetadata", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/EventTriggerWithMetadataResponse")]
	void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/MatrixMonitorActivateCamera", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/MatrixMonitorActivateCameraResponse")]
	void MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServicesList", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/GetVmoServicesListResponse")]
	DictionaryEntry[] GetVmoServicesList(BasicCallContextInfo basicCallContextInfo);

	[OperationContract(ProtectionLevel = ProtectionLevel.EncryptAndSign, Action = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ValidateToken", ReplyAction = "http://schemas.videoos/ServerWebService/2012/11/01/IServerWebService/ValidateTokenResponse")]
	[return: MessageParameter(Name = "tokenUserName")]
	string ValidateToken(out string tokenPassword, string token, string userHostAddress);
}
