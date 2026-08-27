using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class UserDefinedEventUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60099;

	protected override int ErrorCodeDefault => 60099;

	public UserDefinedEventUpdateException()
		: base(VmoResources.VMO60099)
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventUpdateException(string message)
		: base(message)
	{
	}

	[ExcludeFromCodeCoverage]
	public UserDefinedEventUpdateException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[ExcludeFromCodeCoverage]
	protected UserDefinedEventUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60099)]
	public UserDefinedEventUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
