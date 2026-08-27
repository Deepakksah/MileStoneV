using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class AddMemberIdentityAlreadyMemberException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60007;

	protected override int ErrorCodeDefault => 60007;

	public AddMemberIdentityAlreadyMemberException(string roleName, string identityName)
		: base(FormatMessage(roleName, identityName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "RoleName", roleName));
		base.MessageParameters.Add(1, new MessageParameter(1, "IdentityName", identityName));
	}

	[SqlError(60007)]
	public AddMemberIdentityAlreadyMemberException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("RoleName"), databaseError.ExtractString("MemberName"))
	{
	}

	private static string FormatMessage(string roleName, string identityName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60007, roleName, identityName);
	}
}
