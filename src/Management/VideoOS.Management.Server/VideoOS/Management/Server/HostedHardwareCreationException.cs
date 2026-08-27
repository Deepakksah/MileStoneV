using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostedHardwareCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60611;

	protected override int ErrorCodeDefault => 60611;

	public HostedHardwareCreationException()
	{
	}

	public HostedHardwareCreationException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60611)]
	public HostedHardwareCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public HostedHardwareCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostedHardwareCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60611, name);
	}
}
