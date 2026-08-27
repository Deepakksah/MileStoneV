using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostedHardwareUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60612;

	protected override int ErrorCodeDefault => 60612;

	public HostedHardwareUpdateException()
	{
	}

	public HostedHardwareUpdateException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60612)]
	public HostedHardwareUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public HostedHardwareUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostedHardwareUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60612, name);
	}
}
