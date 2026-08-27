using System;
using System.Collections;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public sealed class DatabaseDataReader : DbDataReader, IDisposable
{
	private SqlDataReader _sqlDataReader;

	public override object this[int ordinal] => ((DbDataReader)(object)_sqlDataReader)[ordinal];

	public override object this[string name] => ((DbDataReader)(object)_sqlDataReader)[name];

	public override int Depth => ((DbDataReader)(object)_sqlDataReader).Depth;

	public override int FieldCount => ((DbDataReader)(object)_sqlDataReader).FieldCount;

	public override bool HasRows => ((DbDataReader)(object)_sqlDataReader).HasRows;

	public override bool IsClosed => ((DbDataReader)(object)_sqlDataReader).IsClosed;

	public override int RecordsAffected => ((DbDataReader)(object)_sqlDataReader).RecordsAffected;

	public DatabaseDataReader(SqlDataReader sqlDataReader)
	{
		_sqlDataReader = sqlDataReader;
	}

	public override void Close()
	{
		((DbDataReader)(object)_sqlDataReader).Close();
	}

	public override bool GetBoolean(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetBoolean(ordinal);
	}

	public override byte GetByte(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetByte(ordinal);
	}

	public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
	}

	public override char GetChar(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetChar(ordinal);
	}

	public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
	}

	public override string GetDataTypeName(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetDataTypeName(ordinal);
	}

	public override DateTime GetDateTime(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetDateTime(ordinal);
	}

	public override decimal GetDecimal(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetDecimal(ordinal);
	}

	public override double GetDouble(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetDouble(ordinal);
	}

	public override IEnumerator GetEnumerator()
	{
		throw new NotImplementedException();
	}

	public override Type GetFieldType(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetFieldType(ordinal);
	}

	public override float GetFloat(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetFloat(ordinal);
	}

	public override Guid GetGuid(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetGuid(ordinal);
	}

	public override short GetInt16(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetInt16(ordinal);
	}

	public override int GetInt32(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetInt32(ordinal);
	}

	public override long GetInt64(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetInt64(ordinal);
	}

	public override string GetName(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetName(ordinal);
	}

	public override int GetOrdinal(string name)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetOrdinal(name);
	}

	public override string GetString(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetString(ordinal);
	}

	public override object GetValue(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetValue(ordinal);
	}

	public override int GetValues(object[] values)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetValues(values);
	}

	public override bool IsDBNull(int ordinal)
	{
		return ((DbDataReader)(object)_sqlDataReader).IsDBNull(ordinal);
	}

	public override bool NextResult()
	{
		return ((DbDataReader)(object)_sqlDataReader).NextResult();
	}

	public override bool Read()
	{
		return ((DbDataReader)(object)_sqlDataReader).Read();
	}

	public override Task<bool> ReadAsync(CancellationToken cancellationToken)
	{
		return ((DbDataReader)(object)_sqlDataReader).ReadAsync(cancellationToken);
	}

	public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
	{
		return ((DbDataReader)(object)_sqlDataReader).NextResultAsync(cancellationToken);
	}

	public SqlXml GetSqlXml(int i)
	{
		return _sqlDataReader.GetSqlXml(i);
	}

	public override Stream GetStream(int i)
	{
		return ((DbDataReader)(object)_sqlDataReader).GetStream(i);
	}

	public new async Task<bool> IsDBNullAsync(int i)
	{
		return await ((DbDataReader)(object)_sqlDataReader).IsDBNullAsync(i);
	}

	public new async Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken)
	{
		return await ((DbDataReader)(object)_sqlDataReader).IsDBNullAsync(ordinal, cancellationToken);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _sqlDataReader != null)
		{
			SqlDataReader sqlDataReader = _sqlDataReader;
			_sqlDataReader = null;
			((DbDataReader)(object)sqlDataReader).Dispose();
			GC.SuppressFinalize(this);
		}
	}

	public SqlBinary GetSqlBinary(int i)
	{
		return _sqlDataReader.GetSqlBinary(i);
	}

	public SqlGuid GetSqlGuid(int i)
	{
		return _sqlDataReader.GetSqlGuid(i);
	}

	public new Task<bool> ReadAsync()
	{
		return ((DbDataReader)(object)_sqlDataReader).ReadAsync();
	}

	public new Task<bool> NextResultAsync()
	{
		return ((DbDataReader)(object)_sqlDataReader).NextResultAsync();
	}

	public new void Dispose()
	{
		Dispose(disposing: true);
	}
}
