using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveRoleMemberNotMemberException : SecuritySubsystemException
{
	public const int SqlErrorCode = 60011;

	protected override int ErrorCodeDefault => 60011;

	public RemoveRoleMemberNotMemberException(string memberSid)
		: base(FormatString(memberSid))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "MemberSid", memberSid));
	}

	[SqlError(60011)]
	public RemoveRoleMemberNotMemberException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("MemberSid"))
	{
	}

	private static string FormatString(string memberSid)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60011, memberSid);
	}
}
