using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class GetSmartClientVersionCompletedEventArgs : IGetSmartClientVersionCompletedEventArgs
{
	ISmartClientVersion IGetSmartClientVersionCompletedEventArgs.Result { get; }
}
