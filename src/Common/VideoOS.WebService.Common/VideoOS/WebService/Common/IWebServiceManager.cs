using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common;

public interface IWebServiceManager<out TClient, out TChannel> : IRetryChannel<TChannel> where TClient : class where TChannel : class
{
	TClient Instance { get; }
}
