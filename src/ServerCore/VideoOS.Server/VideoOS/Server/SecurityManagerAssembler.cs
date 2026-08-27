using System;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Server;

public static class SecurityManagerAssembler
{
	public static SecurityConfigurationDto CreateDataSetWithVersionDto(Guid recorderId, Server server)
	{
		SecurityDataSet dataSet = server.Data.LoadSecurityDataSetByRecorder(recorderId);
		return new SecurityConfigurationDto(dataSet);
	}

	public static SecurityManagerDataTransferObject CreateDtoByRole(Guid roleId, Server server)
	{
		SecurityDataSet ds = server.Data.LoadSecurityDataSetByRole(roleId);
		return new SecurityManagerDataTransferObject(ds);
	}

	public static SecurityManagerDataTransferObject CreateDtoByTabels(string[] tableNames, Server server)
	{
		SecurityDataSet ds = server.Data.LoadSecurityDataSetTables(tableNames);
		return new SecurityManagerDataTransferObject(ds);
	}
}
