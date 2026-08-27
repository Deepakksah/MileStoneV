using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PluginEventCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60481;

	protected override int ErrorCodeDefault => 60481;

	public PluginEventCreationException(string id, string name)
		: base(FormatMessage(id, name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Id", id));
		base.MessageParameters.Add(1, new MessageParameter(1, "Name", name));
	}

	[ExcludeFromCodeCoverage]
	public PluginEventCreationException(string message)
		: base(message)
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventCreationException()
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60481)]
	public PluginEventCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("id"), databaseError.ExtractString("name"))
	{
	}

	private static string FormatMessage(string id, string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60481, id, name);
	}
}
