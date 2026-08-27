using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PluginEventRenameException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60482;

	protected override int ErrorCodeDefault => 60482;

	public PluginEventRenameException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[ExcludeFromCodeCoverage]
	public PluginEventRenameException()
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventRenameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventRenameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60482)]
	public PluginEventRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60482, name);
	}
}
