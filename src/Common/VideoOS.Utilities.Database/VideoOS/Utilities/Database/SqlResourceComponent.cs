using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient.Server;

namespace VideoOS.Utilities.Database;

public abstract class SqlResourceComponent : IDisposable
{
	protected enum ExecuteType
	{
		ExecuteReader,
		ExecuteNonQuery,
		ExecuteScalar
	}

	private static readonly SqlMetaData[] _metadataIdTable = (SqlMetaData[])(object)new SqlMetaData[1]
	{
		new SqlMetaData("Id", SqlDbType.Char, 36L)
	};

	private static readonly SqlMetaData[] _metadataIdNvarcharTable = (SqlMetaData[])(object)new SqlMetaData[1]
	{
		new SqlMetaData("Id", SqlDbType.NVarChar, -1L)
	};

	private static readonly SqlMetaData[] _metadataIdGuidTable = (SqlMetaData[])(object)new SqlMetaData[1]
	{
		new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
	};

	private static readonly SqlMetaData[] _metadataIdIntTable = (SqlMetaData[])(object)new SqlMetaData[1]
	{
		new SqlMetaData("Id", SqlDbType.Int)
	};

	private static int _sqlCommandTimeout = 0;

	private object _scalarResult;

	private int _rowsAffected;

	public int _lastStatementIndex;

	private StringBuilder _sqlCommandText;

	private DatabaseConnection _databaseConnection;

	private DatabaseCommand _databaseCommand;

	private DatabaseDataReader _dataReader;

	private string _sqlStoredProcedure;

	protected StringBuilder SqlCommandText
	{
		[DebuggerStepThrough]
		get
		{
			return _sqlCommandText;
		}
		[DebuggerStepThrough]
		set
		{
			_sqlCommandText = value;
		}
	}

	protected DatabaseConnection Connection
	{
		[DebuggerStepThrough]
		get
		{
			return _databaseConnection;
		}
		[DebuggerStepThrough]
		set
		{
			_databaseConnection = value;
		}
	}

	protected DatabaseCommand Command
	{
		[DebuggerStepThrough]
		get
		{
			return _databaseCommand;
		}
		[DebuggerStepThrough]
		set
		{
			_databaseCommand = value;
		}
	}

	protected DatabaseDataReader DataReader
	{
		[DebuggerStepThrough]
		get
		{
			return _dataReader;
		}
		[DebuggerStepThrough]
		set
		{
			_dataReader = value;
		}
	}

	protected string StoredProcedureName
	{
		get
		{
			return _sqlStoredProcedure;
		}
		set
		{
			_sqlStoredProcedure = value;
		}
	}

	protected abstract bool IsDebugEnabled { get; }

	protected SqlResourceComponent()
	{
	}

	protected SqlResourceComponent(DatabaseConnection databaseConnection)
	{
		if (databaseConnection == null)
		{
			throw new ArgumentNullException("databaseConnection");
		}
		if (_sqlCommandTimeout == 0)
		{
			_sqlCommandTimeout = 3600;
		}
		_databaseConnection = databaseConnection;
		_databaseCommand = null;
		_dataReader = null;
	}

	protected SqlResourceComponent(ISqlConnectionManager sqlConnectionManager, int deadlockPause, int deadlockRetries)
	{
		if (sqlConnectionManager == null)
		{
			throw new ArgumentNullException("sqlConnectionManager");
		}
		if (_sqlCommandTimeout == 0)
		{
			_sqlCommandTimeout = 3600;
		}
		_databaseConnection = sqlConnectionManager.GetSqlConnection();
		_databaseCommand = null;
		_dataReader = null;
	}

	private void CloseReader()
	{
		if (_dataReader != null)
		{
			_dataReader.Dispose();
			_dataReader = null;
		}
	}

	private void CloseCommand()
	{
		if (_databaseCommand != null)
		{
			_databaseCommand.Dispose();
			_databaseCommand = null;
		}
	}

	private void CloseConnection()
	{
		if (_databaseConnection != null)
		{
			_databaseConnection.Dispose();
			_databaseConnection = null;
		}
	}

	private void PrepareNewCommand()
	{
		PrepareNewCommand(1000);
	}

	private void PrepareNewCommand(int lengthEstimate)
	{
		CloseReader();
		CloseCommand();
		_databaseCommand = _databaseConnection.CreateCommand();
		_databaseCommand.CommandTimeout = _sqlCommandTimeout;
		_sqlCommandText = new StringBuilder(lengthEstimate);
		_lastStatementIndex = -1;
	}

	private void Execute(ExecuteType executeType, CommandBehavior behavior)
	{
		if (_databaseCommand == null)
		{
			throw new NoNullAllowedException("No command prepared");
		}
		CloseReader();
		Command.CommandText = _sqlCommandText.ToString();
		try
		{
			bool flag = true;
			Trace(string.Format(CultureInfo.InvariantCulture, "Starting SQL operation {0}", _databaseCommand.CommandText));
			while (flag)
			{
				switch (executeType)
				{
				case ExecuteType.ExecuteReader:
					_dataReader = _databaseCommand.ExecuteReader(behavior);
					break;
				case ExecuteType.ExecuteNonQuery:
					_rowsAffected = _databaseCommand.ExecuteNonQuery();
					break;
				case ExecuteType.ExecuteScalar:
					_scalarResult = _databaseCommand.ExecuteScalar();
					break;
				}
				flag = false;
			}
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
		finally
		{
			Trace("SQL operation completed");
		}
	}

	private async Task ExecuteAsync(ExecuteType executeType, CommandBehavior behavior)
	{
		if (_databaseCommand == null)
		{
			throw new NoNullAllowedException("No command prepared");
		}
		CloseReader();
		string text = SqlCommandText.ToString();
		Command.CommandText = text;
		try
		{
			bool flag = true;
			Trace(string.Format(CultureInfo.InvariantCulture, "Starting SQL operation {0}", text));
			while (flag)
			{
				switch (executeType)
				{
				case ExecuteType.ExecuteReader:
					_dataReader = await _databaseCommand.ExecuteReaderAsync(behavior);
					break;
				case ExecuteType.ExecuteNonQuery:
					_rowsAffected = await _databaseCommand.ExecuteNonQueryAsync();
					break;
				case ExecuteType.ExecuteScalar:
					_scalarResult = await _databaseCommand.ExecuteScalarAsync();
					break;
				}
				flag = false;
			}
		}
		finally
		{
			Trace("SQL operation completed");
		}
	}

	protected DatabaseCommand PrepareSqlBatch()
	{
		return PrepareSqlBatch(1000);
	}

	protected DatabaseCommand PrepareSqlBatch(int lengthEstimate)
	{
		PrepareNewCommand(System.Math.Min(lengthEstimate, 65536));
		_databaseCommand.CommandType = CommandType.Text;
		return _databaseCommand;
	}

	protected DatabaseCommand PrepareStoredProcedure(string storedProcedure)
	{
		PrepareNewCommand();
		_databaseCommand.CommandType = CommandType.StoredProcedure;
		_sqlStoredProcedure = storedProcedure;
		_sqlCommandText.Append(storedProcedure);
		return _databaseCommand;
	}

	protected void AddStatement(string sqlStatement)
	{
		AddStatement(sqlStatement, 0, allowExecute: true);
	}

	protected int AddStatement(string sqlStatement, int numberOfParameters)
	{
		return AddStatement(sqlStatement, numberOfParameters, allowExecute: true);
	}

	protected int AddStatement(string sqlStatement, int numberOfParameters, bool allowExecute)
	{
		_lastStatementIndex++;
		_sqlCommandText.AppendLine(string.Format(CultureInfo.InvariantCulture, sqlStatement, _lastStatementIndex));
		return _lastStatementIndex;
	}

	protected int ExecuteNonQuery()
	{
		LogSql("ExecuteNonQuery");
		Execute(ExecuteType.ExecuteNonQuery, CommandBehavior.Default);
		return _rowsAffected;
	}

	protected async Task<int> ExecuteNonQueryAsync()
	{
		LogSql("ExecuteNonQueryAsync");
		await ExecuteAsync(ExecuteType.ExecuteNonQuery, CommandBehavior.Default);
		return _rowsAffected;
	}

	protected DatabaseDataReader ExecuteReader()
	{
		LogSql("ExecuteReader");
		Execute(ExecuteType.ExecuteReader, CommandBehavior.Default);
		return _dataReader;
	}

	protected async Task<DatabaseDataReader> ExecuteReaderAsync(CommandBehavior commandBehavior)
	{
		LogSql("ExecuteReaderAsync");
		await ExecuteAsync(ExecuteType.ExecuteReader, commandBehavior);
		return _dataReader;
	}

	protected async Task<DatabaseDataReader> ExecuteReaderAsync()
	{
		LogSql("ExecuteReaderAsync");
		await ExecuteReaderAsync(CommandBehavior.Default);
		return _dataReader;
	}

	protected object ExecuteScalar()
	{
		LogSql("ExecuteScalar");
		Execute(ExecuteType.ExecuteScalar, CommandBehavior.Default);
		return _scalarResult;
	}

	protected async Task<object> ExecuteScalarAsync()
	{
		LogSql("ExecuteScalarAsync");
		await ExecuteAsync(ExecuteType.ExecuteScalar, CommandBehavior.Default);
		return _scalarResult;
	}

	protected void FlushBatch()
	{
		ExecuteNonQuery();
		PrepareSqlBatch(_sqlCommandText.Capacity);
	}

	protected abstract void MapException(DatabaseException ex);

	private void LogSql(string method)
	{
		try
		{
			if (!IsDebugEnabled)
			{
				return;
			}
			string text = string.Format(CultureInfo.InvariantCulture, "{0}:\r\n{1}", method, _sqlCommandText);
			if (_databaseCommand.Parameters.Count() > 0)
			{
				text += "\tPARAMETERS {";
				foreach (DatabaseParameter parameter in _databaseCommand.Parameters)
				{
					text += string.Format(CultureInfo.InvariantCulture, "{0}={1};", parameter.ParameterName, parameter.Value);
				}
				text += "}\r\n";
			}
			LogDebug(text);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	protected abstract void AddLogEntryDatabaseConnectionLost();

	protected abstract void LogDebug(string entry);

	protected abstract void LogWarning(string entryFormat, params object[] args);

	protected abstract void Trace(string entry);

	protected abstract void Trace(Exception ex);

	protected DatabaseParameter BindNullValue(string parameterName, SqlDbType type)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, DBNull.Value);
		databaseParameter.IsNullable = true;
		databaseParameter.SqlDbType = type;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindBoolean(string parameterName, bool parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.Bit;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindBoolean(string parameterName, bool? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.Bit);
		}
		return BindBoolean(parameterName, parameterValue.Value);
	}

	protected DatabaseParameter BindByte(string parameterName, byte? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.Bit);
		}
		return BindByte(parameterName, parameterValue.Value);
	}

	protected DatabaseParameter BindGuid(string parameterName, Guid parameterValue)
	{
		return Command.AddParameter(parameterName, SqlDbType.UniqueIdentifier, parameterValue);
	}

	protected DatabaseParameter BindReal(string parameterName, float parameterValue)
	{
		return Command.AddParameter(parameterName, SqlDbType.Real, parameterValue);
	}

	protected DatabaseParameter BindReal(string parameterName, float? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.Bit);
		}
		return BindReal(parameterName, parameterValue.Value);
	}

	protected DatabaseParameter BindUniqueIdentifier(string parameterName, string parameterValue)
	{
		return Command.AddParameter(parameterName, SqlDbType.UniqueIdentifier, new Guid(parameterValue));
	}

	protected DatabaseParameter BindVarBinaryMax(string parameterName, byte[] parameterValue)
	{
		return Command.AddParameter(parameterName, SqlDbType.VarBinary, -1, parameterValue);
	}

	protected DatabaseParameter BindReturnValue()
	{
		DatabaseParameter databaseParameter = new DatabaseParameter();
		databaseParameter.ParameterName = "@ReturnVal";
		databaseParameter.SqlDbType = SqlDbType.Int;
		databaseParameter.Direction = ParameterDirection.ReturnValue;
		Command.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindByte(string parameterName, byte parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.TinyInt;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindByteArray(string parameterName, byte[] parameterValue, bool allowNull)
	{
		if (allowNull && parameterValue == null)
		{
			return BindNullValue(parameterName, SqlDbType.VarBinary);
		}
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.VarBinary;
		Command.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindDateTime(string parameterName, DateTime parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.DateTime;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindDateTime(string parameterName, DateTime? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.DateTime);
		}
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.DateTime;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindDateTime2(string parameterName, DateTime parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.DateTime2;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindDateTime2(string parameterName, DateTime? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.DateTime2);
		}
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.DateTime2;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindInt(string parameterName, int parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.Int;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindInt(string parameterName, int? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.Int);
		}
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.Int;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindShort(string parameterName, short parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.SmallInt;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindShort(string parameterName, short? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.Bit);
		}
		return BindShort(parameterName, parameterValue.Value);
	}

	protected DatabaseParameter BindLong(string parameterName, long parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.BigInt;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindLong(string parameterName, long? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.Bit);
		}
		return BindLong(parameterName, parameterValue.Value);
	}

	protected DatabaseParameter BindFloat(string parameterName, double? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.Bit);
		}
		return BindFloat(parameterName, parameterValue.Value);
	}

	protected DatabaseParameter BindFloat(string parameterName, double parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.Float;
		Command.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindUniqueIdentifier(string parameterName, Guid parameterValue)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindUniqueIdentifier(string parameterName, Guid? parameterValue)
	{
		if (!parameterValue.HasValue)
		{
			return BindNullValue(parameterName, SqlDbType.UniqueIdentifier);
		}
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindText(string parameterName, string parameterValue, bool allowNull)
	{
		return BindString(parameterName, parameterValue, 1073741823, allowNull, SqlDbType.NText);
	}

	protected DatabaseParameter BindXml(string parameterName, string parameterValue, bool allowNull)
	{
		if (parameterValue != null && parameterValue.Length > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("parameterValue", "The length of the xml string cannot be longer than Int32.MaxValue.");
		}
		return BindString(parameterName, parameterValue, int.MaxValue, allowNull, SqlDbType.Xml);
	}

	protected DatabaseParameter BindXml(string parameterName, string parameterValue)
	{
		return Command.AddParameter(parameterName, SqlDbType.Xml, parameterValue);
	}

	protected DatabaseParameter BindStringMax(string parameterName, string parameterValue)
	{
		return BindString(parameterName, parameterValue, int.MaxValue, allowNull: false, SqlDbType.NVarChar);
	}

	protected DatabaseParameter BindStringMax(string parameterName, string parameterValue, bool allowNull)
	{
		return BindString(parameterName, parameterValue, int.MaxValue, allowNull, SqlDbType.NVarChar);
	}

	protected DatabaseParameter BindString(string parameterName, string parameterValue, int maxLength, bool allowNull, SqlDbType type)
	{
		if (allowNull)
		{
			if (parameterValue == null)
			{
				return BindNullValue(parameterName, type);
			}
		}
		else if (parameterValue == null)
		{
			parameterValue = string.Empty;
		}
		if (maxLength > 0 && parameterValue.Length > maxLength)
		{
			parameterValue = parameterValue.Substring(0, maxLength);
		}
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, parameterValue);
		databaseParameter.SqlDbType = type;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected DatabaseParameter BindTable(string parameterName, string typeName, IEnumerable<SqlDataRecord> rows, bool convertEmptyToNull)
	{
		if (convertEmptyToNull && !rows.Any())
		{
			rows = null;
		}
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, rows);
		databaseParameter.TypeName = typeName;
		databaseParameter.SqlDbType = SqlDbType.Structured;
		_databaseCommand.AddParameter(databaseParameter);
		return databaseParameter;
	}

	protected void BindGuidType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 36, allowNull: false, SqlDbType.Char);
	}

	protected void BindGuidType(string parameterName, Guid parameterValue)
	{
		BindString(parameterName, parameterValue.ToString(), 36, allowNull: false, SqlDbType.Char);
	}

	protected void BindGuidType(string parameterName, Guid? parameterValue, bool allowNullOrEmpty)
	{
		BindString(parameterName, parameterValue.HasValue ? parameterValue.ToString() : null, 36, allowNullOrEmpty, SqlDbType.Char);
	}

	protected void BindGuidType(string parameterName, string parameterValue, bool allowNullOrEmpty)
	{
		BindString(parameterName, parameterValue, 36, allowNullOrEmpty, SqlDbType.Char);
	}

	protected void BindLogIDType(string parameterName, int paramenterValue)
	{
		BindInt(parameterName, paramenterValue);
	}

	protected void BindUserIdType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 256, allowNull: false, SqlDbType.NVarChar);
	}

	protected void BindPasswordType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 256, allowNull: false, SqlDbType.NVarChar);
	}

	protected void BindPasswordType(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 256, allowNull, SqlDbType.NVarChar);
	}

	protected void BindNameType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 512, allowNull: false, SqlDbType.NVarChar);
	}

	protected void BindNameType(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 512, allowNull, SqlDbType.NVarChar);
	}

	protected void BindShortNameType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 128, allowNull: false, SqlDbType.NVarChar);
	}

	protected void BindLongNameType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 1024, allowNull: false, SqlDbType.NVarChar);
	}

	protected void BindEnumType(string parameterName, Type enumType, Enum parameterValue)
	{
		BindString(parameterName, Enum.GetName(enumType, parameterValue), 128, allowNull: false, SqlDbType.VarChar);
	}

	protected void BindDescription(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 1073741823, allowNull: false, SqlDbType.NVarChar);
	}

	protected void BindDescription(string parameterName, string parameterValue, bool allowNull)
	{
		BindString(parameterName, parameterValue, 1073741823, allowNull, SqlDbType.NText);
	}

	protected void BindUriType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 1073741823, allowNull: true, SqlDbType.NVarChar);
	}

	protected void BindSIDType(string parameterName, string parameterValue)
	{
		BindString(parameterName, parameterValue, 256, allowNull: false, SqlDbType.VarChar);
	}

	protected DatabaseParameter BindIdTable(string parameterName, IEnumerable<string> idArray)
	{
		idArray = idArray ?? Enumerable.Empty<string>();
		Func<string, SqlDataRecord> selector = (string ids) => BindTableRow(ids, _metadataIdTable);
		return BindTable(parameterName, "tvp_StringTableGuidType", idArray.Select(selector).ToList(), convertEmptyToNull: true);
	}

	protected DatabaseParameter BindIdTable(string parameterName, IEnumerable<Guid> idArray)
	{
		idArray = idArray ?? Enumerable.Empty<Guid>();
		Func<Guid, SqlDataRecord> selector = (Guid ids) => BindTableRow(ids, _metadataIdGuidTable);
		return BindTable(parameterName, "tvp_IdTableUniqueIdentifier", idArray.Select(selector).ToList(), convertEmptyToNull: true);
	}

	protected DatabaseParameter BindIdTable(string parameterName, IEnumerable<int> idArray)
	{
		idArray = idArray ?? Enumerable.Empty<int>();
		return BindTable(parameterName, "tvp_IdTableInt", idArray.Select((int id) => BindTableRow(id, _metadataIdIntTable)), convertEmptyToNull: true);
	}

	protected DatabaseParameter BindResultTable(string parameterName, IEnumerable<string> idArray)
	{
		idArray = idArray ?? Enumerable.Empty<string>();
		Func<string, SqlDataRecord> selector = (string ids) => BindTableRow(ids, _metadataIdNvarcharTable);
		return BindTable(parameterName, "tvp_StringTableNvarchar", idArray.Select(selector).ToList(), convertEmptyToNull: true);
	}

	protected DatabaseParameter BindDataTable(string parameterName, DataTable dataTable)
	{
		DatabaseParameter databaseParameter = new DatabaseParameter(parameterName, dataTable);
		databaseParameter.SqlDbType = SqlDbType.Structured;
		Command.AddParameter(databaseParameter);
		return databaseParameter;
	}

	private static SqlDataRecord BindTableRow(string value, SqlMetaData[] type)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		SqlDataRecord val = new SqlDataRecord(type);
		val.SetString(0, value);
		return val;
	}

	private static SqlDataRecord BindTableRow(Guid value, SqlMetaData[] type)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		SqlDataRecord val = new SqlDataRecord(type);
		val.SetGuid(0, value);
		return val;
	}

	private static SqlDataRecord BindTableRow(int value, SqlMetaData[] type)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		SqlDataRecord val = new SqlDataRecord(type);
		val.SetInt32(0, value);
		return val;
	}

	protected DatabaseParameter BindUniqueIdentifiers(string parameterName, IEnumerable<Guid> parameterValues)
	{
		if (parameterValues == null)
		{
			throw new ArgumentNullException("parameterValues");
		}
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("Id", typeof(Guid));
		foreach (Guid parameterValue in parameterValues)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow["Id"] = parameterValue;
			dataTable.Rows.Add(dataRow);
		}
		return BindDataTable(parameterName, dataTable);
	}

	protected string FormatDateTime(DateTime dateTime)
	{
		return dateTime.ToString("yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
	}

	public virtual void Dispose()
	{
		CloseReader();
		CloseCommand();
		CloseConnection();
	}
}
