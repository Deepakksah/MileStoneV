using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class CheckSmartClientVersionCompletedEventArgs : ICheckSmartClientVersionCompletedEventArgs
{
	ISmartClientVersion ICheckSmartClientVersionCompletedEventArgs.Result { get; }
}
