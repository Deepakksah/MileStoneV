using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RoleUpdateException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60002;

	protected override int ErrorCodeDefault => 60002;

	public RoleUpdateException(string roleId)
		: base(FormatMessage(roleId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "RoleId", roleId));
	}

	[SqlError(60002)]
	public RoleUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDRole"))
	{
	}

	private static string FormatMessage(string roleId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60002, roleId);
	}
}
