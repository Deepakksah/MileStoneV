using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IConnectionSync
{
	ConnectResponse Connect(ConnectParams connectParams, TimeSpan timeout);

	BaseCommandResponse LogIn(string username, string password, string clientType, TimeSpan timeout, UserType loginType, LoginParams loginParams);

	BaseCommandResponse LogIn(LoginSettingsParams loginSettings, LoginParams loginParams, TimeSpan timeout);

	LiveMessageResponse LiveMessage(TimeSpan timeout);
}
