using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.EventLogs;

public class UserLoginFailed : EventLogModel
{
	public UserLoginFailed(VmsUser user)
		: base(user.UserName)
	{
		base.ObjectName = user.UserName;
		base.ObjectId = user.Id;
	}
}
