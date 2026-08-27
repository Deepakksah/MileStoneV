using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VideoOS.IM.Server.WebApi.Filters;

public class AuthenticationFailureResult : IHttpActionResult
{
	public string ReasonPhrase { get; }

	public HttpRequestMessage RequestMessage { get; }

	public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage requestMessage)
	{
		ReasonPhrase = reasonPhrase;
		RequestMessage = requestMessage;
	}

	public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
	{
		return Task.FromResult<HttpResponseMessage>(Execute());
	}

	private HttpResponseMessage Execute()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		return new HttpResponseMessage(HttpStatusCode.Unauthorized)
		{
			RequestMessage = RequestMessage,
			ReasonPhrase = ReasonPhrase
		};
	}
}
