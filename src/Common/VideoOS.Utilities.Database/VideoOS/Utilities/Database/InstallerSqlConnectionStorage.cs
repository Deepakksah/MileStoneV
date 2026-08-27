using System;

namespace VideoOS.Utilities.Database;

public sealed class InstallerSqlConnectionStorage : ISqlConnectionReadonlyStorage
{
	private readonly string _connectionString;

	public InstallerSqlConnectionStorage(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentNullException("connectionString");
		}
		_connectionString = connectionString;
	}

	public string RetrieveConnectionString()
	{
		return _connectionString;
	}
}
