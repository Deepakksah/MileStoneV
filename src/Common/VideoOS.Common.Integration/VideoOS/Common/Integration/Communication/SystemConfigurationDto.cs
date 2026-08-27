using System;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[DataContract]
public sealed class SystemConfigurationDto : DataSetWithVersionDto
{
	public ConfigurationDataSet DataSet
	{
		get
		{
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
			ReadDataSet(configurationDataSet);
			return configurationDataSet;
		}
	}

	public SystemConfigurationDto(ConfigurationDataSet dataSet)
	{
		WriteDataSet(dataSet);
	}
}
