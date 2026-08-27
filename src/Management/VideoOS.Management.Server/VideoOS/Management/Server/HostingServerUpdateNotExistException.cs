using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostingServerUpdateNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60603;

	protected override int ErrorCodeDefault => 60603;

	public HostingServerUpdateNotExistException()
	{
	}

	public HostingServerUpdateNotExistException(string pkid)
		: base(FormatMessage(pkid))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Pkid", pkid));
	}

	[SqlError(60603)]
	public HostingServerUpdateNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("pkid"))
	{
	}

	public HostingServerUpdateNotExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostingServerUpdateNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string pkid)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60603, pkid);
	}
}
