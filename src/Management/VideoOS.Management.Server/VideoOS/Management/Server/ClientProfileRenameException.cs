using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ClientProfileRenameException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60703;

	protected override int ErrorCodeDefault => 60703;

	public ClientProfileRenameException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60703)]
	public ClientProfileRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"))
	{
	}

	protected ClientProfileRenameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60703, name);
	}
}
