using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class GetUserInfoCompletedEventArgs : IGetUserInfoCompletedEventArgs
{
	IUserInfo IGetUserInfoCompletedEventArgs.Result { get; }
}
