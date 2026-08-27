using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Owin;

namespace VideoOS.Common.Integration.WebApi;

public class ScopeValidationMiddleware
{
	private readonly ILogger _logger;

	private readonly ScopeValidationOptions _options;

	private readonly Func<IDictionary<string, object>, Task> _next;

	public ScopeValidationMiddleware(Func<IDictionary<string, object>, Task> next, ILogger logger, ScopeValidationOptions options)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (options == null)
		{
			throw new ArgumentNullException("options");
		}
		_logger = logger;
		_options = options;
		_next = next;
	}

	public async Task Invoke(IDictionary<string, object> environment)
	{
		OwinContext val = new OwinContext(environment);
		ClaimsPrincipal user = val.Authentication.User;
		if (user?.Identity == null || !user.Identity.IsAuthenticated)
		{
			await _next(environment);
			return;
		}
		if (string.IsNullOrEmpty(_options.RequiredScope))
		{
			await _next(environment);
			return;
		}
		if (ScopesFound((IOwinContext)(object)val))
		{
			await _next(environment);
			return;
		}
		LoggerExtensions.LogWarning(_logger, "Scope validation failed.", Array.Empty<object>());
		val.Response.StatusCode = 403;
		((IDictionary<string, string[]>)val.Response.Headers).Add("WWW-Authenticate", new string[1] { "Bearer error=\"insufficient_scope\"" });
	}

	private bool ScopesFound(IOwinContext context)
	{
		IEnumerable<Claim> enumerable = context.Authentication.User.FindAll("scope");
		Claim[] source = (enumerable as Claim[]) ?? enumerable.ToArray();
		if (source.Any())
		{
			return source.Any((Claim scope) => _options.RequiredScope.Contains(scope.Value));
		}
		return false;
	}
}
