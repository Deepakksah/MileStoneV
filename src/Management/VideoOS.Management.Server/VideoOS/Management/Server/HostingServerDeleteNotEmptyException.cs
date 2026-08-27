using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HostingServerDeleteNotEmptyException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60605;

	protected override int ErrorCodeDefault => 60605;

	public HostingServerDeleteNotEmptyException()
	{
	}

	public HostingServerDeleteNotEmptyException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "name", name));
	}

	[SqlError(60605)]
	public HostingServerDeleteNotEmptyException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public HostingServerDeleteNotEmptyException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HostingServerDeleteNotEmptyException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60605, name);
	}
}
