using System;
using System.Configuration;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class DatabaseSettings : IDatabaseSettings
{
	private const string SqlApplicationName = "VideoOS Management Server";

	private static readonly Lazy<ISqlConnectionManager> _lazySqlConnectionManager = new Lazy<ISqlConnectionManager>(CreateSqlConnectionManager);

	public int ConnectionTimeout { get; set; }

	public int RequestTimeout { get; set; }

	public int DeadlockRetries { get; set; }

	public int DeadlockPause { get; set; }

	public TimeSpan ExecutionTimeThreshold { get; set; }

	public GeminiDatabaseLoggingOptions LoggingOptions { get; set; }

	public ISqlConnectionManager GetSqlConnectionManager()
	{
		return _lazySqlConnectionManager.Value;
	}

	private static ISqlConnectionManager CreateSqlConnectionManager()
	{
		ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["ModuleTest"];
		IDatabaseConnectionInfo databaseConnectionInfo = null;
		databaseConnectionInfo = ((connectionStringSettings == null || string.IsNullOrEmpty(connectionStringSettings.ConnectionString)) ? new DatabaseConnectionInfoFromStorage(SqlConnectionServerType.ManagementServer, "VideoOS Management Server") : new DatabaseConnectionInfo(connectionStringSettings.ConnectionString, "VideoOS Management Server"));
		return new SqlConnectionManager(databaseConnectionInfo, null);
	}
}
