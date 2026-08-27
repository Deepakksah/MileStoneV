using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ViewItemCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60218;

	protected override int ErrorCodeDefault => 60218;

	public ViewItemCreationException()
	{
	}

	public ViewItemCreationException(string id)
		: base(FormatMessage(id))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "id", id));
	}

	public ViewItemCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ViewItemCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SqlError(60218)]
	public ViewItemCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("id"))
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60218, name);
	}
}
