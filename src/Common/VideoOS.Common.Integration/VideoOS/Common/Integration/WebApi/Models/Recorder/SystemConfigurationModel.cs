using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

[Serializable]
[DataContract]
public sealed class SystemConfigurationModel : DataSetWithVersionDto
{
	[DataMember]
	[Required]
	public CommunicationCallContextDto CallContext { get; set; }

	[DataMember]
	[RequiredNonemptyGuid]
	public Guid Token { get; set; }

	public SystemConfigurationModel()
	{
	}

	public SystemConfigurationModel(ConfigurationDataSet dataSet)
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
