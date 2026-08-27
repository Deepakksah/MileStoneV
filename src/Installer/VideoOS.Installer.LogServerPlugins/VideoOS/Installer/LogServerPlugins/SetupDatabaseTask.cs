using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Logging;
using VideoOS.SqlUpdate;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Configuration;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Installer.LogServerPlugins;

public static class SetupDatabaseTask
{
	public const string MINIMUM_SUPPORTED_DATABASE_VERSION = "1.0.0";

	public static void SetupDatabase(Dictionary<string, string> installParameters, IDatabaseConnectionInfo connectionInfo, IDatabaseHandler databaseHandler)
	{
		if (installParameters == null)
		{
			throw new ArgumentNullException("installParameters");
		}
		if (connectionInfo == null)
		{
			throw new ArgumentNullException("connectionInfo");
		}
		if (databaseHandler == null)
		{
			throw new ArgumentNullException("databaseHandler");
		}
		LogManager.Write("Started");
		ExtractInstallationParameter(installParameters, "SQL_CONNECTION_STRING_LOG");
		string accountName = ExtractInstallationParameter(installParameters, "SERVICEACCOUNT_NONLOC");
		string text = ExtractInstallationParameter(installParameters, "MANAGEMENTAPPNAME");
		LogManager.Write("Management server URL is: " + text);
		try
		{
			new Uri(text);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Error is: {0}", arg));
			throw;
		}
		if (bool.TryParse(ExtractInstallationParameter(installParameters, "LOGSERVER_ANONYMOUSLOGGING"), out var result))
		{
			LogManager.Write("Failed to parse 'LOGSERVER_ANONYMOUSLOGGING'");
		}
		if (!bool.TryParse(ExtractInstallationParameter(installParameters, "IS_EXTERNALLY_MANAGED"), out var result2))
		{
			LogManager.Write("Failed to parse 'IS_EXTERNALLY_MANAGED'");
		}
		if (!installParameters.TryGetValue("SQLKEEPDATA", out var value))
		{
			installParameters.TryGetValue("SQL-KEEP-DATA", out value);
		}
		bool flag = value?.Equals("yes", StringComparison.OrdinalIgnoreCase) ?? true;
		SqlUpdateAction sqlUpdateAction = (result2 ? SqlUpdateAction.CreateOrUpdateDatabaseObjectsOnly : ((!flag) ? SqlUpdateAction.ReCreate : SqlUpdateAction.CreateOrUpgrade));
		LogManager.Write("Calling SqlUpdate");
		if (!CreateOrUpgradeDatabase(connectionInfo.ConnectionString, LogManager.InnerLog, sqlUpdateAction))
		{
			LogManager.Write("SqlUpdate failed");
			throw new SqlUpdateException("SqlUpdate failed");
		}
		if (!result2)
		{
			LogManager.Write("Transferring DB ownership to service account");
			databaseHandler.SetDatabaseOwner(connectionInfo.DatabaseName, accountName);
		}
		if (result)
		{
			databaseHandler.SetAnonymousLogging(result);
		}
	}

	public static string ExtractInstallationParameter(IDictionary<string, string> parameters, string key)
	{
		if (!parameters.TryGetValue(key, out var value))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Installation parameter missing: {0}", key));
			throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not extract key {0} from installation parameters dictionary", key));
		}
		return value;
	}

	private static bool CreateOrUpgradeDatabase(string connectionString, ILogger logger, SqlUpdateAction sqlUpdateAction)
	{
		AppConfig appConfig = new AppConfig(sqlUpdateAction, scriptsUsePlaceholders: false, "XPCO", null, null, null, "1.0.0", connectionString);
		LogServerInstallerOemResources logServerInstallerOemResources = new LogServerInstallerOemResources(new ResourcesFolderLocator());
		IEncryptionKeyCreator encryptionKeyCreator = KeyManagerFactory.GetRestrictedEncryptionKeyCreator(sqlConnectionManager: new SqlConnectionManager(new DatabaseConnectionInfo(connectionString), logger), manufacturerName: logServerInstallerOemResources.OemCompanyNameShort, logger: logger);
		LsScripts lsScripts = new LsScripts(logger);
		return UpdateRunner.Run(assemblyWrapper: new AssemblyWrapper(logger, "VideoOS.Installer.LogServerPlugins"), databaseVersionProvider: new LogServerDatabaseVersionProvider(logger), appConfig: appConfig, encryptionKeyCreator: encryptionKeyCreator, logger: logger, embeddedScriptsPath: lsScripts.EmbeddedPath, upgradeList: lsScripts.GetUpgradeScripts(), baselineList: lsScripts.GetBaselineScripts(), databaseSkuProvider: new DatabaseSkuProvider());
	}

	public static DatabaseValidationResult ValidateDatabase(string connectionString, SqlUpdateAction sqlUpdateAction, ILogger logger)
	{
		LsScripts lsScripts = new LsScripts(logger);
		LogServerDatabaseVersionProvider databaseVersionProvider = new LogServerDatabaseVersionProvider(logger);
		DatabaseSkuProvider databaseSkuProvider = new DatabaseSkuProvider();
		return UpdateRunner.ValidateForService(new AppConfig(sqlUpdateAction, scriptsUsePlaceholders: false, null, null, null, null, "1.0.0", connectionString), databaseVersionProvider, databaseSkuProvider, lsScripts.GetUpgradeScripts(), null, logger);
	}
}
