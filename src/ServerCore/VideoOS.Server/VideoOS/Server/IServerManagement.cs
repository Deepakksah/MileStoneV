using System;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Integration.Security;
using VideoOS.Licensing;

namespace VideoOS.Server;

public interface IServerManagement
{
	LicenseException LastAutomaticActivationError { get; }

	void SendCommand(Guid proxyId, CommandBase command, BasicCallContextInfo basicCallContextInfo);

	ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout);

	void ReleaseGetResponses(Guid proxyId);

	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByRole(Guid roleId);

	SecurityManagerDataTransferObject GetSecurityConfigurationDtoByTables(string[] tableNames);

	void RefreshIdentityCache();

	void ServerRegistrationChanged();

	void MipItemPositionChanged(string fqid, string newPosition);

	void MipItemChanged(string fqid, int changeType);

	AviCodec64[] GetInstalledCodecs();

	string SendMail(MailGeneratorData mailGeneratorData);

	void FireCustomEvent(string eventName, string eventData, BasicCallContextInfo basicCallContextInfo);

	Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId);

	Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId);

	void Log(LogEntry logEntry);

	void Log(LogEntry[] logEntries);

	LogQueryResult QueryLog(LogQuery logQuery);

	byte[] QueryLogExport(LogQuery logQuery, int codePage, CultureInfo culture);

	LogEntryField[] QueryLogFilter(LogFilter logFilter);

	void LogServerChanged();

	GetFileResult ReadFile(string fileName, long index, long count);

	byte[] GetLicenseRequestFromConfiguration();

	string GetLicenseInformation();

	LicenseInformation GetLicenseInformationCache();

	string GetSystemInformationXml();

	void UpdateLicense(byte[] encryptedLicense, string ownerXml, string sitesInformation);

	void QueueAutomaticActivation();

	string GetNavigationXml();

	int[] QueryHostedHardwareIdsNotAdded();

	ConfigurationManagerDataTransferObject GetLicenseByLicenseId(Guid licenseId);

	ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(string digest);

	ConfigurationManagerDataTransferObject GetHardwareByRecorderId(Guid recorderId);

	ConfigurationManagerDataTransferObject GetHardwareByHardwareId(Guid hardwareId);

	ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(Guid[] hardwareId);

	ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(int hostedHardwareId);

	ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(Guid deviceGroupId);

	ConfigurationManagerDataTransferObject GetDevicesByHardwareId(Guid hardwareId);

	ConfigurationManagerDataTransferObject GetDevicesByRecorderId(Guid recorderId);

	ConfigurationManagerDataTransferObject GetDeviceByDeviceId(Guid deviceId);

	ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(Guid[] deviceIds);

	ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(Guid deviceGroupId);

	ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId);

	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId);

	ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(Guid deviceId);

	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId);

	ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds);

	ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(Guid deviceId);

	ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds);

	ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(Guid[] deviceIds);

	ConfigurationManagerDataTransferObject GetHostedHardwareByIds(int[] hostedHardwareIds);

	ConfigurationManagerDataTransferObject GetHostingServersByIds(int[] hostingServerIds);

	ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(string[] tableNames);
}
