using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RegisterObjectNoParentException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60018;

	protected override int ErrorCodeDefault => 60018;

	public RegisterObjectNoParentException(string objectId)
		: base(FormatMessage(objectId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "ObjectId", objectId));
	}

	[SqlError(60018)]
	public RegisterObjectNoParentException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDParentObject"))
	{
	}

	private static string FormatMessage(string objectId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60018, objectId);
	}
}
