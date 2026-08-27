using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class BasicOwnerInfoCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60451;

	protected override int ErrorCodeDefault => 60451;

	public BasicOwnerInfoCreationException()
	{
	}

	public BasicOwnerInfoCreationException(string id)
		: base(FormatMessage(id))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Id", id));
	}

	[SqlError(60451)]
	public BasicOwnerInfoCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Id"))
	{
	}

	public BasicOwnerInfoCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected BasicOwnerInfoCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string id)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60451, id);
	}
}
