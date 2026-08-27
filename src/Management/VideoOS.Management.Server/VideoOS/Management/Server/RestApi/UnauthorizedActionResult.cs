using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VideoOS.Management.Server.RestApi;

public sealed class UnauthorizedActionResult : IHttpActionResult
{
	async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
	{
		return await System.Threading.Tasks.Task.FromResult<HttpResponseMessage>(new HttpResponseMessage(HttpStatusCode.Unauthorized));
	}
}
