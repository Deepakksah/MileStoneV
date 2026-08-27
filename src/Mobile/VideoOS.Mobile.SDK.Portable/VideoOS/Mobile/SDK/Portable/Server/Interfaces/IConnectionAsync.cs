using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IConnectionAsync
{
	IAsyncResult Connect(ConnectParams connectParams = null, OnResponse<ConnectResponse> onSuccess = null, OnResponse<ConnectResponse> onFail = null);

	IAsyncResult LogIn(string username, string password, string clientType, UserType loginType = UserType.Unknown, LoginParams loginParams = null, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult LiveMessage(OnResponse<LiveMessageResponse> onSuccess = null, OnResponse<LiveMessageResponse> onFail = null);
}
