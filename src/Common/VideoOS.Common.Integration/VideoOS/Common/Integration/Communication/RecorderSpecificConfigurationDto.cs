using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class RecorderSpecificConfigurationDto
{
	public SystemConfigurationDto SystemConfigurationDto;

	public KeyValueDto[] AdditionalConfiguration;

	public RecorderSpecificConfigurationDto(SystemConfigurationDto systemConfigurationDto, KeyValueDto[] additionalConfiguration)
	{
		SystemConfigurationDto = systemConfigurationDto;
		AdditionalConfiguration = additionalConfiguration;
	}
}
