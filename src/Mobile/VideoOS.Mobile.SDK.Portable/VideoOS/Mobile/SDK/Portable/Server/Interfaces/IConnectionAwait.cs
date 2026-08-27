using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IConnectionAwait
{
	Task<ConnectResponse> ConnectAsync(ConnectParams connectParams, TimeSpan timeout);

	Task<BaseCommandResponse> LogInAsync(string username, string password, string clientType, LoginParams loginParams, TimeSpan timeout, UserType loginType = UserType.Unknown);

	Task<BaseCommandResponse> LogInAsync(LoginSettingsParams loginSettings, LoginParams loginParams, TimeSpan timeout);

	Task<LiveMessageResponse> LiveMessageAsync(TimeSpan timeout);
}
