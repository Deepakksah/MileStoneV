using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SecurityActionDoesNotExistException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60013;

	protected override int ErrorCodeDefault => 60013;

	public SecurityActionDoesNotExistException(string objectId)
		: base(FormatMessage(objectId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "ObjectId", objectId));
	}

	[SqlError(60013)]
	public SecurityActionDoesNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDAction"))
	{
	}

	private static string FormatMessage(string objectId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60013, objectId);
	}
}
