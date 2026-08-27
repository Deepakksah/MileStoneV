using System;
using System.Runtime.Versioning;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Filters;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.ExternalLogin;

[SupportedOSPlatform("windows")]
[IdentityPageExceptionFilter]
[AllowAnonymous]
public class WindowsChallengeModel(IIdentityServerInteractionService interactionService, ILogger<WindowsChallengeModel> logger) : PageModel
{
	public async Task<IActionResult> OnGetAsync(string returnUrl)
	{
		if (!OperatingSystem.IsWindows())
		{
			logger.LogError("Windows authentication attempted on a non-Windows system.");
			return LocalRedirect("/Error");
		}
		if (string.IsNullOrEmpty(returnUrl) || (!base.Url.IsLocalUrl(returnUrl) && !interactionService.IsValidReturnUrl(returnUrl)))
		{
			logger.LogError("Invalid return URL '" + returnUrl + "'");
			return LocalRedirect("/Error");
		}
		return await HandleWindowsAuthAsync(returnUrl);
	}

	private async Task<IActionResult> HandleWindowsAuthAsync(string returnUrl)
	{
		ClaimsPrincipal? principal = (await base.HttpContext.AuthenticateAsync("Windows")).Principal;
		WindowsPrincipal val = (WindowsPrincipal)(object)((principal is WindowsPrincipal) ? principal : null);
		if (val == null)
		{
			return Challenge("Windows");
		}
		AuthenticationProperties props = new AuthenticationProperties
		{
			RedirectUri = base.Url.Page("/ExternalLogin/Callback"),
			Items = 
			{
				{ "returnUrl", returnUrl },
				{ "scheme", "Windows" }
			}
		};
		Claim claim = ((ClaimsPrincipal)(object)val).FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid");
		if (claim == null || string.IsNullOrEmpty(claim.Value))
		{
			logger.LogError("Windows authentication succeeded but no SID was provided.");
			return LocalRedirect("/Error");
		}
		string value = claim.Value;
		if (string.IsNullOrEmpty(((ClaimsPrincipal)(object)val).Identity.Name))
		{
			logger.LogError("Windows authentication succeeded but no user name was provided.");
			return LocalRedirect("/Error");
		}
		string name = ((ClaimsPrincipal)(object)val).Identity.Name;
		ClaimsIdentity claimsIdentity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
		claimsIdentity.AddClaim(new Claim("sub", value));
		claimsIdentity.AddClaim(new Claim("name", name));
		await base.HttpContext.SignInAsync(IdentityConstants.ExternalScheme, new ClaimsPrincipal(claimsIdentity), props);
		return Redirect(props.RedirectUri);
	}
}
