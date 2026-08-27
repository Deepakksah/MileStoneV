using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Configuration;
using VideoOS.SqlUpdate.Core.Operations;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Scripts;
using VideoOS.SqlUpdate.Core.Services;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.SqlUpdate;

public static class UpdateRunner
{
	private static readonly IReadOnlyList<ScriptBase> _emptyScriptList = new List<ScriptBase>();

	public static bool Run(AppConfig appConfig, IEncryptionKeyCreator encryptionKeyCreator, ILogger logger, string embeddedScriptsPath, IReadOnlyList<ScriptBase> upgradeList, IReadOnlyList<ScriptBase> baselineList, IAssemblyWrapper assemblyWrapper, IDatabaseVersionProvider databaseVersionProvider = null, IDatabaseSkuProvider databaseSkuProvider = null)
	{
		LoggerExtensions.LogInformation(logger, "UpdateRunner initializing...", Array.Empty<object>());
		LoggerExtensions.LogInformation(logger, string.Empty, Array.Empty<object>());
		if (appConfig == null)
		{
			LoggerExtensions.LogError(logger, "UpdateRunner received null AppConfig.", Array.Empty<object>());
			return false;
		}
		if (encryptionKeyCreator == null)
		{
			LoggerExtensions.LogError(logger, "UpdateRunner received null encryptionKeyCreator", Array.Empty<object>());
			return false;
		}
		try
		{
			IDatabaseSkuProvider databaseSkuProvider2 = databaseSkuProvider ?? new DatabaseSkuProvider();
			return RunForService(appConfig, encryptionKeyCreator, logger, embeddedScriptsPath, assemblyWrapper, upgradeList ?? _emptyScriptList, baselineList ?? _emptyScriptList, databaseVersionProvider ?? new DatabaseVersionProviderFactory().Create(logger, appConfig.DbSchema), databaseSkuProvider2);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogWarning(logger, "UpdateRunner failed!", Array.Empty<object>());
			LoggerExtensions.LogError(logger, ex.Message, Array.Empty<object>());
			return false;
		}
	}

	private static bool RunForService(AppConfig appConfig, IEncryptionKeyCreator encryptionKeyCreator, ILogger logger, string embeddedScriptsPath, IAssemblyWrapper assemblyWrapper, IReadOnlyList<ScriptBase> upgradeScripts, IReadOnlyList<ScriptBase> baselineScripts, IDatabaseVersionProvider databaseVersionProvider, IDatabaseSkuProvider databaseSkuProvider)
	{
		MasterDbOperations masterDbOperations = new MasterDbOperations(logger);
		DbOperations dbOperations = new DbOperations(logger);
		ScriptFactory scriptFactory = new ScriptFactory(assemblyWrapper);
		ScriptValidator scriptValidator = new ScriptValidator();
		ScriptsCollector scriptsCollector = new ScriptsCollector(logger, scriptFactory, scriptValidator, assemblyWrapper, embeddedScriptsPath);
		ScriptsExecutor scriptsExecutor = new ScriptsExecutor(logger, dbOperations);
		ValidateUniqueVersion(upgradeScripts);
		return new MainController(logger, appConfig, masterDbOperations, dbOperations, scriptsCollector, upgradeScripts, baselineScripts, scriptsExecutor, databaseVersionProvider, databaseSkuProvider, encryptionKeyCreator).Execute();
	}

	public static DatabaseValidationResult ValidateForService(AppConfig appConfig, IDatabaseVersionProvider databaseVersionProvider, IDatabaseSkuProvider databaseSkuProvider, IReadOnlyList<ScriptBase> upgradeScripts, IEncryptionKeyReader encryptionKeyReader, ILogger logger)
	{
		DbOperations dbOperations = new DbOperations(logger);
		return DbService.Validate(appConfig, dbOperations, databaseVersionProvider, databaseSkuProvider, upgradeScripts, encryptionKeyReader, logger);
	}

	public static void ValidateUniqueVersion(IReadOnlyList<ScriptBase> scripts)
	{
		if (scripts.Count() != scripts.Select((ScriptBase s) => s.Version).Distinct().Count())
		{
			throw new ArgumentException("Script version is not unique!");
		}
	}
}
