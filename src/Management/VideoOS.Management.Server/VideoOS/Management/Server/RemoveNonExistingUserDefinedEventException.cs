using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingUserDefinedEventException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60044;

	protected override int ErrorCodeDefault => 60044;

	public RemoveNonExistingUserDefinedEventException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "IDEventType", name));
	}

	[ExcludeFromCodeCoverage]
	public RemoveNonExistingUserDefinedEventException()
	{
	}

	[ExcludeFromCodeCoverage]
	public RemoveNonExistingUserDefinedEventException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected RemoveNonExistingUserDefinedEventException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60044)]
	public RemoveNonExistingUserDefinedEventException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDEventType"))
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60044, name);
	}
}
