using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration;

public interface ICommunicator : IDisposable
{
	Task<TRet> SendAsync<TObj, TRet>(string endpoint, TObj obj);

	Task<HttpResponseMessage> SendAsync<TObj>(string endpoint, TObj obj);

	Task<TRet> SendAsync<TRet>(string endpoint);

	Task<HttpResponseMessage> SendAsync(string endpoint);

	Task<HttpResponseMessage> SendRawAsync(string endpoint, HttpContent message);

	Task<TObj> ReceiveAsync<TObj>(string endpoint);

	Task<HttpResponseMessage> PatchAsync<TObj>(string endpoint, TObj obj);

	Task<HttpResponseMessage> PatchAsync(string endpoint);

	TRet Send<TObj, TRet>(string endpoint, TObj obj, CancellationToken cancellationToken = default(CancellationToken));

	HttpResponseMessage Send<TObj>(string endpoint, TObj obj, CancellationToken cancellationToken = default(CancellationToken));

	TRet Send<TRet>(string endpoint, CancellationToken cancellationToken = default(CancellationToken));

	HttpResponseMessage Send(string endpoint, CancellationToken cancellationToken = default(CancellationToken));

	TObj Receive<TObj>(string endpoint);

	HttpResponseMessage Patch<TObj>(string endpoint, TObj obj);

	HttpResponseMessage Patch(string endpoint);
}
