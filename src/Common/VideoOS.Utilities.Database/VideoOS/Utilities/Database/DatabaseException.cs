using System;
using System.Data.Common;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public class DatabaseException : DbException
{
	private static readonly int InternalError = 80000;

	private static readonly byte InternalErrorClass = 0;

	private static readonly DatabaseError[] EmptyDatabaseErrorArray = new DatabaseError[0];

	private readonly SqlException _sqlException;

	public bool IsObjectNotFoundException
	{
		get
		{
			SqlException sqlException = _sqlException;
			if (sqlException == null)
			{
				return false;
			}
			return sqlException.Number == 208;
		}
	}

	public int Number
	{
		get
		{
			if (_sqlException != null)
			{
				return _sqlException.Number;
			}
			return InternalError;
		}
	}

	public override int ErrorCode
	{
		get
		{
			if (_sqlException != null)
			{
				return ((ExternalException)(object)_sqlException).ErrorCode;
			}
			return InternalError;
		}
	}

	public byte Class
	{
		get
		{
			if (_sqlException != null)
			{
				return _sqlException.Class;
			}
			return InternalErrorClass;
		}
	}

	public string Procedure
	{
		get
		{
			if (_sqlException != null)
			{
				return _sqlException.Procedure;
			}
			return string.Empty;
		}
	}

	public int LineNumber
	{
		get
		{
			if (_sqlException != null)
			{
				return _sqlException.LineNumber;
			}
			return 0;
		}
	}

	public string Server
	{
		get
		{
			if (_sqlException != null)
			{
				return _sqlException.Server;
			}
			return string.Empty;
		}
	}

	public byte State
	{
		get
		{
			if (_sqlException != null)
			{
				return _sqlException.State;
			}
			return InternalErrorClass;
		}
	}

	public DatabaseError[] Errors
	{
		get
		{
			if (_sqlException == null)
			{
				return EmptyDatabaseErrorArray;
			}
			return _sqlException.DatabaseErrors();
		}
	}

	public DatabaseException()
	{
	}

	public DatabaseException(string message)
		: base(message)
	{
	}

	public DatabaseException(string message, Exception innerException)
		: base((innerException is SqlException) ? (message + $" error code {((SqlException)innerException).Number} class {((SqlException)innerException).Class}") : message, innerException)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (innerException is SqlException)
		{
			_sqlException = (SqlException)(object)((innerException is SqlException) ? innerException : null);
		}
	}
}
