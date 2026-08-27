using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogInfrastructure;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Filters;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.ExternalLogin;

[IdentityPageExceptionFilter]
[AllowAnonymous]
public class Callback(ILogger<Callback> logger, VmsUserManager userManager, IAuthenticationSchemeProvider schemes, IUserProvisioner userProvisioning, ILoginProviderRepository providerRepository, IEventLogService eventLogService) : PageModel
{
	public async Task<IActionResult> OnGetAsync()
	{
		ExternalLoginInfo info = await GetExternalLoginInfoAsync();
		if (info == null)
		{
			return LocalRedirect("/Error");
		}
		LogExternalClaims(info);
		string userId;
		string username;
		IActionResult actionResult;
		if (info.LoginProvider == "Windows")
		{
			(string, string, IActionResult) windowsUserIdentity = GetWindowsUserIdentity(info);
			(userId, username, actionResult) = windowsUserIdentity;
			_ = (windowsUserIdentity.Item1, windowsUserIdentity.Item2, windowsUserIdentity.Item3);
		}
		else
		{
			(string, string, IActionResult) tuple2 = await GetExternalUserIdentity(info);
			(userId, username, actionResult) = tuple2;
			_ = (tuple2.Item1, tuple2.Item2, tuple2.Item3);
		}
		if (actionResult != null)
		{
			return actionResult;
		}
		ClaimsPrincipal principal = BuildPrincipal(userId, username, info);
		await HandleExistingSession(userId);
		await SignInUser(principal);
		await eventLogService.Log(new UserLoginSuccess(username, userId));
		return Redirect(GetReturnUrl(info));
	}

	private async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
	{
		AuthenticateResult auth = await base.HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		IDictionary<string, string> dictionary = auth.Properties?.Items;
		if (auth.Principal == null)
		{
			logger.LogError("Authentication callback failed: Principal is null");
			return null;
		}
		if (dictionary == null)
		{
			logger.LogError("Authentication callback failed: Items dictionary is null");
			return null;
		}
		if (!dictionary.TryGetValue("scheme", out var scheme))
		{
			logger.LogError("Authentication callback failed: Missing scheme in items dictionary");
			return null;
		}
		string sub;
		if (!(scheme == "Windows"))
		{
			if (scheme == null || !Guid.TryParse(scheme, out var providerId))
			{
				logger.LogError("Authentication callback failed: Invalid provider ID format. Scheme: {Scheme}", scheme);
				return null;
			}
			LoginProvider loginProvider = await providerRepository.GetLoginProvider(providerId);
			if (loginProvider == null)
			{
				logger.LogError("Authentication callback failed: Login provider not found for ID: {ProviderId}", providerId);
				return null;
			}
			sub = auth.Principal.FindFirst(loginProvider.MappingClaimType)?.Value;
			logger.LogDebug("Using custom provider claim type: {ClaimType}", loginProvider.MappingClaimType);
		}
		else
		{
			sub = auth.Principal.FindFirst("sub")?.Value;
		}
		if (sub == null)
		{
			logger.LogError("Authentication callback failed: No subject claim found in {AuthType} login", (scheme == "Windows") ? "Windows" : "External");
			return null;
		}
		string displayName = (await schemes.GetSchemeAsync(scheme))?.DisplayName ?? scheme;
		return new ExternalLoginInfo(auth.Principal, scheme, sub, displayName)
		{
			AuthenticationTokens = auth.Properties.GetTokens(),
			AuthenticationProperties = auth.Properties
		};
	}

	private async Task SignInUser(ClaimsPrincipal user)
	{
		await base.HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, user);
		await base.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
	}

	private async Task HandleExistingSession(string newUserId)
	{
		string text = base.User.FindFirstValue("sub");
		if (text == null || !(text != newUserId))
		{
			return;
		}
		await base.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
		if (base.User.Identity?.IsAuthenticated ?? false)
		{
			await eventLogService.Log(new UserLogoutSuccess(base.User.Identity.Name, base.User.Claims.First((Claim c) => c.Type.Equals("sub", StringComparison.Ordinal)).Value));
		}
	}

	private ClaimsPrincipal BuildPrincipal(string userId, string username, ExternalLoginInfo info)
	{
		List<Claim> list = new List<Claim>
		{
			new Claim("sub", userId),
			new Claim("name", username)
		};
		string value = info.AuthenticationProperties?.Items["scheme"];
		if (!string.IsNullOrWhiteSpace(value))
		{
			list.Add(new Claim("idp", value));
		}
		List<Claim> sessionClaims = GetSessionClaims(info);
		list.AddRange(sessionClaims);
		string[] authenticationMethods = GetAuthenticationMethods(info);
		string[] array = authenticationMethods;
		foreach (string value2 in array)
		{
			list.Add(new Claim("amr", value2));
		}
		ClaimsIdentity identity = new ClaimsIdentity(list.Distinct(new ClaimComparer()), IdentityConstants.ApplicationScheme, "name", null);
		return new ClaimsPrincipal(identity);
	}

	private List<Claim> GetSessionClaims(ExternalLoginInfo info)
	{
		List<Claim> list = new List<Claim>();
		Claim claim = info.Principal.Claims.FirstOrDefault((Claim x) => x.Type == "sid");
		if (claim != null)
		{
			list.Add(new Claim("sid", claim.Value));
		}
		return list;
	}

	private string[] GetAuthenticationMethods(ExternalLoginInfo info)
	{
		if (info.LoginProvider == "Windows")
		{
			return new string[1] { "wia" };
		}
		return new string[1] { "external" };
	}

	private async Task<(string userId, string username, IActionResult redirect)> GetExternalUserIdentity(ExternalLoginInfo info)
	{
		List<Claim> list = info.Principal.Claims.ToList();
		list.Add(new Claim("vms_provider_key", info.ProviderKey));
		VmsUser vmsUser = await CreateOrUpdateExternalUser(info, list);
		if (userManager.IsUserLockedOut(vmsUser))
		{
			await base.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			return (userId: null, username: null, redirect: RedirectToPage("/Error", new
			{
				errorMessage = "ErrorLockAccount"
			}));
		}
		return (userId: vmsUser.Id, username: vmsUser.UserName, redirect: null);
	}

	private async Task<VmsUser> CreateOrUpdateExternalUser(ExternalLoginInfo info, List<Claim> claims)
	{
		return await base.HttpContext.WithSystemAuditContext(async () => await userProvisioning.CreateOrUpdateUser(info, claims));
	}

	private (string userId, string username, IActionResult redirect) GetWindowsUserIdentity(ExternalLoginInfo info)
	{
		string windowsIdentifier = GetWindowsIdentifier(info);
		if (string.IsNullOrEmpty(windowsIdentifier))
		{
			logger.LogError("No valid Windows identifier found");
			return (userId: null, username: null, redirect: LocalRedirect("/Error"));
		}
		string windowsUsername = GetWindowsUsername(info.Principal);
		if (string.IsNullOrEmpty(windowsUsername))
		{
			logger.LogError("No valid Windows username found");
			return (userId: null, username: null, redirect: LocalRedirect("/Error"));
		}
		return (userId: windowsIdentifier, username: windowsUsername, redirect: null);
	}

	private string GetReturnUrl(ExternalLoginInfo info)
	{
		AuthenticationProperties? authenticationProperties = info.AuthenticationProperties;
		if (authenticationProperties == null || !authenticationProperties.Items.TryGetValue("returnUrl", out string value))
		{
			return base.Url.Content("~/");
		}
		return value;
	}

	private void LogExternalClaims(ExternalLoginInfo info)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			logger.LogDebug("External claims: {@claims}", info.Principal.Claims.Select((Claim c) => c.Type + ": " + c.Value));
		}
	}

	private static string GetWindowsUsername(ClaimsPrincipal principal)
	{
		return (from c in principal.Claims.Where(delegate(Claim c)
			{
				string type = c.Type;
				return (type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" || type == "name") ? true : false;
			})
			select c.Value).FirstOrDefault((string v) => !string.IsNullOrEmpty(v));
	}

	private static string GetWindowsIdentifier(ExternalLoginInfo info)
	{
		return info.Principal.Claims.FirstOrDefault((Claim c) => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid")?.Value ?? info.ProviderKey;
	}
}
