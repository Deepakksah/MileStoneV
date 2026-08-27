using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogs;

public class UserLoginFailedUserNotActive : EventLogModel
{
	public UserLoginFailedUserNotActive(VmsUser user)
		: base(user.UserName)
	{
		base.ObjectName = user.UserName;
		base.ObjectId = user.Id;
	}
}
