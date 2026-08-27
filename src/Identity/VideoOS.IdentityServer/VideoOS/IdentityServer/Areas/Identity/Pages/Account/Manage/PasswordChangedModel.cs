using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoOS.IdentityServer.Filters;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account.Manage;

[AllowAnonymous]
[IdentityPageExceptionFilter]
public class PasswordChangedModel : PageModel
{
	public IActionResult OnGet()
	{
		return Page();
	}
}
