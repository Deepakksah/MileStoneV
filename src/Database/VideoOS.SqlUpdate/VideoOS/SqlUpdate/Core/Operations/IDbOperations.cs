using System.Collections.Generic;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Scripts;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Operations;

public interface IDbOperations
{
	void ExecuteScript(ScriptBase script, IEnumerable<KeyValuePair<string, string>> scriptContentReplacements, DatabaseConnection connection, IDatabaseVersionProvider databaseVersionProvider, string dbSchema);

	void EnableSnapshotIsolation(DatabaseConnection connection);

	bool IsReadCommittedSnapshotIsolationEnabled(DatabaseConnection connection);

	bool DbExists(string connectionString);

	void SetCompatibilityLevel(DatabaseConnection connection, int compatibilityLevel);
}
