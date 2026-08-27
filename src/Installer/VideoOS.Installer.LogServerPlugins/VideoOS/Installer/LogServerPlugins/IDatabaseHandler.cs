namespace VideoOS.Installer.LogServerPlugins;

public interface IDatabaseHandler
{
	void SetDatabaseOwner(string databaseName, string accountName);

	void SetAnonymousLogging(bool enabled);
}
