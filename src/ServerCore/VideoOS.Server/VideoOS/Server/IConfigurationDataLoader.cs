using System;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server;

public interface IConfigurationDataLoader
{
	ConfigurationDataSet LoadLicenseByLicenseId(Guid licenseId);

	ConfigurationDataSet LoadLicensesByLicenseIds(Guid[] licenseIds);

	ConfigurationDataSet LoadConfigurationDataSetTables(string[] tableNames);

	ConfigurationDataSet LoadDeviceByDeviceId(Guid deviceId);

	ConfigurationDataSet LoadDeviceGroupByDeviceGroupId(Guid deviceGroupId);

	ConfigurationDataSet LoadDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId);

	ConfigurationDataSet LoadDeviceGroupMembersByDeviceId(Guid deviceId);

	ConfigurationDataSet LoadDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId);

	ConfigurationDataSet LoadDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId);

	ConfigurationDataSet LoadDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds);

	ConfigurationDataSet LoadDeviceRecorderPluginsByDeviceId(Guid deviceId);

	ConfigurationDataSet LoadDevicePrivacyMaskByDeviceIds(Guid[] deviceIds);

	ConfigurationDataSet LoadDeviceTracksByDeviceIds(Guid[] deviceIds);

	ConfigurationDataSet LoadDevicesByDeviceIds(Guid[] deviceIds);

	ConfigurationDataSet LoadDevicesByHardwareId(Guid hardwareId);

	ConfigurationDataSet LoadDevicesByRecorderId(Guid recorderId);

	ConfigurationDataSet LoadHardwareByDeviceGroupId(Guid deviceGroupId);

	ConfigurationDataSet LoadHardwareByHardwareId(Guid hardwareId);

	ConfigurationDataSet LoadHardwareByHardwareIds(Guid[] hardwareIds);

	ConfigurationDataSet LoadHardwareByHostedHardwareId(int hostedHardwareId);

	ConfigurationDataSet LoadHardwareByRecorderId(Guid recorderId);

	ConfigurationDataSet LoadHardwareDefinitionByDigest(string digest);

	ConfigurationDataSet LoadHostedHardwareByIds(int[] ids);

	ConfigurationDataSet LoadHostingServersByIds(int[] ids);

	ConfigurationDataSet LoadRecordingStorageDevicesByRecorderId(Guid recorderId);
}
