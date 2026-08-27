using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.GrantValidators;

public class WinAuthGrantValidator : IExtensionGrantValidator
{
	private readonly HttpContext _httpContext;

	private readonly IWindowsAuthenticationProvider _authenticationProvider;

	private readonly ILogger _logger;

	private readonly IEventLogService _eventLogService;

	private readonly IAdminIdentityRepository _repository;

	public string GrantType => "windows_auth";

	public WinAuthGrantValidator(IHttpContextAccessor httpContextAccessor, IWindowsAuthenticationProvider authenticationProvider, IEventLogService eventLogService, ILogger<WinAuthGrantValidator> logger, IAdminIdentityRepository repository)
	{
		_httpContext = httpContextAccessor.HttpContext;
		_authenticationProvider = authenticationProvider;
		_eventLogService = eventLogService;
		_logger = logger;
		_repository = repository;
	}

	public async Task ValidateAsync(ExtensionGrantValidationContext context)
	{
		_ = 3;
		try
		{
			IAuthenticateResult result = await _authenticationProvider.AuthenticateAsync(_httpContext).IgnoreContext();
			if (!result.IsAuthenticated)
			{
				_httpContext.Response.SetHttpStatusCodeOverride(401);
				context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
			}
			else if (await IsAdmin(result))
			{
				ClaimsIdentity id = new ClaimsIdentity("Windows");
				id.AddClaim(new Claim("name", result.Name));
				_logger.LogDebug("Authentication success using grant '{grantType}'", GrantType);
				await _eventLogService.Log(new UserLoginSuccess(result.Name, result.Sid));
				context.Result = new GrantValidationResult(result.Sid, "wia", id.Claims);
			}
			else
			{
				_logger.LogDebug("Authentication failed, reason: User is not administrator");
				await _eventLogService.Log(new UserNotAdministrator(result.Name, result.Sid));
				_httpContext.Response.SetHttpStatusCodeOverride(403);
				context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "An error occurred during grant validation.");
			_httpContext.Response.SetHttpStatusCodeOverride(500);
		}
	}

	private async Task<bool> IsAdmin(IAuthenticateResult authenticateResult)
	{
		if (authenticateResult.IsInRole((WindowsBuiltInRole)544))
		{
			return true;
		}
		List<string> ids = new List<string>(authenticateResult.Groups) { authenticateResult.Sid };
		return (await _repository.GetAdminIdentitiesAsync(ids)).Any();
	}
}
