using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Configuration;
using VideoOS.SqlUpdate.Core.Connection;
using VideoOS.SqlUpdate.Core.Operations;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Scripts;
using VideoOS.SqlUpdate.Core.Services;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.SqlUpdate;

public class MainController : IMainController
{
	private readonly AppConfig _appConfig;

	private readonly ILogger _logger;

	private readonly IMasterDbOperations _masterDbOperations;

	private readonly IDbOperations _dbOperations;

	private readonly IScriptsCollector _scriptsCollector;

	private readonly IReadOnlyList<ScriptBase> _upgradeScripts;

	private readonly IReadOnlyList<ScriptBase> _baselineScripts;

	private readonly IScriptsExecutor _scriptsExecutor;

	private readonly IDatabaseVersionProvider _databaseVersionProvider;

	private readonly IDatabaseSkuProvider _databaseSkuProvider;

	private readonly IEncryptionKeyCreator _encryptionKeyCreator;

	public MainController(ILogger logger, AppConfig appConfig, IMasterDbOperations masterDbOperations, IDbOperations dbOperations, IScriptsCollector scriptsCollector, IReadOnlyList<ScriptBase> upgradeScripts, IReadOnlyList<ScriptBase> baselineScripts, IScriptsExecutor scriptsExecutor, IDatabaseVersionProvider databaseVersionProvider, IDatabaseSkuProvider databaseSkuProvider, IEncryptionKeyCreator encryptionKeyCreator)
	{
		_logger = logger;
		_appConfig = appConfig;
		_masterDbOperations = masterDbOperations;
		_dbOperations = dbOperations;
		_scriptsCollector = scriptsCollector;
		_upgradeScripts = upgradeScripts;
		_baselineScripts = baselineScripts;
		_scriptsExecutor = scriptsExecutor;
		_databaseVersionProvider = databaseVersionProvider;
		_databaseSkuProvider = databaseSkuProvider;
		_encryptionKeyCreator = encryptionKeyCreator;
	}

	public bool Execute()
	{
		try
		{
			DbService dbService = new DbService(_logger, _appConfig, _masterDbOperations, _dbOperations, _scriptsCollector, _upgradeScripts, _baselineScripts, _scriptsExecutor, _databaseVersionProvider, _databaseSkuProvider, _encryptionKeyCreator);
			if (!dbService.ShouldExecute())
			{
				return true;
			}
			string text = "'" + ConnectionStringHelper.ExtractDbName(_appConfig.ConnectionString) + " (" + _appConfig.DbSchema + ")'";
			LoggerExtensions.LogInformation(_logger, "{DatabaseNameAndScnema} is about to execute if all requirements are OK", new object[1] { text });
			if (!dbService.Execute())
			{
				throw new SqlUpdateException(text + " execution has failed");
			}
			LoggerExtensions.LogInformation(_logger, "{DatabaseNameAndSchema} has been executed successfully", new object[1] { text });
			return true;
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "{Excepton}", new object[1] { ex.Message });
			return false;
		}
	}
}
