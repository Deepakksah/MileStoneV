using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostedHardwareUpdateNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60613;

	protected override int ErrorCodeDefault => 60613;

	public HostedHardwareUpdateNotExistException()
	{
	}

	public HostedHardwareUpdateNotExistException(string pkid)
		: base(FormatMessage(pkid))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Pkid", pkid));
	}

	[SqlError(60613)]
	public HostedHardwareUpdateNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("pkid"))
	{
	}

	public HostedHardwareUpdateNotExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostedHardwareUpdateNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string pkid)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60613, pkid);
	}
}
