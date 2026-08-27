using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public sealed class DatabaseCommand : DbCommand, IDisposable
{
	private SqlCommand _sqlCommand;

	public override string CommandText
	{
		get
		{
			return ((DbCommand)(object)_sqlCommand).CommandText;
		}
		set
		{
			((DbCommand)(object)_sqlCommand).CommandText = value;
		}
	}

	public override int CommandTimeout
	{
		get
		{
			return ((DbCommand)(object)_sqlCommand).CommandTimeout;
		}
		set
		{
			((DbCommand)(object)_sqlCommand).CommandTimeout = value;
		}
	}

	public override CommandType CommandType
	{
		get
		{
			return ((DbCommand)(object)_sqlCommand).CommandType;
		}
		set
		{
			((DbCommand)(object)_sqlCommand).CommandType = value;
		}
	}

	public override bool DesignTimeVisible
	{
		get
		{
			return ((DbCommand)(object)_sqlCommand).DesignTimeVisible;
		}
		set
		{
			((DbCommand)(object)_sqlCommand).DesignTimeVisible = value;
		}
	}

	public override UpdateRowSource UpdatedRowSource
	{
		get
		{
			return ((DbCommand)(object)_sqlCommand).UpdatedRowSource;
		}
		set
		{
			((DbCommand)(object)_sqlCommand).UpdatedRowSource = value;
		}
	}

	protected override DbConnection DbConnection
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	protected override DbParameterCollection DbParameterCollection
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	protected override DbTransaction DbTransaction
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public new IEnumerable<DatabaseParameter> Parameters
	{
		get
		{
			IEnumerator sqlParametersEnumerator = ((DbParameterCollection)(object)_sqlCommand.Parameters).GetEnumerator();
			while (sqlParametersEnumerator.MoveNext())
			{
				yield return new DatabaseParameter((SqlParameter)sqlParametersEnumerator.Current);
			}
		}
	}

	public SqlCommand Command => _sqlCommand;

	public new DatabaseConnection Connection
	{
		set
		{
			_sqlCommand.Connection = value.Connection;
		}
	}

	public DatabaseCommand(SqlCommand sqlCommand)
	{
		_sqlCommand = sqlCommand;
	}

	public DatabaseCommand()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		_sqlCommand = new SqlCommand();
	}

	public override void Cancel()
	{
		DatabaseHelper.WrapSqlException(((DbCommand)(object)_sqlCommand).Cancel);
	}

	public override int ExecuteNonQuery()
	{
		return DatabaseHelper.WrapSqlException((Func<int>)((DbCommand)(object)_sqlCommand).ExecuteNonQuery);
	}

	public override object ExecuteScalar()
	{
		return DatabaseHelper.WrapSqlException((Func<object>)((DbCommand)(object)_sqlCommand).ExecuteScalar);
	}

	public override async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => await ((DbCommand)(object)_sqlCommand).ExecuteNonQueryAsync(cancellationToken));
	}

	public override void Prepare()
	{
		DatabaseHelper.WrapSqlException(((DbCommand)(object)_sqlCommand).Prepare);
	}

	protected override DbParameter CreateDbParameter()
	{
		throw new NotImplementedException();
	}

	protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
	{
		throw new NotImplementedException();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _sqlCommand != null)
		{
			SqlCommand sqlCommand = _sqlCommand;
			_sqlCommand = null;
			((Component)(object)sqlCommand).Dispose();
			GC.SuppressFinalize(this);
		}
	}

	public new void Dispose()
	{
		Dispose(disposing: true);
	}

	public new DatabaseDataReader ExecuteReader(CommandBehavior behavior)
	{
		return DatabaseHelper.WrapSqlException(() => new DatabaseDataReader(_sqlCommand.ExecuteReader(behavior)));
	}

	public new DatabaseDataReader ExecuteReader()
	{
		return DatabaseHelper.WrapSqlException(() => new DatabaseDataReader(_sqlCommand.ExecuteReader()));
	}

	public new async Task<object> ExecuteScalarAsync()
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => await ((DbCommand)(object)_sqlCommand).ExecuteScalarAsync());
	}

	public new async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => await ((DbCommand)(object)_sqlCommand).ExecuteScalarAsync(cancellationToken));
	}

	public new async Task<DatabaseDataReader> ExecuteReaderAsync()
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => new DatabaseDataReader(await _sqlCommand.ExecuteReaderAsync()));
	}

	public new async Task<DatabaseDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => new DatabaseDataReader(await _sqlCommand.ExecuteReaderAsync(cancellationToken)));
	}

	public new async Task<DatabaseDataReader> ExecuteReaderAsync(CommandBehavior commandBehavior, CancellationToken cancellationToken)
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => new DatabaseDataReader(await _sqlCommand.ExecuteReaderAsync(commandBehavior, cancellationToken)));
	}

	public new async Task<DatabaseDataReader> ExecuteReaderAsync(CommandBehavior commandBehavior)
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => new DatabaseDataReader(await _sqlCommand.ExecuteReaderAsync(commandBehavior)));
	}

	public new async Task<int> ExecuteNonQueryAsync()
	{
		return await DatabaseHelper.WrapSqlExceptionAsync(async () => await ((DbCommand)(object)_sqlCommand).ExecuteNonQueryAsync());
	}

	public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject)
	{
		return DatabaseHelper.WrapSqlException(() => _sqlCommand.BeginExecuteReader(callback, stateObject));
	}

	public DatabaseDataReader EndExecuteReader(IAsyncResult asyncResult)
	{
		return DatabaseHelper.WrapSqlException(() => new DatabaseDataReader(_sqlCommand.EndExecuteReader(asyncResult)));
	}

	public new DatabaseParameter CreateParameter()
	{
		return new DatabaseParameter();
	}

	public void AddParameter(DatabaseParameter parameter)
	{
		_sqlCommand.Parameters.Add(parameter.Parameter);
	}

	public DatabaseParameter AddParameter(string parameterName, SqlDbType sqlDbType, int size, object value)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, value);
		databaseParameter.SqlDbType = sqlDbType;
		databaseParameter.Size = size;
		AddParameter(databaseParameter);
		return databaseParameter;
	}

	public DatabaseParameter AddParameter(string parameterName, SqlDbType sqlDbType, object value)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, value);
		databaseParameter.SqlDbType = sqlDbType;
		AddParameter(databaseParameter);
		return databaseParameter;
	}

	public void AddParameters(DatabaseParameter[] parameters)
	{
		_sqlCommand.Parameters.AddRange(parameters.Select((DatabaseParameter p) => p.Parameter).ToArray());
	}
}
