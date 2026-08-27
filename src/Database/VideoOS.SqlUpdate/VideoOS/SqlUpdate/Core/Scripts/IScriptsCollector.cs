using System;
using System.Collections.Generic;

namespace VideoOS.SqlUpdate.Core.Scripts;

public interface IScriptsCollector
{
	IReadOnlyList<ScriptBase> GetGlobalScripts(string scriptType, Version currentDbVersion = null, bool scriptsAreTemp = false);
}
