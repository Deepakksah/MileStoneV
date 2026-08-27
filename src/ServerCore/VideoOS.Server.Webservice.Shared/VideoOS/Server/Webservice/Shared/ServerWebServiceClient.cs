using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Server.Webservice.Shared;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ServerWebServiceClient : ClientBase<IServerWebService>, IServerWebService
{
	public ServerWebServiceClient()
	{
	}

	public ServerWebServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServerWebServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerWebServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerWebServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void ProcessPendingTasks(string recordingServerId)
	{
		base.Channel.ProcessPendingTasks(recordingServerId);
	}

	public bool IsRecorderOnline(string recordingServerId)
	{
		return base.Channel.IsRecorderOnline(recordingServerId);
	}

	public bool VerifyRecordingStoragePath(string recordingServerId, string recordingStorageId, string path, bool newPath)
	{
		return base.Channel.VerifyRecordingStoragePath(recordingServerId, recordingStorageId, path, newPath);
	}

	public bool VerifyArchiveStoragePath(string recordingServerId, string archiveStorageId, string path, bool newPath)
	{
		return base.Channel.VerifyArchiveStoragePath(recordingServerId, archiveStorageId, path, newPath);
	}

	public bool VerifyStorageAndArchiveOnline(string recordingServerId, string deviceId, string recordingStorageId, string archiveStorageId)
	{
		return base.Channel.VerifyStorageAndArchiveOnline(recordingServerId, deviceId, recordingStorageId, archiveStorageId);
	}

	public string[] ReadDrivesOnRecorder(string recordingServerId)
	{
		return base.Channel.ReadDrivesOnRecorder(recordingServerId);
	}

	public string[] ReadDirectoriesOnRecorder(string recordingServerId, string path)
	{
		return base.Channel.ReadDirectoriesOnRecorder(recordingServerId, path);
	}

	public void CreateDirectoryOnRecorder(string recordingServerId, string path)
	{
		base.Channel.CreateDirectoryOnRecorder(recordingServerId, path);
	}

	public void TriggerUserDefinedEvent(string userDefinedEventId, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.TriggerUserDefinedEvent(userDefinedEventId, basicCallContextInfo);
	}

	public void TriggerPluginEvent(Guid pluginEventId, bool isDeviceEvent, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.TriggerPluginEvent(pluginEventId, isDeviceEvent, sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData, basicCallContextInfo);
	}

	public void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.TriggerPluginEventBulk(pluginEvents, basicCallContextInfo);
	}

	public void TriggerBuiltInEventForDevice(string eventId, string deviceId, DateTime time, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.TriggerBuiltInEventForDevice(eventId, deviceId, time, basicCallContextInfo);
	}

	public string GetVmoServerName()
	{
		return base.Channel.GetVmoServerName();
	}

	public int GetVmoServerPort()
	{
		return base.Channel.GetVmoServerPort();
	}

	public int GetVmoServerSecurePort()
	{
		return base.Channel.GetVmoServerSecurePort();
	}

	public void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		base.Channel.UpdateLicense(license, ownerXml, sitesInformation);
	}

	public void QueueAutomaticActivation()
	{
		base.Channel.QueueAutomaticActivation();
	}

	public int GetLastAutomaticActivationError()
	{
		return base.Channel.GetLastAutomaticActivationError();
	}

	public GetAbsolutePtzPositionResponse GetAbsolutePtzPosition(string deviceId)
	{
		return base.Channel.GetAbsolutePtzPosition(deviceId);
	}

	public ResponseBase UploadFileChunk(Guid transferId, byte[] chunkData, int offset, long fileSize, byte[] fileChecksum)
	{
		return base.Channel.UploadFileChunk(transferId, chunkData, offset, fileSize, fileChecksum);
	}

	public void SendCommand(Guid proxyId, CommandBase command, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.SendCommand(proxyId, command, basicCallContextInfo);
	}

	public ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout)
	{
		return base.Channel.GetResponses(proxyId, timeout);
	}

	public void ReleaseGetResponses(Guid proxyId)
	{
		base.Channel.ReleaseGetResponses(proxyId);
	}

	public SecurityManagerDataTransferObject GetSecurityConfigurationDtoByRole(Guid roleId)
	{
		return base.Channel.GetSecurityConfigurationDtoByRole(roleId);
	}

	public SecurityManagerDataTransferObject GetSecurityConfigurationDtoByTables(string[] tableNames)
	{
		return base.Channel.GetSecurityConfigurationDtoByTables(tableNames);
	}

	public void RefreshIdentityCache()
	{
		base.Channel.RefreshIdentityCache();
	}

	public void ServerRegistrationChanged()
	{
		base.Channel.ServerRegistrationChanged();
	}

	public void MipItemPositionChanged(string fqid, string newPosition)
	{
		base.Channel.MipItemPositionChanged(fqid, newPosition);
	}

	public void MipItemChanged(string fqid, int changeType)
	{
		base.Channel.MipItemChanged(fqid, changeType);
	}

	public void Log(LogEntry logEntry)
	{
		base.Channel.Log(logEntry);
	}

	public void LogLogs(LogEntry[] logEntries)
	{
		base.Channel.LogLogs(logEntries);
	}

	public LogQueryResult QueryLog(LogQuery logQuery)
	{
		return base.Channel.QueryLog(logQuery);
	}

	public byte[] QueryLogExport(LogQuery logQuery, int codePage, CultureInfo culture)
	{
		return base.Channel.QueryLogExport(logQuery, codePage, culture);
	}

	public LogEntryField[] QueryLogFilter(LogFilter logFilter)
	{
		return base.Channel.QueryLogFilter(logFilter);
	}

	public void LogServerChanged()
	{
		base.Channel.LogServerChanged();
	}

	public byte[] GetLicenseRequestFromConfiguration()
	{
		return base.Channel.GetLicenseRequestFromConfiguration();
	}

	public string GetLicenseInformation()
	{
		return base.Channel.GetLicenseInformation();
	}

	public ConfigurationManagerDataTransferObject GetLicenseByLicenseId(Guid licenseId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetLicenseByLicenseId(licenseId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(string digest, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHardwareDefinitionByDigest(digest, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHardwareByRecorderId(recorderId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHardwareByHardwareId(hardwareId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(Guid[] hardwareIds, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHardwareByHardwareIds(hardwareIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(int hostedHardwareId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHardwareByHostedHardwareId(hostedHardwareId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHardwareByDeviceGroupId(deviceGroupId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDevicesByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDevicesByHardwareId(hardwareId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDevicesByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDevicesByRecorderId(recorderId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceByDeviceId(deviceId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDevicesByDeviceIds(deviceIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceGroupByDeviceGroupId(deviceGroupId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceGroupsByParentDeviceGroupId(parentDeviceGroupId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceGroupMembersByDeviceGroupId(deviceGroupId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceGroupMembersByDeviceId(deviceId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceRecorderPlugin(recorderPluginID, deviceId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceRecorderPlugins(recorderPluginID, deviceIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceRecorderPluginsByDeviceId(deviceId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDevicePrivacyMaskByDeviceIds(deviceIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetDeviceTracksByDeviceIds(deviceIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHostedHardwareByIds(int[] hostedHardwareIds, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHostedHardwareByIds(hostedHardwareIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHostingServersByIds(int[] hostingServerIds, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetHostingServersByIds(hostingServerIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(string[] tableNames, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetConfigurationDataSetTables(tableNames, basicCallContextInfo);
	}

	public Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.QueryRolesForDefaultTimeProfile(timeProfileId, basicCallContextInfo);
	}

	public Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.QueryRolesForPermissionsWithTimeProfile(timeProfileId, basicCallContextInfo);
	}

	public GetFileResult ReadFile(string fileName, long index, long count, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.ReadFile(fileName, index, count, basicCallContextInfo);
	}

	public int[] QueryHostedHardwareIdsNotAdded(BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.QueryHostedHardwareIdsNotAdded(basicCallContextInfo);
	}

	public string GetSystemInformationXml()
	{
		return base.Channel.GetSystemInformationXml();
	}

	public string GetNavigationXml()
	{
		return base.Channel.GetNavigationXml();
	}

	public AviCodec64[] GetInstalledCodecs()
	{
		return base.Channel.GetInstalledCodecs();
	}

	public string SendMail(MailGeneratorData mailGeneratorData)
	{
		return base.Channel.SendMail(mailGeneratorData);
	}

	public void FireCustomEvent(string eventName, string eventData, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.FireCustomEvent(eventName, eventData, basicCallContextInfo);
	}

	public UserInfo GetUserInfo(string token, BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetUserInfo(token, basicCallContextInfo);
	}

	public Identity GetUserIdentity(string token)
	{
		return base.Channel.GetUserIdentity(token);
	}

	public void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.EventTriggerWithMetadata(token, eventId, metadata, basicCallContextInfo);
	}

	public void MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		base.Channel.MatrixMonitorActivateCamera(matrixMonitorId, deviceId, basicCallContextInfo);
	}

	public DictionaryEntry[] GetVmoServicesList(BasicCallContextInfo basicCallContextInfo)
	{
		return base.Channel.GetVmoServicesList(basicCallContextInfo);
	}

	public string ValidateToken(out string tokenPassword, string token, string userHostAddress)
	{
		return base.Channel.ValidateToken(out tokenPassword, token, userHostAddress);
	}
}
