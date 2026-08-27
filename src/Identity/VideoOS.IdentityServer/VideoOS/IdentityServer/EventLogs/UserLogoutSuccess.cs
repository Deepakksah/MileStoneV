namespace VideoOS.IdentityServer.EventLogs;

public class UserLogoutSuccess : EventLogModel
{
	public UserLogoutSuccess(string userName, string userId)
		: base(userName)
	{
		base.ObjectName = userName;
		base.ObjectId = userId;
	}
}
