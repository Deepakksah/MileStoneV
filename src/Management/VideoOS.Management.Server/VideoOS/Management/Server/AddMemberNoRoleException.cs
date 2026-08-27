using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class AddMemberNoRoleException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60006;

	protected override int ErrorCodeDefault => 60006;

	public AddMemberNoRoleException(string roleId)
		: base(FormatMessage(roleId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "RoleId", roleId));
	}

	[SqlError(60006)]
	public AddMemberNoRoleException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDRole"))
	{
	}

	private static string FormatMessage(string roleId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60006, roleId);
	}
}
