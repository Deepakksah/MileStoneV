using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Scripts;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Operations;

public class DbOperations : IDbOperations
{
	private readonly ILogger _logger;

	public DbOperations(ILogger logger)
	{
		_logger = logger;
	}

	public void ExecuteScript(ScriptBase script, IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, DatabaseConnection connection, IDatabaseVersionProvider databaseVersionProvider, string dbSchema)
	{
		try
		{
			if (script.SupportsTransactions)
			{
				using (DatabaseTransaction databaseTransaction = connection.BeginTransaction())
				{
					script.Execute(connection, dbSchema, scriptContentReplacements, _logger);
					TryUpdateDbVersion(script, connection, databaseVersionProvider);
					databaseTransaction.Commit();
					return;
				}
			}
			script.Execute(connection, dbSchema, scriptContentReplacements, _logger);
			TryUpdateDbVersion(script, connection, databaseVersionProvider);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "Script {ScriptName} failed to execute with error: {Exception}", new object[2] { script.Name, ex.Message });
			throw;
		}
	}

	private static void TryUpdateDbVersion(ScriptBase script, DatabaseConnection connection, IDatabaseVersionProvider databaseVersionProvider)
	{
		if (script.IsUpgradeScript)
		{
			databaseVersionProvider.SetVersion(connection, script.Version);
		}
	}

	public void EnableSnapshotIsolation(DatabaseConnection connection)
	{
		SqlConnection.ClearAllPools();
		try
		{
			string commandText = "ALTER DATABASE CURRENT SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
			using (DatabaseCommand databaseCommand = connection.CreateCommand(commandText))
			{
				databaseCommand.ExecuteNonQuery();
			}
			string commandText2 = "ALTER DATABASE CURRENT SET ALLOW_SNAPSHOT_ISOLATION ON; ALTER DATABASE CURRENT SET READ_COMMITTED_SNAPSHOT ON";
			using DatabaseCommand databaseCommand2 = connection.CreateCommand(commandText2);
			databaseCommand2.ExecuteNonQuery();
		}
		finally
		{
			string commandText3 = "ALTER DATABASE CURRENT SET MULTI_USER";
			using DatabaseCommand databaseCommand3 = connection.CreateCommand(commandText3);
			databaseCommand3.ExecuteNonQuery();
		}
	}

	public bool IsReadCommittedSnapshotIsolationEnabled(DatabaseConnection connection)
	{
		string commandText = "SELECT is_read_committed_snapshot_on+snapshot_isolation_state from sys.databases where name=DB_NAME()";
		using DatabaseCommand databaseCommand = connection.CreateCommand(commandText);
		return (byte)databaseCommand.ExecuteScalar() == 2;
	}

	public bool DbExists(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException("connectionString");
		}
		using DatabaseConnection databaseConnection = new DatabaseConnection(connectionString, _logger);
		try
		{
			databaseConnection.Open();
			return true;
		}
		catch (DatabaseException)
		{
			SqlConnection.ClearAllPools();
			return false;
		}
	}

	public void SetCompatibilityLevel(DatabaseConnection connection, int compatibilityLevel)
	{
		using DatabaseCommand databaseCommand = connection.CreateCommand();
		databaseCommand.CommandText = $"ALTER DATABASE [{connection.Database}] SET COMPATIBILITY_LEVEL = {compatibilityLevel}";
		databaseCommand.ExecuteNonQuery();
	}
}
