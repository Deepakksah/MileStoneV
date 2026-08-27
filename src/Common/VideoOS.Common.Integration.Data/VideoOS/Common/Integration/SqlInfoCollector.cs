using System;

namespace VideoOS.Common.Integration;

public class SqlInfoCollector : IInfoCollector
{
	private Guid _managementServerId;

	private string _managementServerName;

	public SqlInfoCollector(Guid managementServerId, string managementServerName)
	{
		_managementServerId = managementServerId;
		_managementServerName = managementServerName;
	}

	public Guid ManagementServerId()
	{
		return _managementServerId;
	}

	public string ManagementServerName()
	{
		return _managementServerName;
	}
}
