using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.GrantValidators;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
	private readonly SignInManager<VmsUser> _signInManager;

	private readonly IEventLogService _eventLogService;

	private readonly VmsUserManager _userManager;

	private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

	public ResourceOwnerPasswordValidator(VmsUserManager userManager, SignInManager<VmsUser> signInManager, IEventLogService eventLogService, ILogger<ResourceOwnerPasswordValidator> logger)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_eventLogService = eventLogService;
		_logger = logger;
	}

	public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
	{
		VmsUser user = await _userManager.FindByNameAsync(context.UserName);
		if (user == null)
		{
			_logger.LogDebug("Authentication failed: user not found");
			await _eventLogService.Log(new UserLoginFailedUnknownUser(context.UserName));
			context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, null, AuthenticationCodes.InvalidCredentialsResponse);
			return;
		}
		SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, context.Password, lockoutOnFailure: true);
		if (signInResult.Succeeded)
		{
			if (!user.Active)
			{
				_logger.LogDebug("Authentication failed: account inactive");
				await _eventLogService.Log(new UserLoginFailedUserNotActive(user));
				context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, null, AuthenticationCodes.LockedOutResponse);
			}
			else if (user.ForcePasswordChange)
			{
				_logger.LogDebug("Authentication failed for user id: ***Redacted***, reason: Need password change");
				await _eventLogService.Log(new UserLoginFailedNeedToChangePassword(user));
				context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, null, AuthenticationCodes.ChangePasswordResponse);
			}
			else
			{
				_logger.LogDebug("Credentials validated for user id: ***Redacted***");
				await _eventLogService.Log(new UserLoginSuccess(user));
				context.Result = new GrantValidationResult(user.Id, "pwd");
			}
		}
		else if (signInResult.IsLockedOut)
		{
			_logger.LogDebug("Authentication failed for username: ***Redacted***, reason: locked out");
			context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, null, AuthenticationCodes.LockedOutResponse);
		}
		else
		{
			_logger.LogDebug("Authentication failed for username: ***Redacted***, reason: invalid credentials");
			await _eventLogService.Log(new UserLoginFailed(user));
			context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, null, AuthenticationCodes.InvalidCredentialsResponse);
		}
	}
}
