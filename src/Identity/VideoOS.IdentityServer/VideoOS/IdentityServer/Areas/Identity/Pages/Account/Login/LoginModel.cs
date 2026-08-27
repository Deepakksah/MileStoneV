using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Filters;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.Resources;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Login;

[AllowAnonymous]
[IdentityPageExceptionFilter]
public class LoginModel(SignInManager<VmsUser> signInManager, ILogger<LoginModel> logger, VmsUserManager userManager, IEventLogService eventLogService, IStringLocalizerFactory factory, IIdentityServerInteractionService interaction, IAuthenticationSchemeProvider schemeProvider) : PageModel
{
	private readonly IStringLocalizer _identityLocalizer = factory.Create(typeof(VideoOS.IdentityServer.Resources.IdentityResource));

	[BindProperty]
	public InputModel Input { get; set; }

	public LoginViewModel View { get; set; }

	public string ErrorMessage { get; set; }

	public bool UserSessionIsExpired { get; set; }

	public bool CancelUserSession { get; set; }

	public async Task<IActionResult> OnGetAsync(string returnUrl = null, string username = null)
	{
		CheckLocalSession();
		if (!(await BuildModelAsync(returnUrl)))
		{
			return RedirectToPage("/Error");
		}
		LoginViewModel view = View;
		if (view != null && view.IsExternalLoginOnly)
		{
			return RedirectToPage("/ExternalLogin/Challenge", new
			{
				scheme = View.ExternalLoginScheme,
				returnUrl = returnUrl
			});
		}
		if (returnUrl == null)
		{
			returnUrl = base.Url.Content("~/");
		}
		if (returnUrl.Contains('?'))
		{
			Dictionary<string, StringValues> dictionary = QueryHelpers.ParseQuery(returnUrl.Split("?")[1]);
			if (dictionary.TryGetValue("UserName", out var value) && username == null)
			{
				username = value;
			}
		}
		if (username != null)
		{
			Input = new InputModel
			{
				UserName = username
			};
		}
		return Page();
	}

	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		if (returnUrl == null)
		{
			returnUrl = base.Url.Content("~/");
		}
		if (!base.ModelState.IsValid)
		{
			logger.LogDebug("Empty form.");
			ErrorMessage = _identityLocalizer["ErrorEmptyField"];
			base.ModelState.AddModelError(string.Empty, ErrorMessage);
			await BuildModelAsync(returnUrl);
			return Page();
		}
		VmsUser user = await userManager.FindByNameAsync(Input.UserName);
		if (user == null)
		{
			logger.LogDebug("Incorrect user name or password.");
			await eventLogService.Log(new UserLoginFailedUnknownUser(Input.UserName));
			ErrorMessage = _identityLocalizer["ErrorWrongField"];
			base.ModelState.AddModelError(string.Empty, ErrorMessage);
			await BuildModelAsync(returnUrl);
			return Page();
		}
		Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: true);
		if (!signInResult.Succeeded)
		{
			if (signInResult.IsLockedOut)
			{
				logger.LogDebug("Authentication failed for username: ***Redacted***, reason: locked out.");
				ErrorMessage = _identityLocalizer["ErrorLockAccount"];
			}
			else
			{
				await eventLogService.Log(new UserLoginFailed(user));
				logger.LogDebug("Incorrect user name or password.");
				ErrorMessage = _identityLocalizer["ErrorWrongField"];
			}
			base.ModelState.AddModelError(string.Empty, ErrorMessage);
			await BuildModelAsync(returnUrl);
			return Page();
		}
		if (!user.Active)
		{
			ErrorMessage = _identityLocalizer["ErrorLockAccount"];
			base.ModelState.AddModelError(string.Empty, ErrorMessage);
			await BuildModelAsync(returnUrl);
			return Page();
		}
		await SignOutWithoutRemovingSessionAsync();
		if (user.ForcePasswordChange)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim("name", user.UserName),
				new Claim("sub", user.Id)
			};
			ClaimsIdentity identity = new ClaimsIdentity(claims, "IDP.ForcePasswordChange", "name", null);
			ClaimsPrincipal principal = new ClaimsPrincipal(identity);
			await base.HttpContext.SignInAsync("IDP.ForcePasswordChange", principal, new AuthenticationProperties
			{
				IsPersistent = false,
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5.0),
				AllowRefresh = false
			});
			await eventLogService.Log(new UserLoginSuccess(user));
			return RedirectToPage("/Account/Manage/ChangePassword", new
			{
				ReturnUrl = returnUrl
			});
		}
		await signInManager.SignInAsync(user, isPersistent: false);
		logger.LogDebug("User logged in.");
		await eventLogService.Log(new UserLoginSuccess(user));
		if (base.Url.IsLocalUrl(returnUrl))
		{
			if (returnUrl == base.Url.Content("~/"))
			{
				return RedirectToPage("/Account/Manage/ChangePassword", new
				{
					ReturnUrl = returnUrl
				});
			}
			return LocalRedirect(returnUrl);
		}
		logger.LogDebug("User might have clicked on a malicious link: {ReturnUrl}", returnUrl);
		return LocalRedirect("/Error");
	}

	private async Task<bool> BuildModelAsync(string returnUrl)
	{
		View = new LoginViewModel();
		Input = new InputModel
		{
			ReturnUrl = returnUrl
		};
		AuthorizationRequest context = await interaction.GetAuthorizationContextAsync(returnUrl);
		if (context == null)
		{
			View.IsClientInitiatedFlow = false;
			return true;
		}
		View.IsClientInitiatedFlow = true;
		View.ClientName = context.Client.ClientName;
		if (context.IdP != null)
		{
			if (await schemeProvider.GetSchemeAsync(context.IdP) == null)
			{
				logger.LogError("Requested external identity provider scheme {ContextIdP} not found. The configuration of the external provider is either disabled or does not match the client request", context.IdP);
				return false;
			}
			bool flag = context.IdP == "local";
			View.EnableLocalLogin = flag;
			Input.UserName = context.LoginHint;
			if (!flag)
			{
				View.ExternalProviders = new _003C_003Ez__ReadOnlySingleElementList<LoginViewModel.ExternalProvider>(new LoginViewModel.ExternalProvider
				{
					AuthenticationScheme = context.IdP
				});
			}
			return true;
		}
		LoginViewModel.ExternalProvider[] externalProviders = (from x in await schemeProvider.GetAllSchemesAsync()
			where x.DisplayName != null
			select new LoginViewModel.ExternalProvider
			{
				AuthenticationScheme = x.Name,
				DisplayName = (x.DisplayName ?? x.Name)
			}).ToArray();
		View.ExternalProviders = externalProviders;
		return true;
	}

	private async Task SignOutWithoutRemovingSessionAsync()
	{
		await signInManager.SignOutAsync();
		await base.HttpContext.SignOutAsync("IDP.ForcePasswordChange");
		if (base.User.Identity?.IsAuthenticated ?? false)
		{
			await eventLogService.Log(new UserLogoutSuccess(base.User.Identity.Name, base.User.Claims.First((Claim c) => c.Type.Equals("sub", StringComparison.Ordinal)).Value));
		}
	}

	private void CheckLocalSession()
	{
		UserSessionIsExpired = base.HttpContext.Session.GetSessionBoolValue("ExpireSession");
		if (UserSessionIsExpired)
		{
			base.HttpContext.Session.DeleteSessionValue("ExpireSession");
		}
		CancelUserSession = base.HttpContext.Session.GetSessionBoolValue("CancelSession");
		if (CancelUserSession)
		{
			base.HttpContext.Session.DeleteSessionValue("CancelSession");
		}
	}
}
