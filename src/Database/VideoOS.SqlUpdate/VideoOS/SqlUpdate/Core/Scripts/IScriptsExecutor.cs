using System.Collections.Generic;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Scripts;

public interface IScriptsExecutor
{
	void Execute(DatabaseConnection connection, IEnumerable<ScriptBase> scripts, IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, IDatabaseVersionProvider databaseVersionProvider, string dbSchema);
}
