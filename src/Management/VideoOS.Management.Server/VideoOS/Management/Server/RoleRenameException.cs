using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RoleRenameException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60003;

	protected override int ErrorCodeDefault => 60003;

	public RoleRenameException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "RoleName", name));
	}

	[SqlError(60003)]
	public RoleRenameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Name"))
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60003, name);
	}
}
