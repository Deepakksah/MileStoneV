using System;
using VideoOS.Common.Integration;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server;

public sealed class ServerInfoCollector : IInfoCollector
{
	private Guid _managementServerId;

	private string _managementServerName;

	private bool _isInitialized;

	private void Initialize()
	{
		IdNameDescription idNameDescription = ConfigurationDataManager.FindHomeSiteInfo();
		_managementServerId = idNameDescription.Id;
		if (!string.IsNullOrEmpty(idNameDescription.Name))
		{
			_managementServerName = idNameDescription.Name;
		}
		else
		{
			_managementServerName = Environment.MachineName;
		}
		_isInitialized = true;
	}

	public Guid ManagementServerId()
	{
		if (!_isInitialized)
		{
			Initialize();
		}
		return _managementServerId;
	}

	public string ManagementServerName()
	{
		if (!_isInitialized)
		{
			Initialize();
		}
		return _managementServerName;
	}
}
