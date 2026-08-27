using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

public class QueryChangesCompletedEventArgs : IQueryChangesCompletedEventArgs
{
	IChange[] IQueryChangesCompletedEventArgs.Result { get; }
}
