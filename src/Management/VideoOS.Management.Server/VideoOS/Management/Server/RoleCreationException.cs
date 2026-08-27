using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RoleCreationException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60001;

	protected override int ErrorCodeDefault => 60001;

	public RoleCreationException(string roleName)
		: base(FormatMessage(roleName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "RoleName", roleName));
	}

	[SqlError(60001)]
	public RoleCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Name"))
	{
	}

	private static string FormatMessage(string roleName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60001, roleName);
	}
}
