using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DatabaseConfigUnknownErrorException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60000;

	protected override int ErrorCodeDefault => 60000;

	public DatabaseConfigUnknownErrorException()
		: base(VmoResources.VMO60000)
	{
	}

	public DatabaseConfigUnknownErrorException(string message)
		: base(message)
	{
	}

	public DatabaseConfigUnknownErrorException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DatabaseConfigUnknownErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60000)]
	public DatabaseConfigUnknownErrorException(DatabaseException ex, DatabaseError databaseError)
		: base(FormatErrorMessage(databaseError))
	{
	}

	private static string FormatErrorMessage(DatabaseError databaseError)
	{
		return string.Format(CultureInfo.InvariantCulture, $"{VmoResources.VMO60000}: {Environment.NewLine}, server: {databaseError.Server}, procedure: {databaseError.Procedure}, line number:{databaseError.LineNumber}," + Environment.NewLine + " message: " + databaseError.Message);
	}
}
