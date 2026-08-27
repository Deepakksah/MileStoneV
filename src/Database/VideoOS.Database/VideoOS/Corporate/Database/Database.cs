using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Configuration;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Scripts;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.Corporate.Database;

public class Database
{
	public const string MINIMUM_SUPPORTED_DATABASE_VERSION = "1.3.16";

	public const string XPCO_SKU = "XPCO";

	public static bool CreateOrUpgradeDatabase(string msConnectionString, string idpConnectionString, string imConnectionString, ILogger logger, string product, IEncryptionKeyCreator encryptionKeyCreator, SqlUpdateAction sqlUpdateAction, string password = null, string accountName = null)
	{
		IDatabaseVersionProvider databaseVersionProvider = new DatabaseVersionProvider(logger, "dbo");
		IDatabaseVersionProvider databaseVersionProvider2 = new IDPVersionProvider(databaseVersionProvider.GetVersion(new DatabaseConnection(msConnectionString, logger)), new DatabaseVersionProvider(logger, "dbo"));
		IDatabaseVersionProvider databaseVersionProvider3 = new DatabaseVersionProvider(logger, "dbo");
		LoggerExtensions.LogInformation(logger, "Creating or updating database...", Array.Empty<object>());
		string connectionString = msConnectionString;
		string secondaryConnectionString = idpConnectionString;
		string password2 = password;
		string accountName2 = accountName;
		AppConfig appConfig = new AppConfig(sqlUpdateAction, scriptsUsePlaceholders: true, product, accountName2, password2, null, "1.3.16", connectionString, secondaryConnectionString);
		accountName2 = idpConnectionString;
		password2 = msConnectionString;
		secondaryConnectionString = password;
		connectionString = accountName;
		AppConfig appConfig2 = new AppConfig(sqlUpdateAction, scriptsUsePlaceholders: true, product, connectionString, secondaryConnectionString, null, "1.3.16", accountName2, password2);
		connectionString = imConnectionString;
		secondaryConnectionString = password;
		password2 = accountName;
		AppConfig appConfig3 = new AppConfig(sqlUpdateAction, scriptsUsePlaceholders: true, product, password2, secondaryConnectionString, null, "1.3.16", connectionString);
		AssemblyWrapper assemblyWrapper = new AssemblyWrapper(logger, "VideoOS.Database");
		MsScripts msScripts = new MsScripts(logger, encryptionKeyCreator);
		IdpScripts idpScripts = new IdpScripts(logger);
		ImScripts imScripts = new ImScripts(logger);
		IReadOnlyList<ScriptBase> upgradeScripts = msScripts.GetUpgradeScripts();
		IReadOnlyList<ScriptBase> baselineScripts = msScripts.GetBaselineScripts();
		IDatabaseSkuProvider databaseSkuProvider = new CorporateDatabaseSkuProvider(logger);
		if (UpdateRunner.Run(appConfig, encryptionKeyCreator, logger, "VideoOS.Corporate.Database.Scripts.MS", upgradeScripts, baselineScripts, assemblyWrapper, databaseVersionProvider, databaseSkuProvider) & UpdateRunner.Run(appConfig2, encryptionKeyCreator, logger, "VideoOS.Corporate.Database.Scripts.IDP", idpScripts.GetUpgradeScripts(), idpScripts.GetBaselineScripts(), assemblyWrapper, databaseVersionProvider2))
		{
			return UpdateRunner.Run(appConfig3, encryptionKeyCreator, logger, "VideoOS.Corporate.Database.Scripts.IM", imScripts.GetUpgradeScripts(), imScripts.GetBaselineScripts(), assemblyWrapper, databaseVersionProvider3);
		}
		return false;
	}

	public static DatabaseValidationResult ValidateDatabase(string connectionString, string product, IEncryptionKeyReader encryptionKeyReader, string password, SqlUpdateAction sqlUpdateAction, ILogger logger)
	{
		MsScripts msScripts = new MsScripts(logger, encryptionKeyReader);
		DatabaseVersionProvider databaseVersionProvider = new DatabaseVersionProvider(logger, "dbo");
		CorporateDatabaseSkuProvider databaseSkuProvider = new CorporateDatabaseSkuProvider(logger);
		return UpdateRunner.ValidateForService(new AppConfig(sqlUpdateAction, scriptsUsePlaceholders: true, product, null, password, null, "1.3.16", connectionString), databaseVersionProvider, databaseSkuProvider, msScripts.GetUpgradeScripts(), encryptionKeyReader, logger);
	}
}
