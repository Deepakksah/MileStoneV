using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SetNonExistingStorageAsDefaultException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60101;

	protected override int ErrorCodeDefault => 60101;

	public SetNonExistingStorageAsDefaultException(int sqlErrorNumber)
		: base(FormatMessage(sqlErrorNumber))
	{
	}

	[SqlError(60101)]
	public SetNonExistingStorageAsDefaultException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.Number)
	{
	}

	private static string FormatMessage(int sqlErrorNumber)
	{
		if (sqlErrorNumber.Equals(60101))
		{
			return VmoResources.VMO60101;
		}
		throw new InvalidOperationException("SQL error code not handled");
	}
}
