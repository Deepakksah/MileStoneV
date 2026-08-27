using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogs;
using VideoOS.IdentityServer.Filters;
using VideoOS.IdentityServer.Services.Interfaces;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Logout;

[AllowAnonymous]
[IdentityPageExceptionFilter]
public class LogoutModel(SignInManager<VmsUser> signInManager, IIdentityServerInteractionService interaction, IEventLogService eventLogService) : PageModel
{
	[BindProperty]
	public string LogoutId { get; set; }

	public async Task<IActionResult> OnGetAsync(string logoutId = null)
	{
		LogoutId = logoutId;
		IIdentity? identity = base.User.Identity;
		if (identity == null || !identity.IsAuthenticated)
		{
			return await OnPostAsync();
		}
		LogoutRequest logoutRequest = await interaction.GetLogoutContextAsync(logoutId);
		if (logoutId != null && !logoutRequest.ShowSignoutPrompt)
		{
			return await OnPostAsync();
		}
		return Page();
	}

	public async Task<IActionResult> OnPostAsync()
	{
		IIdentity identity = base.User.Identity;
		if (identity != null && identity.IsAuthenticated)
		{
			string logoutId = LogoutId;
			string text = logoutId;
			if (text == null)
			{
				LogoutId = await interaction.CreateLogoutContextAsync();
			}
			await signInManager.SignOutAsync();
			await eventLogService.Log(new UserLogoutSuccess(base.User.Identity.Name, base.User.Claims.First((Claim c) => c.Type.Equals("sub", StringComparison.Ordinal)).Value));
		}
		return RedirectToPage("/Account/Logout/LoggedOut", new
		{
			logoutId = LogoutId
		});
	}
}
