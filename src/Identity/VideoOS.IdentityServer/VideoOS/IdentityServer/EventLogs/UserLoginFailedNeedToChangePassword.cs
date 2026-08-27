using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogs;

public class UserLoginFailedNeedToChangePassword : EventLogModel
{
	public UserLoginFailedNeedToChangePassword(VmsUser user)
		: base(user.UserName)
	{
		base.ObjectName = user.UserName;
		base.ObjectId = user.Id;
	}
}
