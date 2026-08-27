using System;
using System.Globalization;
using System.ServiceModel;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

[ServiceContract(Namespace = "http://schemas.videoos/ServerProxyService/2008/06/Vmo/01")]
public interface IServerProxyService
{
	[OperationContract]
	SecurityToken Login(string currentToken);

	[OperationContract]
	void Logout(string currentToken);

	[OperationContract]
	string Authorize(string userDomainName, AuthorizationActionType type);

	[OperationContract]
	Change[] QueryChanges(DateTime changesFrom);

	[OperationContract]
	RecorderInfo QueryRecorderInfo(Guid recorderId, string serverHostAddress);

	[OperationContract]
	void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live);

	[OperationContract]
	MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract]
	MarkedDataResult MarkedDataUpdate(Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract]
	MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedData MarkedDataGet(string token, Guid markedDataId);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch2(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	[OperationContract]
	bool RecordingServerContainMarkedData(string recordingServerId);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime startTime, DateTime endTime);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime startTime, DateTime endTime);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaDelete(Guid id);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.RestrictedMedia[] QueryAll();

	[OperationContract]
	RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime startTime);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime startTime, DateTime endTime);

	[OperationContract]
	RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime startTime);

	[OperationContract]
	RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds);

	[OperationContract]
	RestrictedMediaLive[] QueryAllLive();

	[OperationContract]
	string GetServerVersion();

	[OperationContract]
	string GetUserHostAddress();

	[OperationContract]
	void SendCommand(Guid proxyId, CommandBase command);

	[OperationContract]
	ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout);

	[OperationContract]
	void ReleaseGetResponses(Guid proxyId);

	[OperationContract]
	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByRole(Guid roleId);

	[OperationContract]
	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByTables(string[] tableNames);

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
	byte[] GetLicenseRequestFromConfiguration();

	[OperationContract]
	string GetSystemInformationXml();

	[OperationContract]
	string GetNavigationXml();

	[OperationContract]
	string GetLicenseInformation();

	[OperationContract]
	AviCodec64[] GetInstalledCodecs();

	[OperationContract]
	string SendMail(MailGeneratorData mailGeneratorData);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetLicenseByLicenseId(Guid licenseId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(Guid[] deviceIds);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(string digest);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByRecorderId(Guid recorderId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByHardwareId(Guid hardwareId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(Guid[] hardwareIds);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(int hostedHardwareId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(Guid deviceGroupId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicesByHardwareId(Guid hardwareId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicesByRecorderId(Guid recorderId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceByDeviceId(Guid deviceId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(Guid[] deviceIds);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(Guid deviceGroupId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(Guid deviceId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(Guid deviceId);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHostedHardwareByIds(int[] hostedHardwareIds);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetHostingServersByIds(int[] hostingServerIds);

	[OperationContract]
	ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(string[] tableNames);

	[OperationContract]
	Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId);

	[OperationContract]
	Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId);

	[OperationContract]
	GetFileResult ReadFile(string fileName, long index, long count);

	[OperationContract]
	int[] QueryHostedHardwareIdsNotAdded();
}
