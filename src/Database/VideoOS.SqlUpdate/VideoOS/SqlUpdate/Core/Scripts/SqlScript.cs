using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Scripts;

public class SqlScript : ScriptBase
{
	public bool IsTempScript { get; }

	public IEnumerable<string> Statements { get; set; }

	public SqlScript(string name, IEnumerable<string> statements, Version version, bool isTempScript, bool isUpgradeScript, bool supportsTransactions)
		: base(name, version, isUpgradeScript, supportsTransactions)
	{
		IsTempScript = isTempScript;
		Statements = statements;
	}

	public override void Execute(DatabaseConnection dbConnection, string dbSchema, IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, ILogger logger)
	{
		string text = string.Empty;
		try
		{
			if (Statements == null || Statements.Count() == 0)
			{
				LoggerExtensions.LogDebug(logger, "Script does not contain any statements!", Array.Empty<object>());
				return;
			}
			foreach (string modifiedScriptStatement in GetModifiedScriptStatements(scriptContentReplacements, logger))
			{
				text = modifiedScriptStatement;
				using DatabaseCommand databaseCommand = dbConnection.CreateCommand(modifiedScriptStatement);
				databaseCommand.CommandTimeout = 1800;
				databaseCommand.ExecuteNonQuery();
			}
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(logger, "Script '{ScriptName}' failed on statement: {Statement}", new object[2] { base.Name, text });
			LoggerExtensions.LogError(logger, "{Exception}", new object[1] { ex.Message });
			throw;
		}
	}

	private IEnumerable<string> GetModifiedScriptStatements(IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, ILogger logger)
	{
		if (scriptContentReplacements == null || scriptContentReplacements.Count() == 0)
		{
			return Statements;
		}
		List<string> list = new List<string>();
		foreach (string statement in Statements)
		{
			string text = statement;
			foreach (KeyValuePair<string, string> scriptContentReplacement in scriptContentReplacements)
			{
				if (text.Contains(scriptContentReplacement.Key))
				{
					LoggerExtensions.LogDebug(logger, "Script content has been replaced. Key: {Key}. Value: {Value}", new object[2] { scriptContentReplacement.Key, scriptContentReplacement.Value });
				}
				text = text.Replace(scriptContentReplacement.Key, scriptContentReplacement.Value);
			}
			list.Add(text);
		}
		return list;
	}
}
