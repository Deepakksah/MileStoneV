using System;
using VideoOS.Utilities.Database;

namespace VideoOS.SqlUpdate.Core.Providers;

public interface IDatabaseVersionProvider
{
	bool IsMigrationVersionProvider { get; }

	Version GetVersion(DatabaseConnection connection);

	void SetVersion(DatabaseConnection connection, Version version);
}
