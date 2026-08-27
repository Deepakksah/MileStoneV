using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public static class SqlExceptionExtensions
{
	public static DatabaseError[] DatabaseErrors(this SqlException sqlException)
	{
		DatabaseError[] array = new DatabaseError[sqlException.Errors.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new DatabaseError(sqlException.Errors[i]);
		}
		return array;
	}
}
