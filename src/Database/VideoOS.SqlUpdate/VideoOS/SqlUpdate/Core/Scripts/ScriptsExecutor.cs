using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Operations;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Scripts;

public class ScriptsExecutor : IScriptsExecutor
{
	private readonly ILogger _logger;

	private readonly IDbOperations _dbOperations;

	public ScriptsExecutor(ILogger logger, IDbOperations dbOperations)
	{
		_logger = logger;
		_dbOperations = dbOperations;
	}

	public void Execute(DatabaseConnection connection, IEnumerable<ScriptBase> scripts, IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, IDatabaseVersionProvider databaseVersionProvider, string dbSchema)
	{
		LoggerExtensions.LogInformation(_logger, "Calling Execute on Database: {DatabaseName}", new object[1] { connection.Database });
		if (scripts == null || scripts.Count() == 0)
		{
			LoggerExtensions.LogInformation(_logger, "Scripts collection to execute was empty!", Array.Empty<object>());
			return;
		}
		foreach (ScriptBase script in scripts)
		{
			LoggerExtensions.LogInformation(_logger, "Executing: {ScriptName}", new object[1] { script.Name });
			_dbOperations.ExecuteScript(script, scriptContentReplacements, connection, databaseVersionProvider, dbSchema);
		}
	}
}
