using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ViewCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60214;

	protected override int ErrorCodeDefault => 60214;

	public ViewCreationException()
	{
	}

	public ViewCreationException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	public ViewCreationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60214)]
	public ViewCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	protected ViewCreationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60214, name);
	}
}
