using System;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server;

public sealed class ConfigurationManagerAssembler
{
	public static SystemConfigurationDto CreateSystemConfigurationWithVersionDto(Guid recorderId, Guid recorderServiceId, Server server)
	{
		ConfigurationDataSet dataSet = server.Data.LoadConfigurationDataSetByRecorder(recorderId, recorderServiceId);
		return new SystemConfigurationDto(dataSet);
	}
}
