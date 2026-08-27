using System;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.Utilities.Database;

namespace VideoOS.Corporate.Database;

public class IDPVersionProvider : IDatabaseVersionProvider
{
	private readonly IDatabaseVersionProvider _idpVersionProvider;

	private readonly Version _versionFromMs;

	public bool IsMigrationVersionProvider => true;

	public IDPVersionProvider(Version versionFromMS, IDatabaseVersionProvider idpDatabaseVersionProvider)
	{
		_versionFromMs = versionFromMS;
		_idpVersionProvider = idpDatabaseVersionProvider;
	}

	public Version GetVersion(DatabaseConnection connection)
	{
		Version version = _idpVersionProvider.GetVersion(connection);
		if (!(version == null))
		{
			return version;
		}
		return _versionFromMs;
	}

	public void SetVersion(DatabaseConnection connection, Version version)
	{
		_idpVersionProvider.SetVersion(connection, version);
	}
}
