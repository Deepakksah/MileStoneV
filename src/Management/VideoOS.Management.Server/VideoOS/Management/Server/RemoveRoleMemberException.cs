using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveRoleMemberException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60009;

	protected override int ErrorCodeDefault => 60009;

	public RemoveRoleMemberException(string roleId)
		: base(FormatMessage(roleId))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "RoleId", roleId));
	}

	[SqlError(60009)]
	public RemoveRoleMemberException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Name"))
	{
	}

	private static string FormatMessage(string roleId)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60009, roleId);
	}
}
