using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Database;

public sealed class SqlConnectionManager : ISqlConnectionManager
{
	private readonly IDatabaseConnectionInfo _databaseConnectionInfo;

	public string DatabaseName { get; }

	public string ServerName { get; }

	public ILogger Logger { get; }

	public SqlConnectionManager(IDatabaseConnectionInfo databaseConnectionInfo, ILogger logger)
	{
		if (databaseConnectionInfo == null)
		{
			throw new ArgumentNullException("databaseConnectionInfo");
		}
		_databaseConnectionInfo = databaseConnectionInfo;
		_databaseConnectionInfo.Validate();
		DatabaseName = _databaseConnectionInfo.DatabaseName;
		ServerName = _databaseConnectionInfo.ServerName;
		Logger = logger;
	}

	public DatabaseConnection GetSqlConnection()
	{
		return new DatabaseConnection(_databaseConnectionInfo.ConnectionString, Logger);
	}

	public void ExecuteRetryable(Action<DatabaseConnection> action, CancellationToken cancellationToken)
	{
		DatabaseConnection connection = GetSqlConnection();
		try
		{
			connection.ExecuteRetryable(delegate
			{
				action(connection);
			}, cancellationToken);
		}
		finally
		{
			if (connection != null)
			{
				((IDisposable)connection).Dispose();
			}
		}
	}

	public void ExecuteRetryable(Action<DatabaseConnection> action)
	{
		ExecuteRetryable(action, CancellationToken.None);
	}

	public T ExecuteRetryable<T>(Func<DatabaseConnection, T> func, CancellationToken cancellationToken)
	{
		DatabaseConnection connection = GetSqlConnection();
		try
		{
			return connection.ExecuteRetryable(() => func(connection), cancellationToken);
		}
		finally
		{
			if (connection != null)
			{
				((IDisposable)connection).Dispose();
			}
		}
	}

	public T ExecuteRetryable<T>(Func<DatabaseConnection, T> func)
	{
		return ExecuteRetryable(func, CancellationToken.None);
	}

	public async Task<T> ExecuteRetryableAsync<T>(Func<DatabaseConnection, Task<T>> func, CancellationToken cancellationToken)
	{
		DatabaseConnection connection = GetSqlConnection();
		try
		{
			return await connection.ExecuteRetryableAsync(async () => await func(connection), cancellationToken);
		}
		finally
		{
			if (connection != null)
			{
				((IDisposable)connection).Dispose();
			}
		}
	}

	public async Task<T> ExecuteRetryableAsync<T>(Func<DatabaseConnection, Task<T>> func)
	{
		return await ExecuteRetryableAsync(func, CancellationToken.None);
	}

	public async Task ExecuteRetryableAsync(Func<DatabaseConnection, Task> action, CancellationToken cancellationToken)
	{
		DatabaseConnection connection = GetSqlConnection();
		try
		{
			await connection.ExecuteRetryableAsync(async delegate
			{
				await action(connection);
			}, cancellationToken);
		}
		finally
		{
			if (connection != null)
			{
				((IDisposable)connection).Dispose();
			}
		}
	}

	public async Task ExecuteRetryableAsync(Func<DatabaseConnection, Task> action)
	{
		await ExecuteRetryableAsync(action, CancellationToken.None);
	}

	public IDatabaseConnectionInfo GetConnectionInfo()
	{
		return _databaseConnectionInfo;
	}
}
