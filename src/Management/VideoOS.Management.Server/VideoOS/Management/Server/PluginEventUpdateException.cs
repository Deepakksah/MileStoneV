using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class PluginEventUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60483;

	protected override int ErrorCodeDefault => 60483;

	public PluginEventUpdateException()
		: base(VmoResources.VMO60483)
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventUpdateException(string message)
		: base(message)
	{
	}

	[ExcludeFromCodeCoverage]
	public PluginEventUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PluginEventUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60483)]
	public PluginEventUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
