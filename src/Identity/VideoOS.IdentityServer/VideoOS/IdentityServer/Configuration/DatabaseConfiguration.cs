using VideoOS.Utilities.Database;

namespace VideoOS.IdentityServer.Configuration;

public class DatabaseConfiguration : IDatabaseConfiguration
{
	public string ConnectionString { get; private set; }

	public DatabaseConfiguration()
	{
		DatabaseConnectionInfoFromStorage databaseConnectionInfoFromStorage = new DatabaseConnectionInfoFromStorage(SqlConnectionServerType.IDP);
		ConnectionString = databaseConnectionInfoFromStorage.ConnectionString;
	}
}
