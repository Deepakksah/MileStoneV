using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server.DataComponents;

public static class ConfigurationDataManager
{
	public static string FindRecorderNameByRecorderId(Guid recorderId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecorderNameByRecorderId(recorderId);
	}

	public static string FindRecorderNameByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecorderNameByDeviceId(deviceId);
	}

	public static string FindRecorderWebServerUriByDevice(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecorderWebServerUriByDevice(deviceId);
	}

	public static Guid FindDefaultStorageIdByRecorder(Guid recorderId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindDefaultStorageIdByRecorder(recorderId);
	}

	public static Guid FindRecorderIdByRecordingStorageId(Guid recordingStorageId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecorderIdByRecordingStorageId(recordingStorageId);
	}

	public static Guid FindRecorderIdFromServiceId(Guid serviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecorderIdFromServiceId(serviceId);
	}

	public static Guid FindRecordingServerServiceIdByArchiveStorageId(Guid archiveStorageId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecordingServerServiceIdByArchiveStorageId(archiveStorageId);
	}

	public static Guid FindRecorderIdByHardwareId(Guid hardwareId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecorderIdByHardwareId(hardwareId);
	}

	public static Guid FindActiveRecorderServiceIdByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindActiveRecorderServiceIdByDeviceId(deviceId);
	}

	public static Guid QueryRecorderIdByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryRecorderIdByDeviceId(deviceId);
	}

	public static Guid QueryDeviceEdgeTrackIdByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryDeviceEdgeTrackIdByDeviceId(deviceId);
	}

	public static Guid QueryRecorderIdByDeviceId(IDataComponentFactory componentFactory, Guid deviceId)
	{
		using IConfigurationDataComponent configurationDataComponent = componentFactory.CreateConfigurationDataComponent();
		return configurationDataComponent.QueryRecorderIdByDeviceId(deviceId);
	}

	public static string[] QueryRecorders()
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryRecorders();
	}

	public static string FindHardwareNameByHardwareId(Guid hardwareId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindHardwareNameByHardwareId(hardwareId);
	}

	public static string FindHardwareNameByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindHardwareNameByDeviceId(deviceId);
	}

	public static string FindHardwareMacAddressByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindHardwareMacAddressByDeviceId(deviceId);
	}

	public static string[] QueryHardwareAddressesByRecorder(Guid recorderId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryHardwareAddressesByRecorder(recorderId);
	}

	public static string FindDeviceNameByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindDeviceNameByDeviceId(deviceId);
	}

	public static string FindMatrixNameByMatrixId(Guid matrixId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindMatrixNameByMatrixId(matrixId);
	}

	public static string FindAcUnitNameByMipObjectId(Guid mipObjectId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindAcUnitNameByMipObjectId(mipObjectId);
	}

	public static string FindDeviceTypeByDeviceId(Guid deviceId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindDeviceTypeByDeviceId(deviceId);
	}

	public static string FindRuleNameByRuleId(Guid ruleId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRuleNameByRuleId(ruleId);
	}

	public static string FindRoleNameByRoleId(Guid roleId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRoleNameByRoleId(roleId);
	}

	public static string FindEventTypeDefinitionByEventTypeId(Guid eventTypeId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindEventTypeDefinitionByEventTypeId(eventTypeId);
	}

	public static string FindEventTypeGeneratorTypeByEventTypeId(Guid eventTypeId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindEventTypeGeneratorTypeByEventTypeId(eventTypeId);
	}

	public static string FindEventTypeGeneratorSubtypeByEventTypeId(Guid eventTypeId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindEventTypeGeneratorSubtypeByEventTypeId(eventTypeId);
	}

	public static Guid FindUserDefinedEventTypeIdByName(string eventName)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindUserDefinedEventTypeIdByName(eventName);
	}

	public static Guid FindHardwareEventTypeIdByName(string eventName)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindHardwareEventTypeIdByName(eventName);
	}

	public static Guid FindHardwareEventTypeIdByNameAndDisplayName(string eventName, string eventDisplayName)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindHardwareEventTypeIdByNameAndDisplayName(eventName, eventDisplayName);
	}

	public static string GetNavigationXml()
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.GetNavigationXml();
	}

	public static string FindRecordingStorageNameByRecordingStorageId(Guid recordingStorageId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindRecordingStorageNameByRecordingStorageId(recordingStorageId);
	}

	public static IdNameDescription FindHomeSiteInfo()
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.FindHomeSiteInfo();
	}

	public static void UpdateRecorderStatusData(Guid recorderId, string type, string xml)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.UpdateRecorderStatusData(recorderId, type, xml);
	}

	public static void PromoteNewEncryptionPassword(Guid recordingStorageId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.PromoteNewEncryptionPassword(recordingStorageId);
	}

	public static string[] QueryExistingMacAddresses(string[] macAddresses)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryExistingMacAddresses(macAddresses);
	}

	public static void CheckConfigurationAgainstLicense(ILicenseDocument licenseDocument)
	{
		FeatureManager featureManager = new FeatureManager();
		featureManager.Initialize(licenseDocument);
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.CheckConfigurationAgainstLicense(featureManager);
	}

	public static Guid[] QueryTracksWithPossibleMediaOnRecorder(Guid recorderId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryTracksWithPossibleMediaOnRecorder(recorderId);
	}

	public static string FindRegisteredServerNameById(Guid serverId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryRegisteredServerNamebyId(serverId);
	}

	public static string FindManagementServerNameById(Guid serverId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryManagementNamebyId(serverId);
	}

	public static string FindFailoverServerNameById(Guid serverId)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		return configurationDataComponent.QueryFailoverServerNamebyId(serverId);
	}

	public static void DeleteBookmarksBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.DeleteBookmarksByDeviceIdBeforeTime(deviceId, keepFromTime);
	}

	public static void DeleteRestrictedMediaBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.DeleteRestrictedMediaDeviceBeforeTime(deviceId, keepFromTime);
	}

	public static void DeleteCompletedTasksBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.DeleteCompletedTasksByDeviceIdBeforeTime(deviceId, keepFromTime);
	}

	public static void DeletePrivacyMasksBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.DeletePrivacyMasksBeforeTime(deviceId, keepFromTime);
	}

	public static void UpdateDeviceTrackOnlyUseMediaAfter(Guid trackId, DateTime keepFromTime)
	{
		using ConfigurationDataComponent configurationDataComponent = new ConfigurationDataComponent();
		configurationDataComponent.UpdateDeviceTrackOnlyUseMediaAfter(trackId, keepFromTime);
	}
}
