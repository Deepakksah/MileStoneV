using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class GetProductInfoCompletedEventArgs : IGetProductInfoCompletedEventArgs
{
	IProductInfo IGetProductInfoCompletedEventArgs.Result { get; }
}
