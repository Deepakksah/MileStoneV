namespace VideoOS.IdentityServer.EventLogs;

public class UserLoginFailedUnknownUser : EventLogModel
{
	public UserLoginFailedUnknownUser(string userName)
		: base(userName)
	{
		base.ObjectName = userName;
	}
}
