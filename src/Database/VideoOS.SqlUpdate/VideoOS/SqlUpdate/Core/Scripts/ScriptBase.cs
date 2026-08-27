using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Scripts;

public abstract class ScriptBase
{
	public string Name { get; }

	public Version Version { get; }

	public bool IsUpgradeScript { get; }

	public bool SupportsTransactions { get; }

	public ScriptBase(string name, Version version, bool isUpgradeScript, bool supportsTransactions)
	{
		Name = name;
		Version = version;
		IsUpgradeScript = isUpgradeScript;
		SupportsTransactions = supportsTransactions;
	}

	public abstract void Execute(DatabaseConnection dbConnection, string dbSchema, IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, ILogger logger);

	public override string ToString()
	{
		return $"{Version} {Name}";
	}
}
