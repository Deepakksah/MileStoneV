using System;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public static class ExceptionHandling
{
	public static string GetResourceString(Exception ex)
	{
		if (ex is DatabaseException ex2)
		{
			if (ex2.Number == 10054)
			{
				return Resources.IServerSqlError;
			}
			return Resources.GenerelSqlError;
		}
		return string.Empty;
	}
}
