using System;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class SecurityConfigurationDto : DataSetWithVersionDto
{
	public SecurityDataSet DataSet
	{
		get
		{
			SecurityDataSet securityDataSet = new SecurityDataSet();
			ReadDataSet(securityDataSet);
			return securityDataSet;
		}
	}

	public SecurityConfigurationDto(SecurityDataSet dataSet)
	{
		WriteDataSet(dataSet);
	}
}
