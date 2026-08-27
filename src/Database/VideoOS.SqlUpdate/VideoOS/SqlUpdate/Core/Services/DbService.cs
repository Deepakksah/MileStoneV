using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Configuration;
using VideoOS.SqlUpdate.Core.Connection;
using VideoOS.SqlUpdate.Core.Operations;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Scripts;
using VideoOS.SqlUpdate.Resources;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.SqlUpdate.Core.Services;

public class DbService : IDbService
{
	private readonly IMasterDbOperations _masterDbOperations;

	private readonly IScriptsCollector _scriptsCollector;

	private readonly IReadOnlyList<ScriptBase> _upgradeScripts;

	private readonly IReadOnlyList<ScriptBase> _baselineScripts;

	private readonly IScriptsExecutor _scriptsExecutor;

	private readonly IDatabaseVersionProvider _databaseVersionProvider;

	private readonly IDbOperations _dbOperations;

	private readonly ILogger _logger;

	private readonly IEncryptionKeyCreator _encryptionKeyCreator;

	private readonly IDatabaseSkuProvider _databaseSkuProvider;

	private readonly AppConfig _appConfig;

	public DbService(ILogger logger, AppConfig appConfig, IMasterDbOperations masterDbOperations, IDbOperations dbOperations, IScriptsCollector scriptsCollector, IReadOnlyList<ScriptBase> upgradeScripts, IReadOnlyList<ScriptBase> baselineScripts, IScriptsExecutor scriptsExecutor, IDatabaseVersionProvider databaseVersionProvider, IDatabaseSkuProvider databaseSkuProvider, IEncryptionKeyCreator encryptionKeyCreator)
	{
		_logger = logger ?? throw new ArgumentNullException("logger");
		_appConfig = appConfig ?? throw new ArgumentNullException("appConfig");
		_masterDbOperations = masterDbOperations ?? throw new ArgumentNullException("masterDbOperations");
		_dbOperations = dbOperations ?? throw new ArgumentNullException("dbOperations");
		_scriptsCollector = scriptsCollector ?? throw new ArgumentNullException("scriptsCollector");
		_upgradeScripts = upgradeScripts ?? throw new ArgumentNullException("upgradeScripts");
		_baselineScripts = baselineScripts ?? throw new ArgumentNullException("baselineScripts");
		_scriptsExecutor = scriptsExecutor ?? throw new ArgumentNullException("scriptsExecutor");
		_databaseVersionProvider = databaseVersionProvider ?? throw new ArgumentNullException("databaseVersionProvider");
		_databaseSkuProvider = databaseSkuProvider ?? throw new ArgumentNullException("databaseSkuProvider");
		_encryptionKeyCreator = encryptionKeyCreator ?? throw new ArgumentNullException("encryptionKeyCreator");
	}

	public bool ShouldExecute()
	{
		return !string.IsNullOrEmpty(_appConfig.ConnectionString);
	}

	protected static IEnumerable<KeyValuePair<string, string>> GetScriptContentReplacements(AppConfig appConfig, string masterKey, string masterKeyEncrypted)
	{
		if (!appConfig.ScriptsUsePlaceholders)
		{
			return null;
		}
		string value = ConnectionStringHelper.ExtractDbName(appConfig.ConnectionString);
		string value2 = ConnectionStringHelper.ExtractDbName(appConfig.SecondaryConnectionString);
		string value3 = CreateLicenseTlsData(appConfig.NewDatabaseHomeSiteId, masterKey);
		return new KeyValuePair<string, string>[8]
		{
			new KeyValuePair<string, string>("{IdpDatabaseName}", value2),
			new KeyValuePair<string, string>("{DatabaseName}", value),
			new KeyValuePair<string, string>("{MasterEncryptionKey}", masterKey),
			new KeyValuePair<string, string>("{EncryptedMasterEncryptionKey}", masterKeyEncrypted),
			new KeyValuePair<string, string>("{NewDatabaseHomeSiteId}", appConfig.NewDatabaseHomeSiteId),
			new KeyValuePair<string, string>("{NewDatabaseLicenseTlsData}", value3),
			new KeyValuePair<string, string>("{target}", value),
			new KeyValuePair<string, string>("{source}", value2)
		};
	}

	protected static DatabaseValidationResult ValidateDbUpdatePossibility(AppConfig appConfig, string masterKey, Version currentDbVersion, IDatabaseSkuProvider databaseSkuProvider, IReadOnlyList<ScriptBase> upgradeScripts, ILogger logger)
	{
		string arg = ConnectionStringHelper.ExtractDbName(appConfig.ConnectionString) + "." + appConfig.DbSchema;
		if (!string.IsNullOrEmpty(appConfig.MinimumSupportedVersion))
		{
			Version version = new Version(appConfig.MinimumSupportedVersion);
			if (currentDbVersion < version)
			{
				return new DatabaseValidationResult(DatabaseValidationStatus.VersionTooLow, string.Format(VideoOS.SqlUpdate.Resources.Resources.DatabaseValidation_VersionTooLow, arg, currentDbVersion, version));
			}
			if (upgradeScripts != null && upgradeScripts.Count > 0)
			{
				Version version2 = upgradeScripts.Max((ScriptBase x) => x.Version);
				if (currentDbVersion > version2)
				{
					return new DatabaseValidationResult(DatabaseValidationStatus.DatabaseVersionDowngradeNotSupported, string.Format(VideoOS.SqlUpdate.Resources.Resources.DatabaseValidation_DatabaseVersionDowngradeNotSupported, arg, currentDbVersion, version2));
				}
			}
		}
		VmsDbType sku;
		using (DatabaseConnection databaseConnection = new DatabaseConnection(appConfig.ConnectionString, logger))
		{
			sku = databaseSkuProvider.GetSku(databaseConnection, masterKey);
		}
		if (sku == VmsDbType.Unknown)
		{
			LoggerExtensions.LogInformation(logger, "Unknown VMSDbType", Array.Empty<object>());
			return new DatabaseValidationResult(DatabaseValidationStatus.Success, string.Empty);
		}
		if (string.IsNullOrEmpty(appConfig.Product))
		{
			throw new ArgumentNullException("Product");
		}
		VmsDbType vmsDbType = VmsDbTypeMapper.Map(appConfig.Product);
		if (sku > vmsDbType)
		{
			return new DatabaseValidationResult(DatabaseValidationStatus.DatabaseTypeDowngradeNotSupported, string.Format(VideoOS.SqlUpdate.Resources.Resources.DatabaseValidation_DatabaseTypeDowngradeNotSupported, arg, sku, vmsDbType));
		}
		return new DatabaseValidationResult(DatabaseValidationStatus.Success, string.Empty);
	}

	private static IReadOnlyList<ScriptBase> CollectUpgradeScripts(Version currentDbVersion, IReadOnlyList<ScriptBase> upgradeScripts, IScriptsCollector scriptsCollector, ILogger logger)
	{
		LoggerExtensions.LogDebug(logger, "Getting upgrade scripts to upgrade from version {CurrentDbVersion}", new object[1] { currentDbVersion });
		return (from s in upgradeScripts.Where(delegate(ScriptBase s)
			{
				if (!(s.Version == null) && !(currentDbVersion == null))
				{
					Version version = s.Version;
					if ((object)version == null)
					{
						return false;
					}
					return version.CompareTo(currentDbVersion) > 0;
				}
				return true;
			})
			orderby s.Version
			select s).ToList().Concat(scriptsCollector.GetGlobalScripts("Temp", currentDbVersion, scriptsAreTemp: true)).Concat(scriptsCollector.GetGlobalScripts("Functions", currentDbVersion))
			.Concat(scriptsCollector.GetGlobalScripts("StoredProcedures", currentDbVersion))
			.Concat(scriptsCollector.GetGlobalScripts("FinalScripts", currentDbVersion))
			.ToList();
	}

	public bool Execute()
	{
		string text = ConnectionStringHelper.ExtractDbName(_appConfig.ConnectionString) + "." + _appConfig.DbSchema;
		LoggerExtensions.LogInformation(_logger, "Update process started for database '{DatabaseNameWithSchema}'.", new object[1] { text });
		DatabaseValidationResult databaseValidationResult = Validate(_appConfig, _dbOperations, _databaseVersionProvider, _databaseSkuProvider, _upgradeScripts, _encryptionKeyCreator, _logger);
		if (!databaseValidationResult.Status.HasFlag(DatabaseValidationStatus.Success))
		{
			LoggerExtensions.LogError(_logger, "{ErrorMessage}", new object[1] { databaseValidationResult.ErrorMessage });
			return false;
		}
		LoggerExtensions.LogInformation(_logger, "Executing update for database '{DatabaseNameWithSchema}'.", new object[1] { text });
		DoExecute(_appConfig, databaseValidationResult.Status, _dbOperations, _databaseVersionProvider, _masterDbOperations, _baselineScripts, _upgradeScripts, _scriptsCollector, _scriptsExecutor, _encryptionKeyCreator, _logger);
		LoggerExtensions.LogInformation(_logger, "Done executing update for database '{DatabaseNameWithSchema}'.", new object[1] { text });
		return true;
	}

	public static DatabaseValidationResult Validate(AppConfig appConfig, IDbOperations dbOperations, IDatabaseVersionProvider databaseVersionProvider, IDatabaseSkuProvider databaseSkuProvider, IReadOnlyList<ScriptBase> upgradeScripts, IEncryptionKeyReader encryptionKeyReader, ILogger logger)
	{
		string text = ConnectionStringHelper.ExtractDbName(appConfig.ConnectionString) + "." + appConfig.DbSchema;
		LoggerExtensions.LogInformation(logger, "Validating database '" + text + "'.", Array.Empty<object>());
		DatabaseValidationResult databaseValidationResult = DoValidate(appConfig, dbOperations, databaseVersionProvider, databaseSkuProvider, encryptionKeyReader, upgradeScripts, logger);
		LoggerExtensions.LogInformation(logger, $"Done validating database '{text}' Result: '{databaseValidationResult.Status}.", Array.Empty<object>());
		if (!databaseValidationResult.Status.HasFlag(DatabaseValidationStatus.Success))
		{
			LoggerExtensions.LogInformation(logger, databaseValidationResult.ErrorMessage, Array.Empty<object>());
		}
		return databaseValidationResult;
	}

	private static void DoExecute(AppConfig appConfig, DatabaseValidationStatus databaseValidationStatus, IDbOperations dbOperations, IDatabaseVersionProvider databaseVersionProvider, IMasterDbOperations masterDbOperations, IReadOnlyList<ScriptBase> baselineScripts, IReadOnlyList<ScriptBase> upgradeScripts, IScriptsCollector scriptsCollector, IScriptsExecutor scriptsExecutor, IEncryptionKeyCreator encryptionKeyCreator, ILogger logger)
	{
		Version currentDbVersion = null;
		if (databaseValidationStatus.HasFlag(DatabaseValidationStatus.Delete))
		{
			DropExistingDb(appConfig, masterDbOperations, logger);
		}
		if (databaseValidationStatus.HasFlag(DatabaseValidationStatus.Create))
		{
			CreateNewDb(appConfig, masterDbOperations, logger);
		}
		UpdatePasswordAndKey(appConfig, encryptionKeyCreator);
		var (masterKey, masterKeyEncrypted) = GetMasterKeyAndEncrypted(validateOnly: false, appConfig, encryptionKeyCreator, logger);
		if (databaseValidationStatus.HasFlag(DatabaseValidationStatus.EmptyDatabase))
		{
			RunBaselineScripts(appConfig, masterKey, masterKeyEncrypted, baselineScripts, scriptsExecutor, logger);
		}
		using (DatabaseConnection connection = new DatabaseConnection(appConfig.ConnectionString, logger))
		{
			currentDbVersion = databaseVersionProvider.GetVersion(connection);
		}
		UpdateCompatibilityLevel(appConfig, masterDbOperations, dbOperations, logger);
		IReadOnlyList<ScriptBase> validScripts = CollectUpgradeScripts(currentDbVersion, upgradeScripts, scriptsCollector, logger);
		UpdateExistingDb(appConfig, masterKey, masterKeyEncrypted, databaseVersionProvider, dbOperations, validScripts, scriptsExecutor, logger);
	}

	private static DatabaseValidationResult DoValidate(AppConfig appConfig, IDbOperations dbOperations, IDatabaseVersionProvider versionProvider, IDatabaseSkuProvider databaseSkuProvider, IEncryptionKeyReader encryptionKeyReader, IReadOnlyList<ScriptBase> upgradeScripts, ILogger logger)
	{
		bool num = dbOperations.DbExists(appConfig.ConnectionString);
		bool flag = num && appConfig.SqlUpdateAction == SqlUpdateAction.ReCreate;
		bool flag2 = !num || flag;
		Version version = null;
		if (flag || flag2)
		{
			if (appConfig.SqlUpdateAction == SqlUpdateAction.CreateOrUpdateDatabaseObjectsOnly)
			{
				string text = ConnectionStringHelper.ExtractDbName(appConfig.ConnectionString);
				return new DatabaseValidationResult(DatabaseValidationStatus.DatabaseCreationNotSupported, string.Format(VideoOS.SqlUpdate.Resources.Resources.DatabaseValidation_DatabaseCreationNotSupported, text + "." + appConfig.DbSchema));
			}
			DatabaseValidationStatus databaseValidationStatus = DatabaseValidationStatus.Success | DatabaseValidationStatus.Create;
			if (flag)
			{
				databaseValidationStatus |= DatabaseValidationStatus.Delete | DatabaseValidationStatus.EmptyDatabase;
				return new DatabaseValidationResult(databaseValidationStatus, string.Empty);
			}
			if (flag2)
			{
				if (versionProvider.IsMigrationVersionProvider)
				{
					using DatabaseConnection connection = new DatabaseConnection(appConfig.ConnectionString, logger);
					version = versionProvider.GetVersion(connection);
				}
				if (version == null)
				{
					databaseValidationStatus |= DatabaseValidationStatus.EmptyDatabase;
				}
				return new DatabaseValidationResult(databaseValidationStatus, string.Empty);
			}
		}
		using (DatabaseConnection connection2 = new DatabaseConnection(appConfig.ConnectionString, logger))
		{
			version = versionProvider.GetVersion(connection2);
		}
		if (version == null)
		{
			return new DatabaseValidationResult(DatabaseValidationStatus.Success | DatabaseValidationStatus.EmptyDatabase, string.Empty);
		}
		string item = GetMasterKeyAndEncrypted(validateOnly: true, appConfig, encryptionKeyReader, logger).Item1;
		DatabaseValidationResult databaseValidationResult = ValidateDbUpdatePossibility(appConfig, item, version, databaseSkuProvider, upgradeScripts, logger);
		if (databaseValidationResult.Status != DatabaseValidationStatus.Success)
		{
			return databaseValidationResult;
		}
		return new DatabaseValidationResult(DatabaseValidationStatus.Success | DatabaseValidationStatus.Upgrade, string.Empty);
	}

	private static void DropExistingDb(AppConfig appConfig, IMasterDbOperations masterDbOperations, ILogger logger)
	{
		string text = ConnectionStringHelper.ExtractDbName(appConfig.ConnectionString);
		LoggerExtensions.LogInformation(logger, "Deleting existing database: {DatabasseName}", new object[1] { text });
		masterDbOperations.DropDb(appConfig.ConnectionString);
		LoggerExtensions.LogInformation(logger, "Database '{DatabaseName}' has been deleted", new object[1] { text });
	}

	private static void CreateNewDb(AppConfig appConfig, IMasterDbOperations masterDbOperations, ILogger logger)
	{
		string text = ConnectionStringHelper.ExtractDbName(appConfig.ConnectionString);
		LoggerExtensions.LogInformation(logger, "Creating database: {DatabaseName}", new object[1] { text });
		masterDbOperations.CreateDb(appConfig.ConnectionString);
		LoggerExtensions.LogInformation(logger, "Database '{DatabaseName}' has been created", new object[1] { text });
	}

	private static void RunBaselineScripts(AppConfig appConfig, string masterKey, string masterKeyEncrypted, IReadOnlyList<ScriptBase> baselineScripts, IScriptsExecutor scriptsExecutor, ILogger logger)
	{
		if (baselineScripts != null)
		{
			LoggerExtensions.LogInformation(logger, "Executing Baseline scripts", Array.Empty<object>());
			using (DatabaseConnection connection = new DatabaseConnection(appConfig.ConnectionString, logger))
			{
				scriptsExecutor.Execute(connection, baselineScripts, GetScriptContentReplacements(appConfig, masterKey, masterKeyEncrypted), null, null);
			}
			LoggerExtensions.LogInformation(logger, "Baseline scripts have been executed", Array.Empty<object>());
		}
	}

	protected static void UpdateExistingDb(AppConfig appConfig, string masterKey, string masterKeyEncrypted, IDatabaseVersionProvider databaseVersionProvider, IDbOperations dbOperations, IReadOnlyList<ScriptBase> validScripts, IScriptsExecutor scriptsExecutor, ILogger logger)
	{
		string text = ConnectionStringHelper.ExtractDbName(appConfig.ConnectionString);
		LoggerExtensions.LogInformation(logger, "Updating existing database {DatabaseName}", new object[1] { text });
		try
		{
			using DatabaseConnection connection = new DatabaseConnection(appConfig.ConnectionString, logger);
			if (!dbOperations.IsReadCommittedSnapshotIsolationEnabled(connection))
			{
				LoggerExtensions.LogInformation(logger, "Enabling snapshot isolation for database {DatabaseName}", new object[1] { text });
				dbOperations.EnableSnapshotIsolation(connection);
			}
		}
		catch (DatabaseException ex)
		{
			LoggerExtensions.LogError(logger, "An error occurred: {Exception}\nREAD_COMMITTED_SNAPSHOT could not be checked or enabled for database {DatabaseName}. Please make sure it is enabled to ensure proper performance of the system.", new object[2] { ex, text });
		}
		if (validScripts == null || validScripts.Count == 0)
		{
			return;
		}
		LoggerExtensions.LogInformation(logger, "Executing update of database: {DatabaseName}", new object[1] { text });
		using DatabaseConnection connection2 = new DatabaseConnection(appConfig.ConnectionString, logger);
		scriptsExecutor.Execute(connection2, validScripts, GetScriptContentReplacements(appConfig, masterKey, masterKeyEncrypted), databaseVersionProvider, appConfig.DbSchema);
	}

	private static string CreateLicenseTlsData(string homeSiteId, string masterKey)
	{
		if (string.IsNullOrEmpty(masterKey))
		{
			return "";
		}
		AsymmetricAlgorithm asymmetricAlgorithm = AsymmetricAlgorithm.Create("RSA");
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings settings = new XmlWriterSettings
		{
			OmitXmlDeclaration = true,
			Encoding = Encoding.Unicode
		};
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
		{
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("TLS");
			xmlWriter.WriteAttributeString("Id", homeSiteId.ToUpperInvariant());
			xmlWriter.WriteRaw(asymmetricAlgorithm.ToXmlString(includePrivateParameters: true));
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
		}
		StringBuilder stringBuilder2 = new StringBuilder("0x", stringBuilder.Length * 2);
		byte[] array = Encrypt(stringBuilder.ToString(), masterKey);
		foreach (byte b in array)
		{
			stringBuilder2.AppendFormat("{0:x2}", b);
		}
		return stringBuilder2.ToString();
	}

	private static byte[] Encrypt(string value, string masterKey)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(value);
		return EncryptionFactory.CreateSymmetric(masterKey).EncryptSequence(bytes);
	}

	private static void UpdatePasswordAndKey(AppConfig appConfig, IEncryptionKeyCreator encryptionKeyCreator)
	{
		if (appConfig.ScriptsUsePlaceholders)
		{
			if (encryptionKeyCreator.GetKeyManagementState() == KeyManagementState.KeyMissing)
			{
				encryptionKeyCreator.InsertPasswordAndKey(appConfig.Password, appConfig.AccountName);
			}
			else if (appConfig.Password != null)
			{
				encryptionKeyCreator.VerifyAndSavePassword(appConfig.Password, appConfig.AccountName);
			}
		}
	}

	private static (string, string) GetMasterKeyAndEncrypted(bool validateOnly, AppConfig appConfig, IEncryptionKeyReader encryptionKeyReader, ILogger logger)
	{
		if (!appConfig.ScriptsUsePlaceholders)
		{
			return (string.Empty, string.Empty);
		}
		string item2;
		if (validateOnly)
		{
			if (encryptionKeyReader.GetKeyManagementState() == KeyManagementState.KeyMissing)
			{
				return (string.Empty, string.Empty);
			}
			if (appConfig.Password != null)
			{
				string item = Convert.ToBase64String(encryptionKeyReader.GetUnencryptedKey(appConfig.Password));
				item2 = Convert.ToBase64String(encryptionKeyReader.GetKey());
				return (item, item2);
			}
		}
		string item3 = Convert.ToBase64String(encryptionKeyReader.GetUnencryptedKey());
		item2 = Convert.ToBase64String(encryptionKeyReader.GetKey());
		return (item3, item2);
	}

	private static void UpdateCompatibilityLevel(AppConfig appConfig, IMasterDbOperations masterDbOperations, IDbOperations dbOperations, ILogger logger)
	{
		if (appConfig.SqlUpdateAction == SqlUpdateAction.CreateOrUpdateDatabaseObjectsOnly)
		{
			return;
		}
		int compatibilityLevel = masterDbOperations.GetCompatibilityLevel(appConfig.ConnectionString);
		using DatabaseConnection connection = new DatabaseConnection(appConfig.ConnectionString, logger);
		dbOperations.SetCompatibilityLevel(connection, compatibilityLevel);
	}
}
