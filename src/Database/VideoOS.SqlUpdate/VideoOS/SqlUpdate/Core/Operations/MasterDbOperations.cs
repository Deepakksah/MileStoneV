using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Connection;
using VideoOS.SqlUpdate.Core.Statics;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Operations;

public class MasterDbOperations : IMasterDbOperations
{
	private readonly ILogger _logger;

	public MasterDbOperations(ILogger logger)
	{
		_logger = logger;
	}

	private bool DbExists(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException(connectionString);
		}
		string commandText = "IF DB_ID(@dbName) IS NOT NULL SELECT 1 ELSE SELECT 0";
		string value = ConnectionStringHelper.ExtractDbName(connectionString);
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConnectionStringHelper.GetConnectionStringToMasterDb(connectionString), _logger);
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
		databaseCommand.AddParameter(new DatabaseParameter("@dbName", value));
		return (int)databaseCommand.ExecuteScalar() == 1;
	}

	public void CreateDb(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException(connectionString);
		}
		string text = ConnectionStringHelper.ExtractDbName(connectionString);
		if (DbExists(connectionString))
		{
			throw new Exception(Messages.DbAlreadyExists(text));
		}
		string commandText = string.Format(CultureInfo.InvariantCulture, "CREATE DATABASE [{0}] COLLATE SQL_Latin1_General_CP1_CI_AS; ALTER DATABASE [{0}] SET RECOVERY SIMPLE", text);
		using (DatabaseConnection databaseConnection = new DatabaseConnection(ConnectionStringHelper.GetConnectionStringToMasterDb(connectionString), _logger))
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
			databaseCommand.ExecuteNonQuery();
		}
		DatabaseConnection.ClearAllPools();
	}

	public bool DropDb(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException(connectionString);
		}
		string text = ConnectionStringHelper.ExtractDbName(connectionString);
		if (!DbExists(connectionString))
		{
			throw new SqlUpdateException(Messages.DbNotFound(text));
		}
		string commandText = string.Format(CultureInfo.InvariantCulture, "ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", text);
		string commandText2 = string.Format(CultureInfo.InvariantCulture, "DROP DATABASE [{0}];\r\n", text);
		string connectionStringToMasterDb = ConnectionStringHelper.GetConnectionStringToMasterDb(connectionString);
		DatabaseConnection.ClearAllPools();
		using (DatabaseConnection databaseConnection = new DatabaseConnection(connectionStringToMasterDb, _logger))
		{
			using (DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText))
			{
				databaseCommand.ExecuteNonQuery();
			}
			using DatabaseCommand databaseCommand2 = databaseConnection.CreateCommand(commandText2);
			databaseCommand2.ExecuteNonQuery();
		}
		return !DbExists(connectionString);
	}

	public int GetCompatibilityLevel(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException(connectionString);
		}
		using DatabaseConnection databaseConnection = new DatabaseConnection(ConnectionStringHelper.GetConnectionStringToMasterDb(connectionString), _logger);
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
		databaseCommand.CommandText = "SELECT max(compatibility_level) FROM sys.databases";
		return Convert.ToInt32(databaseCommand.ExecuteScalar());
	}
}
