using System;
using System.Security;
using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public sealed class DatabaseConnectionInfoFromStorage : DatabaseConnectionInfo
{
	private readonly SqlConnectionServerType _sqlConnectionServerType;

	private readonly IRegistryService _registryService;

	public override string ConnectionString
	{
		get
		{
			if (_connectionString == null)
			{
				try
				{
					_connectionString = _registryService.GetRegistryValueFrom64View("SOFTWARE\\VideoOS\\Server\\ConnectionString", RegistryHive.LocalMachine, _sqlConnectionServerType.ToString());
				}
				catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
				{
					throw new ConnectionStorageReadException("Reading connection string failed", ex);
				}
			}
			return _connectionString;
		}
	}

	public DatabaseConnectionInfoFromStorage(SqlConnectionServerType sqlConnectionServerType)
		: this(sqlConnectionServerType, new RegistryService(), null)
	{
	}

	public DatabaseConnectionInfoFromStorage(SqlConnectionServerType sqlConnectionServerType, string applicationName)
		: this(sqlConnectionServerType, new RegistryService(), applicationName)
	{
	}

	public DatabaseConnectionInfoFromStorage(SqlConnectionServerType sqlConnectionServerType, IRegistryService registryService)
		: this(sqlConnectionServerType, registryService, null)
	{
	}

	public DatabaseConnectionInfoFromStorage(SqlConnectionServerType sqlConnectionServerType, IRegistryService registryService, string applicationName)
	{
		_sqlConnectionServerType = sqlConnectionServerType;
		_registryService = registryService;
		_applicationName = applicationName;
	}
}
