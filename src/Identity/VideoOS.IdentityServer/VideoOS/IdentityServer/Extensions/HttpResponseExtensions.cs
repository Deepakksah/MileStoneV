using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Extensions;

public static class HttpResponseExtensions
{
	public static void SetHttpStatusCodeOverride(this HttpResponse response, int httpStatusCode)
	{
		response.OnStarting(delegate
		{
			response.StatusCode = httpStatusCode;
			return Task.CompletedTask;
		});
	}
}
