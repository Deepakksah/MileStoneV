using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostingServerDeleteNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60604;

	protected override int ErrorCodeDefault => 60604;

	public HostingServerDeleteNotExistException()
	{
	}

	public HostingServerDeleteNotExistException(string pkid)
		: base(FormatMessage(pkid))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Pkid", pkid));
	}

	[SqlError(60604)]
	public HostingServerDeleteNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("pkid"))
	{
	}

	public HostingServerDeleteNotExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostingServerDeleteNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string pkid)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60604, pkid);
	}
}
