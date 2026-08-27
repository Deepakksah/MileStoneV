using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ViewRenameException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60216;

	protected override int ErrorCodeDefault => 60216;

	public ViewRenameException()
	{
	}

	public ViewRenameException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	public ViewRenameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60216)]
	public ViewRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	protected ViewRenameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60216, name);
	}
}
