using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.WebApi;

public class ServiceUnavailableStatusCodeResult : ContentStatusCodeResult
{
	private int _retryAfter;

	public ServiceUnavailableStatusCodeResult(int retryAfter, HttpRequestMessage request)
		: this(retryAfter, request, null)
	{
	}

	public ServiceUnavailableStatusCodeResult(int retryAfter, HttpRequestMessage request, object content)
		: base(HttpStatusCode.ServiceUnavailable, request, content)
	{
		_retryAfter = retryAfter;
	}

	public override async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
	{
		HttpResponseMessage obj = await base.ExecuteAsync(cancellationToken);
		((HttpHeaders)obj.Headers).Add("Retry-After", _retryAfter.ToString());
		return obj;
	}
}
