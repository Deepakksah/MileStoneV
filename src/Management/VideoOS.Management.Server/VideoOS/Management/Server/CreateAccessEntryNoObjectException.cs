using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class CreateAccessEntryNoObjectException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60021;

	protected override int ErrorCodeDefault => 60021;

	public CreateAccessEntryNoObjectException(string objectId)
		: base(FormatMessage(objectId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "ObjectId", objectId));
	}

	[SqlError(60021)]
	public CreateAccessEntryNoObjectException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDObject"))
	{
	}

	private static string FormatMessage(string objectId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60021, objectId);
	}
}
