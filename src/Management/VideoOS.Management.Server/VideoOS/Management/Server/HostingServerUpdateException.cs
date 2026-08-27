using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostingServerUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60602;

	protected override int ErrorCodeDefault => 60602;

	public HostingServerUpdateException()
	{
	}

	public HostingServerUpdateException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60602)]
	public HostingServerUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public HostingServerUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostingServerUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60602, name);
	}
}
