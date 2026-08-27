using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveUsedStorageException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60102;

	public RemoveUsedStorageException(int sqlErrorNumber, string name)
		: base(FormatMessage(sqlErrorNumber, name))
	{
		base.ErrorCode = sqlErrorNumber;
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60102)]
	[SqlError(60103)]
	public RemoveUsedStorageException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.Number, databaseError.ExtractString("Name"))
	{
	}

	private static string FormatMessage(int sqlErrorNumber, string name)
	{
		if (sqlErrorNumber.Equals(60102))
		{
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60102, name);
		}
		if (sqlErrorNumber.Equals(60103))
		{
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60103, name);
		}
		throw new InvalidOperationException("SQL error code not handled");
	}
}
