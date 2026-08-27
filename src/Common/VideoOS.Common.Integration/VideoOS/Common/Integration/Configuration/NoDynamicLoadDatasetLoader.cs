using System;

namespace VideoOS.Common.Integration.Configuration;

public sealed class NoDynamicLoadDatasetLoader : IConfigurationDataSetLoader
{
	bool IConfigurationDataSetLoader.IsDynamicLoader => false;

	ConfigurationDataSet IConfigurationDataSetLoader.GetLicenseByLicenseId(Guid licenseId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetLicensesByLicenseIds(Guid[] licenseIds)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHardwareDefinitionByDigest(string digest)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHardwareByRecorderId(Guid recorderId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHardwareByHardwareId(Guid hardwareId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHardwareByHardwareIds(Guid[] hardwareIds)
	{
		throw new NotImplementedException();
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHardwareByHostedHardwareId(int hostedHardwareId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHardwareByDeviceGroupId(Guid deviceGroupId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDevicesByHardwareId(Guid hardwareId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDevicesByRecorderId(Guid recorderId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceByDeviceId(Guid deviceId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDevicesByDeviceIds(Guid[] deviceIds)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceGroupByDeviceGroupId(Guid deviceGroupId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceGroupMembersByDeviceId(Guid deviceId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceRecorderPluginsByDeviceId(Guid deviceId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetDeviceTracksByDeviceIds(Guid[] deviceIds)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHostingServersByIds(int[] ids)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetHostedHardwareByIds(int[] ids)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	ConfigurationDataSet IConfigurationDataSetLoader.GetConfigurationDataSetTables(string[] tableNames)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}

	public ConfigurationDataSet GetRecordingStorageDevicesByRecorderId(Guid recorderId)
	{
		throw new NotSupportedException("ConfigurationDatasetLoader does not support dynamic loading.");
	}
}
