using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ClientProfileCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60701;

	protected override int ErrorCodeDefault => 60701;

	public ClientProfileCreationException()
	{
	}

	public ClientProfileCreationException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60701)]
	public ClientProfileCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	public ClientProfileCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ClientProfileCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60701, name);
	}
}
