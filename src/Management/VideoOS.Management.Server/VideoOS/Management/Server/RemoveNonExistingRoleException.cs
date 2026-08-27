using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveNonExistingRoleException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60004;

	protected override int ErrorCodeDefault => 60004;

	public RemoveNonExistingRoleException(string roleId)
		: base(FormatMessage(roleId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "RoleId", roleId));
	}

	[SqlError(60004)]
	public RemoveNonExistingRoleException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("IDRole"))
	{
	}

	private static string FormatMessage(string roleId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60004, roleId);
	}
}
