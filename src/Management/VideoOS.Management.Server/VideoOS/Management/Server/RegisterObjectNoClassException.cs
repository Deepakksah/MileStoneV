using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RegisterObjectNoClassException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60017;

	protected override int ErrorCodeDefault => 60017;

	public RegisterObjectNoClassException(string classId)
		: base(FormatMessage(classId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "ClassId", classId));
	}

	[SqlError(60017)]
	public RegisterObjectNoClassException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDClass"))
	{
	}

	private static string FormatMessage(string classId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60017, classId);
	}
}
