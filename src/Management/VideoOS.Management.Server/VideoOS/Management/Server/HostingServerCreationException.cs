using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostingServerCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60601;

	protected override int ErrorCodeDefault => 60601;

	public HostingServerCreationException()
	{
	}

	public HostingServerCreationException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60601)]
	public HostingServerCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public HostingServerCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostingServerCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60601, name);
	}
}
