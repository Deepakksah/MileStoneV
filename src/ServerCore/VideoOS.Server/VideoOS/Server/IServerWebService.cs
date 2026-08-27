using System;
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

namespace VideoOS.Server;

[ServiceContract(Namespace = "http://schemas.videoos/ServerWebService/2012/11/01")]
public interface IServerWebService
{
	[OperationContract]
	void ProcessPendingTasks(string recordingServerId);

	[OperationContract]
	bool IsRecorderOnline(string recordingServerId);

	[OperationContract]
	[FaultContract(typeof(RecordingServerUnavailableFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), ProtectionLevel = ProtectionLevel.None)]
	bool VerifyRecordingStoragePath(string recordingServerId, string recordingStorageId, string path, bool newPath);

	[OperationContract]
	[FaultContract(typeof(RecordingServerUnavailableFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), ProtectionLevel = ProtectionLevel.None)]
	bool VerifyArchiveStoragePath(string recordingServerId, string archiveStorageId, string path, bool newPath);

	[OperationContract]
	[FaultContract(typeof(RecordingServerUnavailableFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), ProtectionLevel = ProtectionLevel.None)]
	bool VerifyStorageAndArchiveOnline(string recordingServerId, string deviceId, string recordingStorageId, string archiveStorageId);

	[OperationContract]
	[FaultContract(typeof(RecordingServerUnavailableFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerOperationFailedFault), ProtectionLevel = ProtectionLevel.None)]
	string[] ReadDrivesOnRecorder(string recordingServerId);

	[OperationContract]
	[FaultContract(typeof(RecordingServerUnavailableFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerOperationFailedFault), ProtectionLevel = ProtectionLevel.None)]
	string[] ReadDirectoriesOnRecorder(string recordingServerId, string path);

	[OperationContract]
	[FaultContract(typeof(RecordingServerUnavailableFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerOperationFailedFault), ProtectionLevel = ProtectionLevel.None)]
	void CreateDirectoryOnRecorder(string recordingServerId, string path);

	[OperationContract]
	void TriggerUserDefinedEvent(string userDefinedEventId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	void TriggerPluginEvent(Guid pluginEventId, bool isDeviceEvent, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	void TriggerBuiltInEventForDevice(string eventId, string deviceId, DateTime time, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	string GetVmoServerName();

	[OperationContract]
	int GetVmoServerPort();

	[OperationContract]
	int GetVmoServerSecurePort();

	[OperationContract]
	[FaultContract(typeof(LicenseExceptionFault), ProtectionLevel = ProtectionLevel.None)]
	void UpdateLicense(byte[] license, string ownerXml, string sitesInformation);

	[OperationContract]
	void QueueAutomaticActivation();

	[OperationContract]
	int GetLastAutomaticActivationError();

	[OperationContract]
	[FaultContract(typeof(RecordingServerUnavailableFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(RecordingServerCommunicationTimeoutFault), ProtectionLevel = ProtectionLevel.None)]
	[FaultContract(typeof(ErrorResponseFault), ProtectionLevel = ProtectionLevel.None)]
	GetAbsolutePtzPositionResponse GetAbsolutePtzPosition(string deviceId);

	[OperationContract]
	[FaultContract(typeof(ErrorResponseFault), ProtectionLevel = ProtectionLevel.None)]
	ResponseBase UploadFileChunk(Guid transferId, byte[] chunkData, int offset, long fileSize, byte[] fileChecksum);

	[OperationContract]
	void SendCommand(Guid proxyId, CommandBase command, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout);

	[OperationContract]
	void ReleaseGetResponses(Guid proxyId);

	[OperationContract]
	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByRole(Guid roleId);

	[OperationContract]
	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByTables(string[] tableNames);

	[OperationContract]
	void RefreshIdentityCache();

	[OperationContract]
	void Log(LogEntry logEntry);

	[OperationContract]
	void LogLogs(LogEntry[] logEntries);

	[OperationContract]
	LogQueryResult QueryLog(LogQuery logQuery);

	[OperationContract]
	byte[] QueryLogExport(LogQuery logQuery, int codePage, CultureInfo culture);

	[OperationContract]
	LogEntryField[] QueryLogFilter(LogFilter logFilter);

	[OperationContract]
	void LogServerChanged();

	[OperationContract]
	byte[] GetLicenseRequestFromConfiguration();

	[OperationContract]
	string GetLicenseInformation();

	[OperationContract]
	ConfigurationManagerDataTransferObject GetLicenseByLicenseId(Guid licenseId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(string digest, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(Guid[] hardwareIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(int hostedHardwareId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicesByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicesByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHostedHardwareByIds(int[] hostedHardwareIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHostingServersByIds(int[] hostingServerIds, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(string[] tableNames, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	GetFileResult ReadFile(string fileName, long index, long count, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	int[] QueryHostedHardwareIdsNotAdded(BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	string GetSystemInformationXml();

	[OperationContract]
	string GetNavigationXml();

	[OperationContract]
	AviCodec64[] GetInstalledCodecs();

	[OperationContract]
	string SendMail(MailGeneratorData mailGeneratorData);

	[OperationContract]
	void FireCustomEvent(string eventName, string eventData, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	UserInfo GetUserInfo(string token, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	Identity GetUserIdentity(string token);

	[OperationContract]
	void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	void MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	[OperationContract]
	DictionaryEntry[] GetVmoServicesList(BasicCallContextInfo basicCallContextInfo);

	[OperationContract(IsOneWay = false)]
	void ValidateToken(string token, string userHostAddress, out string tokenUserName, out string tokenPassword);

	[OperationContract]
	void ServerRegistrationChanged();

	[OperationContract]
	void MipItemPositionChanged(string fqid, string newPosition);

	[OperationContract]
	void MipItemChanged(string fqid, int changeType);
}
