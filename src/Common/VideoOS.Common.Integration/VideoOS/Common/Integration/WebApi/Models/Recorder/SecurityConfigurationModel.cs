using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public sealed class SecurityConfigurationModel : DataSetWithVersionDto
{
	[DataMember]
	[Required]
	public CommunicationCallContextDto CallContext { get; set; }

	[DataMember]
	[RequiredNonemptyGuid]
	public Guid Token { get; set; }

	public SecurityConfigurationModel()
	{
	}

	public SecurityConfigurationModel(SecurityDataSet dataSet)
	{
		WriteDataSet(dataSet);
	}

	public SecurityDataSet CompileDataSet()
	{
		SecurityDataSet securityDataSet = new SecurityDataSet();
		ReadDataSet(securityDataSet);
		return securityDataSet;
	}
}
