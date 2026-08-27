using System;

namespace VideoOS.SqlUpdate.Core.Configuration;

public class AppConfig
{
	public string Product { get; }

	public string AccountName { get; }

	public string Password { get; }

	public string DbSchema { get; }

	public SqlUpdateAction SqlUpdateAction { get; }

	public bool ScriptsUsePlaceholders { get; }

	public string MinimumSupportedVersion { get; }

	public string ConnectionString { get; }

	public string SecondaryConnectionString { get; }

	public string NewDatabaseHomeSiteId { get; }

	public AppConfig(SqlUpdateAction sqlUpdateAction, bool scriptsUsePlaceholders = true, string product = null, string accountName = null, string password = null, string dbSchema = null, string minimumSupportedVersion = null, string connectionString = null, string secondaryConnectionString = null)
	{
		SqlUpdateAction = sqlUpdateAction;
		ScriptsUsePlaceholders = scriptsUsePlaceholders;
		Product = product;
		AccountName = accountName;
		Password = password;
		DbSchema = dbSchema ?? "dbo";
		MinimumSupportedVersion = minimumSupportedVersion;
		ConnectionString = connectionString;
		SecondaryConnectionString = secondaryConnectionString;
		NewDatabaseHomeSiteId = Guid.NewGuid().ToString().ToUpperInvariant();
	}
}
