using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Filters;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.ExternalLogin;

[IdentityPageExceptionFilter]
[AllowAnonymous]
public class Challenge(IIdentityServerInteractionService interactionService, ILogger<Challenge> logger) : PageModel
{
	public IActionResult OnGet(string scheme, string returnUrl)
	{
		if (string.IsNullOrEmpty(returnUrl))
		{
			returnUrl = base.Url.Content("~/");
		}
		if (!base.Url.IsLocalUrl(returnUrl) && !interactionService.IsValidReturnUrl(returnUrl))
		{
			logger.LogError("Invalid return URL '" + returnUrl + "'");
			return RedirectToPage("/Error");
		}
		AuthenticationProperties properties = new AuthenticationProperties
		{
			RedirectUri = base.Url.Page("/ExternalLogin/Callback"),
			Items = 
			{
				{ "returnUrl", returnUrl },
				{ "scheme", scheme }
			}
		};
		return Challenge(properties, scheme);
	}
}
