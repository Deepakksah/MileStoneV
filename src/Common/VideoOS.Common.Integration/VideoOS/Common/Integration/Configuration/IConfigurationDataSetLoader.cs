using System;

namespace VideoOS.Common.Integration.Configuration;

public interface IConfigurationDataSetLoader
{
	bool IsDynamicLoader { get; }

	ConfigurationDataSet GetLicenseByLicenseId(Guid licenseId);

	ConfigurationDataSet GetLicensesByLicenseIds(Guid[] licenseIds);

	ConfigurationDataSet GetHardwareDefinitionByDigest(string digest);

	ConfigurationDataSet GetHardwareByRecorderId(Guid recorderId);

	ConfigurationDataSet GetHardwareByHardwareId(Guid hardwareId);

	ConfigurationDataSet GetHardwareByHardwareIds(Guid[] hardwareIds);

	ConfigurationDataSet GetHardwareByHostedHardwareId(int hostedHardwareId);

	ConfigurationDataSet GetHardwareByDeviceGroupId(Guid deviceGroupId);

	ConfigurationDataSet GetDevicesByHardwareId(Guid hardwareId);

	ConfigurationDataSet GetDevicesByRecorderId(Guid recorderId);

	ConfigurationDataSet GetDeviceByDeviceId(Guid deviceId);

	ConfigurationDataSet GetDevicesByDeviceIds(Guid[] deviceIds);

	ConfigurationDataSet GetDeviceGroupByDeviceGroupId(Guid deviceGroupId);

	ConfigurationDataSet GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId);

	ConfigurationDataSet GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId);

	ConfigurationDataSet GetDeviceGroupMembersByDeviceId(Guid deviceId);

	ConfigurationDataSet GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId);

	ConfigurationDataSet GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds);

	ConfigurationDataSet GetDeviceRecorderPluginsByDeviceId(Guid deviceId);

	ConfigurationDataSet GetDevicePrivacyMaskByDeviceIds(Guid[] ids);

	ConfigurationDataSet GetDeviceTracksByDeviceIds(Guid[] ids);

	ConfigurationDataSet GetHostingServersByIds(int[] ids);

	ConfigurationDataSet GetHostedHardwareByIds(int[] ids);

	ConfigurationDataSet GetConfigurationDataSetTables(string[] tableNames);

	ConfigurationDataSet GetRecordingStorageDevicesByRecorderId(Guid recorderId);
}
