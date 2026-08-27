using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogs;

public class UserLoginSuccess : EventLogModel
{
	public UserLoginSuccess(VmsUser user)
		: this(user.UserName, user.Id)
	{
	}

	public UserLoginSuccess(string username, string userId)
		: base(username)
	{
		base.ObjectName = username;
		base.ObjectId = userId;
	}
}
