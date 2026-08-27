using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UserDefinedEventRenameException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60015;

	protected override int ErrorCodeDefault => 60015;

	public UserDefinedEventRenameException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventRenameException()
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventRenameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventRenameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60015)]
	public UserDefinedEventRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60015, name);
	}
}
