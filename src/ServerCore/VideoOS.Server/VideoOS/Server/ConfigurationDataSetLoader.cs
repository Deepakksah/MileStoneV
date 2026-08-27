using System;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server;

public class ConfigurationDataSetLoader : IConfigurationDataSetLoader
{
	private IConfigurationDataLoader configurationDataLoader;

	public bool IsDynamicLoader => true;

	public ConfigurationDataSetLoader(IConfigurationDataLoader loader)
	{
		configurationDataLoader = loader;
	}

	public ConfigurationDataSet GetLicenseByLicenseId(Guid licenseId)
	{
		return configurationDataLoader.LoadLicenseByLicenseId(licenseId);
	}

	public ConfigurationDataSet GetLicensesByLicenseIds(Guid[] licenseIds)
	{
		return configurationDataLoader.LoadLicensesByLicenseIds(licenseIds);
	}

	public ConfigurationDataSet GetHardwareDefinitionByDigest(string digest)
	{
		return configurationDataLoader.LoadHardwareDefinitionByDigest(digest);
	}

	public ConfigurationDataSet GetHardwareByRecorderId(Guid recorderId)
	{
		return configurationDataLoader.LoadHardwareByRecorderId(recorderId);
	}

	public ConfigurationDataSet GetHardwareByHardwareId(Guid hardwareId)
	{
		return configurationDataLoader.LoadHardwareByHardwareId(hardwareId);
	}

	public ConfigurationDataSet GetHardwareByHardwareIds(Guid[] hardwareIds)
	{
		return configurationDataLoader.LoadHardwareByHardwareIds(hardwareIds);
	}

	public ConfigurationDataSet GetHardwareByHostedHardwareId(int hostedHardwareId)
	{
		return configurationDataLoader.LoadHardwareByHostedHardwareId(hostedHardwareId);
	}

	public ConfigurationDataSet GetHardwareByDeviceGroupId(Guid deviceGroupId)
	{
		return configurationDataLoader.LoadHardwareByDeviceGroupId(deviceGroupId);
	}

	public ConfigurationDataSet GetDevicesByHardwareId(Guid hardwareId)
	{
		return configurationDataLoader.LoadDevicesByHardwareId(hardwareId);
	}

	public ConfigurationDataSet GetDevicesByRecorderId(Guid recorderId)
	{
		return configurationDataLoader.LoadDevicesByRecorderId(recorderId);
	}

	public ConfigurationDataSet GetDeviceByDeviceId(Guid deviceId)
	{
		return configurationDataLoader.LoadDeviceByDeviceId(deviceId);
	}

	public ConfigurationDataSet GetDevicesByDeviceIds(Guid[] deviceIds)
	{
		return configurationDataLoader.LoadDevicesByDeviceIds(deviceIds);
	}

	public ConfigurationDataSet GetDeviceGroupByDeviceGroupId(Guid deviceGroupId)
	{
		return configurationDataLoader.LoadDeviceGroupByDeviceGroupId(deviceGroupId);
	}

	public ConfigurationDataSet GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId)
	{
		return configurationDataLoader.LoadDeviceGroupsByParentDeviceGroupId(parentDeviceGroupId);
	}

	public ConfigurationDataSet GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId)
	{
		return configurationDataLoader.LoadDeviceGroupMembersByDeviceGroupId(deviceGroupId);
	}

	public ConfigurationDataSet GetDeviceGroupMembersByDeviceId(Guid deviceId)
	{
		return configurationDataLoader.LoadDeviceGroupMembersByDeviceId(deviceId);
	}

	public ConfigurationDataSet GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId)
	{
		return configurationDataLoader.LoadDeviceRecorderPlugin(recorderPluginID, deviceId);
	}

	public ConfigurationDataSet GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds)
	{
		return configurationDataLoader.LoadDeviceRecorderPlugins(recorderPluginID, deviceIds);
	}

	public ConfigurationDataSet GetDeviceRecorderPluginsByDeviceId(Guid deviceId)
	{
		return configurationDataLoader.LoadDeviceRecorderPluginsByDeviceId(deviceId);
	}

	public ConfigurationDataSet GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds)
	{
		return configurationDataLoader.LoadDevicePrivacyMaskByDeviceIds(deviceIds);
	}

	public ConfigurationDataSet GetDeviceTracksByDeviceIds(Guid[] deviceIds)
	{
		return configurationDataLoader.LoadDeviceTracksByDeviceIds(deviceIds);
	}

	public ConfigurationDataSet GetConfigurationDataSetTables(string[] tableNames)
	{
		return configurationDataLoader.LoadConfigurationDataSetTables(tableNames);
	}

	public ConfigurationDataSet GetHostingServersByIds(int[] ids)
	{
		return configurationDataLoader.LoadHostingServersByIds(ids);
	}

	public ConfigurationDataSet GetHostedHardwareByIds(int[] ids)
	{
		return configurationDataLoader.LoadHostedHardwareByIds(ids);
	}

	public ConfigurationDataSet GetRecordingStorageDevicesByRecorderId(Guid recorderId)
	{
		return configurationDataLoader.LoadRecordingStorageDevicesByRecorderId(recorderId);
	}
}
