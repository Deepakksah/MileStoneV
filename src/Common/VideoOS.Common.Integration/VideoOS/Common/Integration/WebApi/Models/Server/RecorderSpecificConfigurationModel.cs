using System;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

[Serializable]
[DataContract]
public class RecorderSpecificConfigurationModel : DataSetWithVersionDto
{
	[DataMember]
	public KeyValueDto[] AdditionalConfiguration { get; set; }

	[DataMember]
	public Guid RecorderId { get; set; }

	public RecorderSpecificConfigurationModel()
	{
	}

	public RecorderSpecificConfigurationModel(ConfigurationDataSet dataSet)
	{
		WriteDataSet(dataSet);
	}

	public ConfigurationDataSet CompileDataSet()
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		ReadDataSet(configurationDataSet);
		return configurationDataSet;
	}
}
