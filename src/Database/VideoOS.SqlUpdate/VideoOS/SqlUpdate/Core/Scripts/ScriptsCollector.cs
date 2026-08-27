using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.SqlUpdate.Core.Common;

namespace VideoOS.SqlUpdate.Core.Scripts;

public class ScriptsCollector : IScriptsCollector
{
	private readonly ILogger _logger;

	private readonly IScriptFactory _scriptFactory;

	private readonly IScriptValidator _scriptValidator;

	private readonly IAssemblyWrapper _assemblyWrapper;

	private readonly string _embeddedScriptsPath;

	public ScriptsCollector(ILogger logger, IScriptFactory scriptFactory, IScriptValidator scriptValidator, IAssemblyWrapper assemblyWrapper, string embeddedScriptsPath)
	{
		_logger = logger;
		_scriptFactory = scriptFactory;
		_scriptValidator = scriptValidator;
		_assemblyWrapper = assemblyWrapper;
		_embeddedScriptsPath = embeddedScriptsPath;
	}

	public IReadOnlyList<ScriptBase> GetGlobalScripts(string scriptType, Version currentDbVersion, bool scriptsAreTemp = false)
	{
		LoggerExtensions.LogDebug(_logger, "Retrieving all {ScriptType} sql files.", new object[1] { scriptType });
		List<SqlScript> list = (from sn in _assemblyWrapper.GetManifestResourceNames()
			where sn.StartsWith(_embeddedScriptsPath + "." + scriptType)
			select _scriptFactory.CreateEmbedded(sn, null, isUpgradeScript: false, scriptType == "Temp")).ToList();
		ValidateScripts(list);
		LoggerExtensions.LogDebug(_logger, "{ScriptCount} sql files found.", new object[1] { list.Count });
		return list.ToList();
	}

	private void ValidateScripts(List<SqlScript> scripts)
	{
		scripts.ForEach(delegate(SqlScript script)
		{
			_scriptValidator.IsValidScript(script);
		});
	}
}
