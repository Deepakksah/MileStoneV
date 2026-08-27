using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Corporate.Database;
using VideoOS.SqlUpdate.Core.Configuration;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.Server.Admin.Tasks;

public class SetupDatabaseTask
{
	public ILogger Logger { get; }

	public SetupDatabaseTask(ILogger logger)
	{
		Logger = logger;
	}

	public void Install(IDatabaseConnectionInfo databaseConnectionInfoMs, IDatabaseConnectionInfo databaseConnectionInfoIdp, IDatabaseConnectionInfo databaseConnectionInfoIncidentManager, IEncryptionKeyCreator encryptionKeyCreator, string product, string accountName, string password, bool isExternallyManaged, bool useExistingDatabases)
	{
		if (databaseConnectionInfoMs == null)
		{
			throw new ArgumentNullException("databaseConnectionInfoMs");
		}
		if (databaseConnectionInfoIdp == null)
		{
			throw new ArgumentNullException("databaseConnectionInfoIdp");
		}
		if (databaseConnectionInfoIncidentManager == null)
		{
			throw new ArgumentNullException("databaseConnectionInfoIncidentManager");
		}
		if (!Database.CreateOrUpgradeDatabase(databaseConnectionInfoMs.ConnectionString, databaseConnectionInfoIdp.ConnectionString, databaseConnectionInfoIncidentManager.ConnectionString, Logger, product, encryptionKeyCreator, isExternallyManaged ? SqlUpdateAction.CreateOrUpdateDatabaseObjectsOnly : ((!useExistingDatabases) ? SqlUpdateAction.ReCreate : SqlUpdateAction.CreateOrUpgrade), password, accountName))
		{
			throw new DbUpdateException(string.Format(CultureInfo.InvariantCulture, "SqlUpdate failed!"));
		}
	}
}
