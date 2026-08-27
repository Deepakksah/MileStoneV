using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoOS.IdentityServer.Filters;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Logout;

[AllowAnonymous]
[IdentityPageExceptionFilter]
public class LoggedOutModel(IIdentityServerInteractionService interaction) : PageModel
{
	public LoggedOutViewModel View { get; set; }

	public async Task<IActionResult> OnGetAsync(string logoutId = null)
	{
		LogoutRequest logoutRequest = await interaction.GetLogoutContextAsync(logoutId);
		View = new LoggedOutViewModel
		{
			PostLogoutRedirectUri = logoutRequest.PostLogoutRedirectUri,
			ClientName = logoutRequest.ClientName
		};
		return Page();
	}
}
