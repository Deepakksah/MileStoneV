using System;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.LogServerPlugins;

public class LogServerDatabaseVersionProvider : IDatabaseVersionProvider
{
	private readonly ILogger _logger;

	private Version _currentDatabaseVersion;

	private bool _versionTableExists;

	public bool IsMigrationVersionProvider => false;

	public LogServerDatabaseVersionProvider(ILogger logger)
	{
		_logger = logger;
	}

	public Version GetVersion(DatabaseConnection connection)
	{
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		if (_currentDatabaseVersion != null)
		{
			return _currentDatabaseVersion;
		}
		string text = string.Empty;
		try
		{
			text = "SELECT [Version] FROM [LogServer].[Version]";
			using DatabaseCommand databaseCommand = connection.CreateCommand();
			databaseCommand.CommandText = text;
			_currentDatabaseVersion = new Version((string)databaseCommand.ExecuteScalar());
			_versionTableExists = true;
		}
		catch (DatabaseException ex) when (ex.IsObjectNotFoundException)
		{
			LoggerExtensions.LogDebug(_logger, "Could not retrieve the database version at this time. Query: " + text, Array.Empty<object>());
			LoggerExtensions.LogDebug(_logger, $"Current logserver database version: {_currentDatabaseVersion}", Array.Empty<object>());
		}
		return _currentDatabaseVersion;
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
		string commandText = $"UPDATE [LogServer].[Version] SET [Version] = '{version}'";
		if (!_versionTableExists)
		{
			GetVersion(connection);
			if (!_versionTableExists)
			{
				CreateVersionTable(connection);
				commandText = $"INSERT INTO [LogServer].[Version] (Version) VALUES ('{version}')";
				_versionTableExists = true;
			}
		}
		using (DatabaseCommand databaseCommand = connection.CreateCommand())
		{
			databaseCommand.CommandText = commandText;
			databaseCommand.ExecuteNonQuery();
		}
		_currentDatabaseVersion = version;
		LoggerExtensions.LogDebug(_logger, $"Version table set to version: {version}", Array.Empty<object>());
		LoggerExtensions.LogDebug(_logger, string.Empty, Array.Empty<object>());
	}

	private void CreateVersionTable(DatabaseConnection connection)
	{
		string commandText = "CREATE TABLE [LogServer].[Version] (Version varchar(30) NOT NULL)";
		using (DatabaseCommand databaseCommand = connection.CreateCommand())
		{
			databaseCommand.CommandText = commandText;
			databaseCommand.ExecuteNonQuery();
		}
		LoggerExtensions.LogDebug(_logger, "'Version' table created.", Array.Empty<object>());
		LoggerExtensions.LogDebug(_logger, string.Empty, Array.Empty<object>());
	}
}
