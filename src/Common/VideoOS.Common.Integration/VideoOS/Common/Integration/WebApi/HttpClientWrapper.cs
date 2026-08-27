using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.WebApi;

public class HttpClientWrapper : RefCountedObjectWrapper<HttpClient>
{
	public HttpClientWrapper(HttpClient client)
		: base(client)
	{
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
	{
		try
		{
			return ExecuteCallWithRefCount((HttpClient client) => client.SendAsync(request));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException();
		}
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		try
		{
			return ExecuteCallWithRefCount((HttpClient client) => ((HttpMessageInvoker)client).SendAsync(request, cancellationToken));
		}
		catch (ObjectDeadException)
		{
			throw new CommunicationException();
		}
	}
}
