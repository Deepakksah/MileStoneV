using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Database;

public sealed class DatabaseConnection : DbConnection, IDisposable
{
	private static readonly (int ErrorCode, int Severity)[] ConnectionErrors = new(int, int)[18]
	{
		(926, 14),
		(4060, 11),
		(40197, 17),
		(40501, 20),
		(40613, 17),
		(49918, 16),
		(49919, 16),
		(49920, 16),
		(4221, 16),
		(615, 21),
		(10054, 20),
		(10054, 16),
		(10053, 20),
		(596, 16),
		(64, 20),
		(233, 20),
		(983, 14),
		(976, 14)
	};

	private static (int ErrorCode, int Severity)[] RetryableQueryErrors = new(int, int)[5]
	{
		(1205, 13),
		(8645, 17),
		(8628, 17),
		(3960, 16),
		(3961, 16)
	};

	public static int MaxRetryAttempts = 9;

	public static int RetryAttemptDelay = 300;

	private SqlConnection _connection;

	private string _connectionString;

	private SqlCredential _credential;

	private DatabaseTransaction _transaction;

	private int _retryCount;

	private ILogger _logger;

	private Stack<bool> _retryBlockOwnsTransaction = new Stack<bool>();

	public override string ConnectionString
	{
		get
		{
			return _connectionString;
		}
		set
		{
			throw new InvalidOperationException("Must set the ConnectionString in the constructor");
		}
	}

	public override string Database => ((DbConnection)(object)_connection).Database;

	public override string DataSource => ((DbConnection)(object)_connection).DataSource;

	public override string ServerVersion => ((DbConnection)(object)GetConnectionEnsureOpen()).ServerVersion;

	public override ConnectionState State => ((DbConnection)(object)_connection).State;

	public SqlCredential Credential => _credential;

	public SqlConnection Connection => GetConnectionEnsureOpen();

	public event SqlInfoMessageEventHandler InfoMessage
	{
		add
		{
			_connection.InfoMessage += value;
		}
		remove
		{
			_connection.InfoMessage -= value;
		}
	}

	public DatabaseConnection(string connectionString, SqlCredential credential, ILogger logger)
	{
		_connectionString = connectionString;
		_credential = credential;
		_logger = logger;
		ResetConnection();
	}

	public DatabaseConnection(string connectionString, ILogger logger)
		: this(connectionString, null, logger)
	{
	}

	private void ResetConnection()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		_connection = new SqlConnection(_connectionString, _credential);
	}

	private SqlConnection GetConnectionEnsureOpen()
	{
		if (!State.HasFlag(ConnectionState.Open))
		{
			Open();
		}
		return _connection;
	}

	private async Task<SqlConnection> GetConnectionEnsureOpenAsync()
	{
		if (!State.HasFlag(ConnectionState.Open))
		{
			await DatabaseHelper.WrapSqlExceptionAsync(((DbConnection)(object)_connection).OpenAsync);
		}
		return _connection;
	}

	public void ClearTransaction()
	{
		_transaction = null;
	}

	private static void CheckNoActiveTransactions()
	{
	}

	private bool ShouldRetry(DatabaseError[] errors)
	{
		if (_retryBlockOwnsTransaction.Count > 1 && !_retryBlockOwnsTransaction.Peek())
		{
			return false;
		}
		bool flag = _retryCount < MaxRetryAttempts && _connection != null;
		if (errors.Any((DatabaseError e) => RetryableQueryErrors.Contains((e.Number, e.Class))))
		{
			if (flag)
			{
				ILogger logger = _logger;
				if (logger != null)
				{
					LoggerExtensions.LogWarning(logger, "Retrying operation after receieving query error {ErrorMessage}", new object[1] { string.Join(",", errors.Select((DatabaseError e) => $"E={e.Number},C={e.Class}\n")) });
				}
				return true;
			}
			ILogger logger2 = _logger;
			if (logger2 != null)
			{
				LoggerExtensions.LogError(logger2, "Unable to execute operation after receiving query error after {RetryCount} retries", new object[1] { _retryCount });
			}
		}
		if (!State.HasFlag(ConnectionState.Open) || errors.Any((DatabaseError e) => ConnectionErrors.Contains((e.Number, e.Class))))
		{
			if (flag)
			{
				try
				{
					SqlConnection connection = _connection;
					ResetConnection();
					((Component)(object)connection).Dispose();
				}
				catch
				{
				}
				ILogger logger3 = _logger;
				if (logger3 != null)
				{
					LoggerExtensions.LogWarning(logger3, "Retrying operation after loosing connection to the database {ErrorMessage}", new object[1] { string.Join(",", errors.Select((DatabaseError e) => $"E={e.Number},C={e.Class}\n")) });
				}
				return true;
			}
			ILogger logger4 = _logger;
			if (logger4 != null)
			{
				LoggerExtensions.LogError(logger4, "Unable to establish connection to the database after {RetryCount} retries.", new object[1] { _retryCount });
			}
		}
		return false;
	}

	private void IncrementRetryLevel()
	{
		_retryBlockOwnsTransaction.Push(_transaction == null);
		if (_retryBlockOwnsTransaction.Count == 1)
		{
			_retryCount = 0;
			ThrowOnTransactionStartedBeforeRetryableAction();
		}
	}

	private void DecrementRetryLevel()
	{
		ThrowOnTransactionNotProperlyEnded();
		_retryBlockOwnsTransaction.Pop();
	}

	private static void ThrowOnTransactionNotProperlyEnded()
	{
	}

	private static void ThrowOnTransactionStartedBeforeRetryableAction()
	{
	}

	public static void ClearAllPools()
	{
		SqlConnection.ClearAllPools();
	}

	public override void ChangeDatabase(string databaseName)
	{
		throw new NotImplementedException("ChangeDatabase");
	}

	protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
	{
		throw new NotImplementedException("BeginDbTransaction");
	}

	protected override DbCommand CreateDbCommand()
	{
		return CreateCommand();
	}

	public override void Open()
	{
		DatabaseHelper.WrapSqlException(((DbConnection)(object)_connection).Open);
	}

	public override void Close()
	{
		if (_connection != null)
		{
			try
			{
				CheckNoActiveTransactions();
			}
			finally
			{
				((DbConnection)(object)_connection).Close();
			}
		}
	}

	private void DisposeConnection()
	{
		if (_connection != null)
		{
			try
			{
				CheckNoActiveTransactions();
			}
			finally
			{
				SqlConnection connection = _connection;
				_connection = null;
				((Component)(object)connection).Dispose();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			DisposeConnection();
			GC.SuppressFinalize(this);
		}
	}

	public override DataTable GetSchema()
	{
		return DatabaseHelper.WrapSqlException((Func<DataTable>)((DbConnection)(object)GetConnectionEnsureOpen()).GetSchema);
	}

	public override DataTable GetSchema(string collectionName)
	{
		return DatabaseHelper.WrapSqlException(() => ((DbConnection)(object)GetConnectionEnsureOpen()).GetSchema(collectionName));
	}

	public new DatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
	{
		_transaction = new DatabaseTransaction(this, DatabaseHelper.WrapSqlException(() => GetConnectionEnsureOpen().BeginTransaction(isolationLevel)));
		return _transaction;
	}

	public new DatabaseTransaction BeginTransaction()
	{
		_transaction = new DatabaseTransaction(this, DatabaseHelper.WrapSqlException(() => GetConnectionEnsureOpen().BeginTransaction()));
		return _transaction;
	}

	public DatabaseCommand CreateCommand(string commandText)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0039: Expected O, but got Unknown
		SqlCommand val = new SqlCommand
		{
			Connection = GetConnectionEnsureOpen(),
			Transaction = ((_transaction == null) ? null : _transaction.Transaction)
		};
		((DbCommand)val).CommandText = commandText;
		return new DatabaseCommand(val);
	}

	public async Task<DatabaseCommand> CreateCommandAsync(string commandText)
	{
		SqlCommand command = new SqlCommand();
		SqlCommand val = command;
		val.Connection = await GetConnectionEnsureOpenAsync();
		command.Transaction = ((_transaction == null) ? null : _transaction.Transaction);
		((DbCommand)(object)command).CommandText = commandText;
		return new DatabaseCommand(command);
	}

	public new DatabaseCommand CreateCommand()
	{
		return CreateCommand(null);
	}

	public async Task<DatabaseCommand> CreateCommandAsync()
	{
		return await CreateCommandAsync(null);
	}

	public void ExecuteRetryable(Action action, CancellationToken cancellationToken)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		IncrementRetryLevel();
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				action();
				DecrementRetryLevel();
				break;
			}
			catch (Exception ex) when (ex is DatabaseException || ex is SqlException)
			{
				DatabaseError[] errors = ((ex is DatabaseException) ? ((DatabaseException)ex).Errors : SqlExceptionExtensions.DatabaseErrors((SqlException)ex));
				if (ShouldRetry(errors))
				{
					Task.Delay(RetryAttemptDelay, cancellationToken).Wait();
					_retryCount++;
					continue;
				}
				DecrementRetryLevel();
				throw;
			}
		}
	}

	public void ExecuteRetryable(Action action)
	{
		ExecuteRetryable(action, CancellationToken.None);
	}

	public T ExecuteRetryable<T>(Func<T> func, CancellationToken cancellationToken)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		IncrementRetryLevel();
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				T result = func();
				DecrementRetryLevel();
				return result;
			}
			catch (Exception ex) when (ex is DatabaseException || ex is SqlException)
			{
				DatabaseError[] errors = ((ex is DatabaseException) ? ((DatabaseException)ex).Errors : SqlExceptionExtensions.DatabaseErrors((SqlException)ex));
				if (ShouldRetry(errors))
				{
					Task.Delay(RetryAttemptDelay, cancellationToken).Wait();
					_retryCount++;
					continue;
				}
				DecrementRetryLevel();
				throw;
			}
		}
	}

	public T ExecuteRetryable<T>(Func<T> func)
	{
		return ExecuteRetryable(func, CancellationToken.None);
	}

	public async Task<T> ExecuteRetryableAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken)
	{
		IncrementRetryLevel();
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				T result = await func();
				DecrementRetryLevel();
				return result;
			}
			catch (Exception ex) when (ex is DatabaseException || ex is SqlException)
			{
				DatabaseError[] errors = ((ex is DatabaseException) ? ((DatabaseException)ex).Errors : SqlExceptionExtensions.DatabaseErrors((SqlException)ex));
				if (!ShouldRetry(errors))
				{
					DecrementRetryLevel();
					throw;
				}
				await Task.Delay(RetryAttemptDelay, cancellationToken);
				_retryCount++;
			}
		}
	}

	public async Task<T> ExecuteRetryableAsync<T>(Func<Task<T>> func)
	{
		return await ExecuteRetryableAsync(func, CancellationToken.None);
	}

	public async Task ExecuteRetryableAsync(Func<Task> action, CancellationToken cancellationToken)
	{
		IncrementRetryLevel();
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				await action();
				DecrementRetryLevel();
				break;
			}
			catch (Exception ex) when (ex is DatabaseException || ex is SqlException)
			{
				DatabaseError[] errors = ((ex is DatabaseException) ? ((DatabaseException)ex).Errors : SqlExceptionExtensions.DatabaseErrors((SqlException)ex));
				if (!ShouldRetry(errors))
				{
					DecrementRetryLevel();
					throw;
				}
				await Task.Delay(RetryAttemptDelay, cancellationToken);
				_retryCount++;
			}
		}
	}

	public async Task ExecuteRetryableAsync(Func<Task> action)
	{
		await ExecuteRetryableAsync(action, CancellationToken.None);
	}
}
