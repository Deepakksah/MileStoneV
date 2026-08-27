namespace VideoOS.IdentityServer.EventLogs;

public class UserNotAdministrator : EventLogModel
{
	public UserNotAdministrator(string username, string userId)
		: base(username)
	{
		base.ObjectName = username;
		base.ObjectId = userId;
	}
}
