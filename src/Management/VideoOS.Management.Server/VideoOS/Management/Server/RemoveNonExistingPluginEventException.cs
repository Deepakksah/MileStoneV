using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingPluginEventException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60484;

	protected override int ErrorCodeDefault => 60484;

	public RemoveNonExistingPluginEventException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "IDEventType", name));
	}

	[ExcludeFromCodeCoverage]
	public RemoveNonExistingPluginEventException()
	{
	}

	[ExcludeFromCodeCoverage]
	public RemoveNonExistingPluginEventException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected RemoveNonExistingPluginEventException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60484)]
	public RemoveNonExistingPluginEventException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDEventType"))
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60484, name);
	}
}
