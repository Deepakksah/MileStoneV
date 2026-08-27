using System;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

[Serializable]
[DataContract]
public class UpdateHardwareDriverModel : DataSetWithVersionDto
{
	[DataMember]
	[RequiredNonemptyGuid]
	public Guid Token { get; set; }

	public UpdateHardwareDriverModel()
	{
	}

	public UpdateHardwareDriverModel(ConfigurationDataSet dataSet)
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
