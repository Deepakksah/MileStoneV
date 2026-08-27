using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UserDefinedEventCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60014;

	protected override int ErrorCodeDefault => 60014;

	public UserDefinedEventCreationException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventCreationException()
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60014)]
	public UserDefinedEventCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60014, name);
	}
}
