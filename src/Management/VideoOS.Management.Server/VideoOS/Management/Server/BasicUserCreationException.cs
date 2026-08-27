using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicUserCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60351;

	protected override int ErrorCodeDefault => 60351;

	public BasicUserCreationException()
	{
	}

	public BasicUserCreationException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60351)]
	public BasicUserCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public BasicUserCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected BasicUserCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60351, name);
	}
}
