using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public sealed class DatabaseParameter : DbParameter
{
	private readonly SqlParameter _sqlParameter = new SqlParameter();

	public override DbType DbType
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).DbType;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).DbType = value;
		}
	}

	public override ParameterDirection Direction
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).Direction;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).Direction = value;
		}
	}

	public override bool IsNullable
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).IsNullable;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).IsNullable = value;
		}
	}

	public override string ParameterName
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).ParameterName;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).ParameterName = value;
		}
	}

	public override int Size
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).Size;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).Size = value;
		}
	}

	public override string SourceColumn
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).SourceColumn;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).SourceColumn = value;
		}
	}

	public override bool SourceColumnNullMapping
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).SourceColumnNullMapping;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).SourceColumnNullMapping = value;
		}
	}

	public override object Value
	{
		get
		{
			return ((DbParameter)(object)_sqlParameter).Value;
		}
		set
		{
			((DbParameter)(object)_sqlParameter).Value = value;
		}
	}

	public SqlParameter Parameter => _sqlParameter;

	public SqlDbType SqlDbType
	{
		get
		{
			return _sqlParameter.SqlDbType;
		}
		set
		{
			_sqlParameter.SqlDbType = value;
		}
	}

	public string TypeName
	{
		get
		{
			return _sqlParameter.TypeName;
		}
		set
		{
			_sqlParameter.TypeName = value;
		}
	}

	public string UdtTypeName
	{
		get
		{
			return _sqlParameter.UdtTypeName;
		}
		set
		{
			_sqlParameter.UdtTypeName = value;
		}
	}

	public DatabaseParameter()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_000b: Expected O, but got Unknown


	public DatabaseParameter(string parameterName, object value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		ParameterName = parameterName;
		Value = value;
	}

	public DatabaseParameter(string parameterName, SqlDbType sqlDbType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		ParameterName = parameterName;
		SqlDbType = sqlDbType;
	}

	public DatabaseParameter(string parameterName, SqlDbType sqlDbType, int size)
		: this(parameterName, sqlDbType)
	{
		Size = size;
	}

	public DatabaseParameter(string parameterName, object value, int size)
		: this(parameterName, value)
	{
		Size = size;
	}

	public DatabaseParameter(SqlParameter sqlParameter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_sqlParameter = sqlParameter;
	}

	public override void ResetDbType()
	{
		((DbParameter)(object)_sqlParameter).ResetDbType();
	}
}
