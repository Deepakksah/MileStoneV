using System;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Providers;

public class DatabaseVersionProvider : IDatabaseVersionProvider
{
	private readonly ILogger _logger;

	private readonly string _dbSchema;

	public bool IsMigrationVersionProvider => false;

	public DatabaseVersionProvider(ILogger logger, string dbSchema)
	{
		_logger = logger;
		_dbSchema = dbSchema;
	}

	public Version GetVersion(DatabaseConnection connection)
	{
		Version version = null;
		string text = "SELECT [version] FROM [" + _dbSchema + "].[System]";
		try
		{
			using DatabaseCommand databaseCommand = connection.CreateCommand();
			databaseCommand.CommandText = text;
			version = new Version((string)databaseCommand.ExecuteScalar());
		}
		catch (DatabaseException ex)
		{
			LoggerExtensions.LogDebug(_logger, "Could not retrieve the database version at this time. Query: {Query}", new object[1] { text });
			LoggerExtensions.LogDebug(_logger, "Current database version ({Schema}): {DatabaseVersion}", new object[2] { _dbSchema, version });
			if (!ex.IsObjectNotFoundException)
			{
				LoggerExtensions.LogWarning(_logger, "{Exception}", new object[1] { ex.Message });
			}
		}
		return version;
	}

	public void SetVersion(DatabaseConnection connection, Version version)
	{
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		string commandText = $"UPDATE [{_dbSchema}].[System] SET [version] = '{version}'";
		if (GetVersion(connection) == null)
		{
			CreateSystemTable(connection);
			commandText = $"INSERT INTO [{_dbSchema}].[System] (version) VALUES ('{version}')";
		}
		using (DatabaseCommand databaseCommand = connection.CreateCommand())
		{
			databaseCommand.CommandText = commandText;
			databaseCommand.ExecuteNonQuery();
		}
		LoggerExtensions.LogDebug(_logger, "System table for schema '{Schema}' set to version: {DatabaseVersion}", new object[2] { _dbSchema, version });
	}

	private void CreateSystemTable(DatabaseConnection connection)
	{
		string commandText = "CREATE TABLE [" + _dbSchema + "].[System] (version varchar(30) NOT NULL)";
		using (DatabaseCommand databaseCommand = connection.CreateCommand())
		{
			databaseCommand.CommandText = commandText;
			databaseCommand.ExecuteNonQuery();
		}
		LoggerExtensions.LogDebug(_logger, "'System' table created for schema '{Schema}'.", new object[1] { _dbSchema });
	}
}
