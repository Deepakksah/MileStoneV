namespace VideoOS.Utilities.Database;

public interface IDatabaseConnectionInfo
{
	string ServerName { get; }

	string DatabaseName { get; }

	string ConnectionString { get; }

	string ApplicationName { get; }

	AuthenticationType AuthenticationType { get; }

	void Validate();

	IDatabaseConnectionInfo CloneWithNewDatabaseName(string newDatabaseName);

	IDatabaseConnectionInfo CloneWithMasterDatabaseName();
}
