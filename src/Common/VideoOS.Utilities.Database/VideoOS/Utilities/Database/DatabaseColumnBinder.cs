using System;
using System.IO;
using System.Threading.Tasks;

namespace VideoOS.Utilities.Database;

public class DatabaseColumnBinder
{
	protected int Column;

	protected string ColumnName;

	public DatabaseColumnBinder(string columnName)
	{
		Column = -1;
		ColumnName = columnName;
	}

	protected void InitColumn(DatabaseDataReader reader)
	{
		if (Column == -1)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			Column = reader.GetOrdinal(ColumnName);
		}
	}

	public DateTime GetDateTime(DatabaseDataReader reader)
	{
		return GetDateTime(reader, allowNulls: false, DateTime.MinValue);
	}

	public DateTime GetDateTime(DatabaseDataReader reader, bool allowNulls, DateTime defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return new DateTime(reader.GetDateTime(Column).Ticks, DateTimeKind.Utc);
	}

	public bool GetBoolean(DatabaseDataReader reader)
	{
		return GetBoolean(reader, allowNulls: false, defaultValue: false);
	}

	public bool GetBoolean(DatabaseDataReader reader, bool allowNulls, bool defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetBoolean(Column);
	}

	public short GetInt16(DatabaseDataReader reader)
	{
		return GetInt16(reader, allowNulls: false, 0);
	}

	public short GetInt16(DatabaseDataReader reader, bool allowNulls, short defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetInt16(Column);
	}

	public int GetInt32(DatabaseDataReader reader)
	{
		return GetInt32(reader, allowNulls: false, 0);
	}

	public int GetInt32(DatabaseDataReader reader, bool allowNulls, int defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetInt32(Column);
	}

	public long GetInt64(DatabaseDataReader reader)
	{
		return GetInt64(reader, allowNulls: false, 0L);
	}

	public long GetInt64(DatabaseDataReader reader, bool allowNulls, long defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetInt64(Column);
	}

	public float GetFloat(DatabaseDataReader reader)
	{
		return GetFloat(reader, allowNulls: false, 0f);
	}

	public float GetFloat(DatabaseDataReader reader, bool allowNulls, float defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetFloat(Column);
	}

	public double GetDouble(DatabaseDataReader reader)
	{
		return GetDouble(reader, allowNulls: false, 0.0);
	}

	public double GetDouble(DatabaseDataReader reader, bool allowNulls, double defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetDouble(Column);
	}

	public decimal GetDecimal(DatabaseDataReader reader)
	{
		return GetDecimal(reader, allowNulls: false, 0m);
	}

	public decimal GetDecimal(DatabaseDataReader reader, bool allowNulls, decimal defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetDecimal(Column);
	}

	public string GetString(DatabaseDataReader reader)
	{
		return GetString(reader, allowNulls: false, null);
	}

	public string GetString(DatabaseDataReader reader, bool allowNulls)
	{
		return GetString(reader, allowNulls, null);
	}

	public string GetString(DatabaseDataReader reader, bool allowNulls, string defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetString(Column);
	}

	public Guid GetStringAsGuid(DatabaseDataReader reader)
	{
		return GetStringAsGuid(reader, allowNulls: false, Guid.Empty);
	}

	public Guid GetStringAsGuid(DatabaseDataReader reader, bool allowNulls, Guid defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return new Guid(reader.GetString(Column));
	}

	public Guid GetGuid2(DatabaseDataReader reader)
	{
		return GetGuid2(reader, allowNulls: false, Guid.Empty);
	}

	public Guid GetGuid2(DatabaseDataReader reader, bool allowNulls, Guid defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetGuid(Column);
	}

	public byte GetByte(DatabaseDataReader reader)
	{
		return GetByte(reader, allowNulls: false, 0);
	}

	public byte GetByte(DatabaseDataReader reader, bool allowNulls, byte defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetByte(Column);
	}

	public byte[] GetBinary(DatabaseDataReader reader, bool allowNulls)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return null;
		}
		return reader[Column] as byte[];
	}

	public string GetGuid(DatabaseDataReader reader)
	{
		return GetGuid(reader, allowNulls: false, Guid.Empty.ToString());
	}

	public string GetGuid(DatabaseDataReader reader, bool allowNulls, string defaultValue)
	{
		InitColumn(reader);
		if (allowNulls && reader.IsDBNull(Column))
		{
			return defaultValue;
		}
		return reader.GetString(Column);
	}

	public object GetVariant(DatabaseDataReader reader)
	{
		InitColumn(reader);
		return reader.GetValue(Column);
	}

	public async Task<byte[]> GetBytesAsync(DatabaseDataReader reader)
	{
		return await GetBytesAsync(reader, allowNull: false, null);
	}

	public async Task<byte[]> GetBytesAsync(DatabaseDataReader reader, bool allowNull, byte[] defaultValue)
	{
		InitColumn(reader);
		bool flag = allowNull;
		if (flag)
		{
			flag = await reader.IsDBNullAsync(Column);
		}
		if (flag)
		{
			return defaultValue;
		}
		using MemoryStream memoryStream = new MemoryStream();
		using Stream stream = reader.GetStream(Column);
		await stream.CopyToAsync(memoryStream);
		return memoryStream.ToArray();
	}
}
