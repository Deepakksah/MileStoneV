using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class TimeProfileCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60224;

	protected override int ErrorCodeDefault => 60224;

	public TimeProfileCreationException()
	{
	}

	public TimeProfileCreationException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60224)]
	public TimeProfileCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public TimeProfileCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected TimeProfileCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60224, name);
	}
}
