using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class InvalidParentObjectClassIdException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60019;

	protected override int ErrorCodeDefault => 60019;

	public InvalidParentObjectClassIdException(string objectId, string parentId)
		: base(FormatMessage(objectId, parentId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "ObjectId", objectId));
		base.MessageParameters.Add(1, new MessageParameter(1, "ParentId", parentId));
	}

	[SqlError(60019)]
	public InvalidParentObjectClassIdException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDObject"), databaseError.ExtractString("IDParentObject"))
	{
	}

	private static string FormatMessage(string objectId, string parentId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60019, objectId, parentId);
	}
}
