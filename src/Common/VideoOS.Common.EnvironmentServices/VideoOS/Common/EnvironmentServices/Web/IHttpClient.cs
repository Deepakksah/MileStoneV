using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpClient : IDisposable
{
	TimeSpan Timeout { get; set; }

	Task<IHttpResponseMessage> GetAsync(Uri requestUri);

	Task<IHttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);

	Task<IHttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}
