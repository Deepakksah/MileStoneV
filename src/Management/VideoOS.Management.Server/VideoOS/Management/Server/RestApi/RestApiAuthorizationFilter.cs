using System;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.Management.Server.RestApi;

public sealed class RestApiAuthorizationFilter : IAuthenticationFilter, IFilter
{
	private readonly OpenIdTokenHandler _openIdTokenHandler;

	bool IFilter.AllowMultiple => true;

	public RestApiAuthorizationFilter()
	{
		Uri uri = new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/IDP"));
		bool requireHttps = uri.Scheme == Uri.UriSchemeHttps;
		_openIdTokenHandler = new OpenIdTokenHandler(uri, requireHttps);
	}

	System.Threading.Tasks.Task IAuthenticationFilter.AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (context.Request.Headers.Authorization != null && string.Equals("Bearer", context.Request.Headers.Authorization.Scheme, StringComparison.OrdinalIgnoreCase))
		{
			string parameter = context.Request.Headers.Authorization.Parameter;
			context.Principal = _openIdTokenHandler.ValidateToken(parameter, cancellationToken).GetAwaiter().GetResult();
		}
		return System.Threading.Tasks.Task.CompletedTask;
	}

	System.Threading.Tasks.Task IAuthenticationFilter.ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		AuthenticationHeaderValue challenge = new AuthenticationHeaderValue("Bearer");
		context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
		return System.Threading.Tasks.Task.CompletedTask;
	}
}
