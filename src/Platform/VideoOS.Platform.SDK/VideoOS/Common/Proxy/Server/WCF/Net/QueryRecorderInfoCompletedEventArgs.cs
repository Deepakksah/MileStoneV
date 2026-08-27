using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class QueryRecorderInfoCompletedEventArgs : IQueryRecorderInfoCompletedEventArgs
{
	IRecorderInfo IQueryRecorderInfoCompletedEventArgs.Result { get; }
}
