using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public static class DatabaseHelper
{
	public static void WrapSqlException(Action action)
	{
		//IL_0009: Expected O, but got Unknown
		try
		{
			action();
		}
		catch (SqlException ex)
		{
			SqlException ex2 = ex;
			throw new DatabaseException(((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	public static T WrapSqlException<T>(Func<T> func)
	{
		//IL_000a: Expected O, but got Unknown
		try
		{
			return func();
		}
		catch (SqlException ex)
		{
			SqlException ex2 = ex;
			throw new DatabaseException(((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	public static async Task WrapSqlExceptionAsync(Func<Task> action)
	{
		try
		{
			await action();
		}
		catch (SqlException ex)
		{
			SqlException ex2 = ex;
			throw new DatabaseException(((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}

	public static async Task<T> WrapSqlExceptionAsync<T>(Func<Task<T>> func)
	{
		try
		{
			return await func();
		}
		catch (SqlException ex)
		{
			SqlException ex2 = ex;
			throw new DatabaseException(((Exception)(object)ex2).Message, (Exception)(object)ex2);
		}
	}
}
