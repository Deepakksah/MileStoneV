using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.GrantValidators;

public class WindowsCredentialsGrantValidator : IExtensionGrantValidator
{
	private readonly IEventLogService _eventLoggerService;

	private readonly IWindowsAuthenticationProvider _authenticationProvider;

	private readonly HttpContext _httpContext;

	private readonly ILogger<WindowsCredentialsGrantValidator> _logger;

	public string GrantType => "windows_credentials";

	public WindowsCredentialsGrantValidator(IHttpContextAccessor httpContextAccessor, IWindowsAuthenticationProvider authenticationProvider, IEventLogService eventLogService, ILogger<WindowsCredentialsGrantValidator> logger)
	{
		_authenticationProvider = authenticationProvider;
		_eventLoggerService = eventLogService;
		_logger = logger;
		_httpContext = httpContextAccessor.HttpContext;
	}

	public async Task ValidateAsync(ExtensionGrantValidationContext context)
	{
		IAuthenticateResult result = await _authenticationProvider.AuthenticateAsync(_httpContext).IgnoreContext();
		if (!result.IsAuthenticated)
		{
			_httpContext.Response.SetHttpStatusCodeOverride(401);
			context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
			return;
		}
		ClaimsIdentity id = new ClaimsIdentity("Windows");
		id.AddClaim(new Claim("name", result.Name));
		_logger.LogDebug("Authentication success using grant '{grantType}'", GrantType);
		await _eventLoggerService.Log(new UserLoginSuccess(result.Name, result.Sid));
		context.Result = new GrantValidationResult(result.Sid, "wia", id.Claims);
	}
}
