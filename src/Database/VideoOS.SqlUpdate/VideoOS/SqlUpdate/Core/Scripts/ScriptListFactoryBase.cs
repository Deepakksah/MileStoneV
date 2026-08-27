using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Scripts;

public abstract class ScriptListFactoryBase
{
	private readonly IScriptFactory _scriptFactory;

	private readonly string _embeddedPath;

	public AssemblyWrapper AssemblyWrapper { get; }

	public string EmbeddedPath => _embeddedPath;

	public ScriptListFactoryBase(ILogger logger, string assemblyName, string embeddedPath)
	{
		AssemblyWrapper = new AssemblyWrapper(logger, assemblyName);
		_scriptFactory = new ScriptFactory(AssemblyWrapper);
		_embeddedPath = embeddedPath;
	}

	protected SqlScript CreateSqlScript(Version version, string scriptName)
	{
		return CreateSqlScript(version, scriptName, isUpgradeScript: true, supportsTransactions: true);
	}

	protected SqlScript CreateSqlScriptNonTransaction(Version version, string scriptName)
	{
		return CreateSqlScript(version, scriptName, isUpgradeScript: true, supportsTransactions: false);
	}

	protected static CSharpScript CreateCSharpScript(Version version, string name, Action<DatabaseConnection> action)
	{
		return new CSharpScript(name, action, version);
	}

	protected SqlScript CreateBaselineScript(string scriptName)
	{
		return CreateSqlScript(new Version(), scriptName, isUpgradeScript: false, supportsTransactions: true);
	}

	public virtual IReadOnlyList<ScriptBase> GetUpgradeScripts()
	{
		throw new NotImplementedException();
	}

	public virtual IReadOnlyList<ScriptBase> GetBaselineScripts()
	{
		throw new NotImplementedException();
	}

	private SqlScript CreateSqlScript(Version version, string scriptName, bool isUpgradeScript, bool supportsTransactions)
	{
		return _scriptFactory.CreateEmbedded(_embeddedPath + "." + scriptName + ".sql", version, isUpgradeScript, isTempScript: false, supportsTransactions);
	}
}
