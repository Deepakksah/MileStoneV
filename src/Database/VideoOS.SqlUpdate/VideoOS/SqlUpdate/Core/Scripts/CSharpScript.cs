using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Scripts;

public class CSharpScript : ScriptBase
{
	private readonly Action<DatabaseConnection> _action;

	public CSharpScript(string name, Action<DatabaseConnection> action, Version version, bool isUpgradeScript = true, bool supportsTransactions = true)
		: base(name, version, isUpgradeScript, supportsTransactions)
	{
		_action = action;
	}

	public override void Execute(DatabaseConnection dbConnection, string dbSchema, IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, ILogger logger)
	{
		try
		{
			_action(dbConnection);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(logger, "Script '{ScriptName}' action failed", new object[1] { base.Name });
			LoggerExtensions.LogError(logger, "{Exception}", new object[1] { ex.Message });
			LoggerExtensions.LogError(logger, "{ExceptionStackTrace}", new object[1] { ex.StackTrace });
			throw;
		}
	}
}
