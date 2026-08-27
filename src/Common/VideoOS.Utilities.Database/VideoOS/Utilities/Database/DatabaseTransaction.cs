using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public sealed class DatabaseTransaction : DbTransaction, IDisposable
{
	private readonly DatabaseConnection _connection;

	private SqlTransaction _transaction;

	public override IsolationLevel IsolationLevel => ((DbTransaction)(object)_transaction).IsolationLevel;

	protected override DbConnection DbConnection => (DbConnection)(object)_transaction.Connection;

	public SqlTransaction Transaction => _transaction;

	public DatabaseTransaction(DatabaseConnection connection, SqlTransaction transaction)
	{
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		if (transaction == null)
		{
			throw new ArgumentNullException("transaction");
		}
		_connection = connection;
		_transaction = transaction;
	}

	public override void Commit()
	{
		try
		{
			DatabaseHelper.WrapSqlException(((DbTransaction)(object)_transaction).Commit);
		}
		finally
		{
			_connection.ClearTransaction();
		}
	}

	public override void Rollback()
	{
		try
		{
			DatabaseHelper.WrapSqlException(((DbTransaction)(object)_transaction).Rollback);
		}
		finally
		{
			_connection.ClearTransaction();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _transaction != null)
		{
			SqlTransaction transaction = _transaction;
			_connection.ClearTransaction();
			_transaction = null;
			((DbTransaction)(object)transaction).Dispose();
			GC.SuppressFinalize(this);
		}
		base.Dispose(disposing);
	}
}
