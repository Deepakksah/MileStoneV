using System;
using System.Security;
using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public sealed class DatabaseConnectionInfoConfigurator : IDatabaseConnectionInfoConfigurator
{
	private readonly SqlConnectionServerType _sqlConnectionServerType;

	private readonly IRegistryService _registryService;

	public DatabaseConnectionInfoConfigurator(SqlConnectionServerType sqlConnectionServerType)
		: this(sqlConnectionServerType, new RegistryService())
	{
	}

	public DatabaseConnectionInfoConfigurator(SqlConnectionServerType sqlConnectionServerType, IRegistryService registryService)
	{
		_sqlConnectionServerType = sqlConnectionServerType;
		_registryService = registryService;
	}

	public void Write(string connectionString)
	{
		if (connectionString == null)
		{
			throw new ArgumentNullException("connectionString");
		}
		try
		{
			_registryService.UpdateRegistryValueIn64View("SOFTWARE\\VideoOS\\Server\\ConnectionString", RegistryHive.LocalMachine, _sqlConnectionServerType.ToString(), connectionString);
			CleanLegacyRegistryConnectionString();
		}
		catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
		{
			throw new ConnectionStorageSetException("Setting connection string failed", ex);
		}
	}

	private void CleanLegacyRegistryConnectionString()
	{
		if (_sqlConnectionServerType.Equals(SqlConnectionServerType.ManagementServer))
		{
			new RegistrySqlConnectionStorage().CleanConnectionString();
		}
	}
}
