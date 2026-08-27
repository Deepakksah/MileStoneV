using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class StorageRetentionTimeException : AdministrationSubsystemException
{
	private int sqlErrorNumber;

	protected override int ErrorCodeDefault => sqlErrorNumber;

	public StorageRetentionTimeException()
	{
	}

	public StorageRetentionTimeException(string message)
		: base(message)
	{
	}

	public StorageRetentionTimeException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected StorageRetentionTimeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60145)]
	[SqlError(60146)]
	[SqlError(60147)]
	[SqlError(60148)]
	[SqlError(60149)]
	[SqlError(60150)]
	public StorageRetentionTimeException(DatabaseException ex, DatabaseError databaseError)
		: this(FormatMessage(databaseError.Number))
	{
		sqlErrorNumber = databaseError.Number;
	}

	private static string FormatMessage(int sqlErrorNumber)
	{
		return sqlErrorNumber switch
		{
			60145 => VmoResources.VMO60145, 
			60146 => VmoResources.VMO60146, 
			60147 => VmoResources.VMO60147, 
			60148 => VmoResources.VMO60148, 
			60149 => VmoResources.VMO60149, 
			60150 => VmoResources.VMO60150, 
			_ => throw new NotSupportedException("The specified error number is not supported for this exception"), 
		};
	}
}
