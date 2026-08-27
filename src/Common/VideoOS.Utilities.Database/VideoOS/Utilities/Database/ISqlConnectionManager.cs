using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Database;

public interface ISqlConnectionManager
{
	string DatabaseName { get; }

	string ServerName { get; }

	ILogger Logger { get; }

	DatabaseConnection GetSqlConnection();

	void ExecuteRetryable(Action<DatabaseConnection> action, CancellationToken cancellationToken);

	void ExecuteRetryable(Action<DatabaseConnection> action);

	T ExecuteRetryable<T>(Func<DatabaseConnection, T> func, CancellationToken cancellationToken);

	T ExecuteRetryable<T>(Func<DatabaseConnection, T> func);

	Task<T> ExecuteRetryableAsync<T>(Func<DatabaseConnection, Task<T>> func, CancellationToken cancellationToken);

	Task<T> ExecuteRetryableAsync<T>(Func<DatabaseConnection, Task<T>> func);

	Task ExecuteRetryableAsync(Func<DatabaseConnection, Task> action, CancellationToken cancellationToken);

	Task ExecuteRetryableAsync(Func<DatabaseConnection, Task> action);

	IDatabaseConnectionInfo GetConnectionInfo();
}
