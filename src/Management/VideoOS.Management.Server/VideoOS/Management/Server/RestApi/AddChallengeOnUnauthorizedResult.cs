using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VideoOS.Management.Server.RestApi;

public class AddChallengeOnUnauthorizedResult : IHttpActionResult
{
	public AuthenticationHeaderValue Challenge { get; private set; }

	public IHttpActionResult InnerResult { get; private set; }

	public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
	{
		Challenge = challenge;
		InnerResult = innerResult;
	}

	public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
	{
		HttpResponseMessage val = await InnerResult.ExecuteAsync(cancellationToken);
		if (val.StatusCode == HttpStatusCode.Unauthorized && ((IEnumerable<AuthenticationHeaderValue>)val.Headers.WwwAuthenticate).All((AuthenticationHeaderValue h) => h.Scheme != Challenge.Scheme))
		{
			val.Headers.WwwAuthenticate.Add(Challenge);
		}
		return val;
	}
}
