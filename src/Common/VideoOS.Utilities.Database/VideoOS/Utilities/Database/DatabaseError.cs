using System;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

[Serializable]
public class DatabaseError
{
	private readonly SqlError _sqlError;

	public int Number => _sqlError.Number;

	public byte State => _sqlError.State;

	public byte Class => _sqlError.Class;

	public string Server => _sqlError.Server;

	public string Source => _sqlError.Source;

	public string Procedure => _sqlError.Procedure;

	public int LineNumber => _sqlError.LineNumber;

	public string Message => _sqlError.Message;

	public DatabaseError(SqlError sqlError)
	{
		_sqlError = sqlError;
	}
}
