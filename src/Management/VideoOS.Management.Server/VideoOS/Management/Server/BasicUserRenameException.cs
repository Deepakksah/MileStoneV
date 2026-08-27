using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicUserRenameException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60353;

	protected override int ErrorCodeDefault => 60353;

	public BasicUserRenameException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60353)]
	public BasicUserRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Name"))
	{
	}

	protected BasicUserRenameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60353, name);
	}
}
