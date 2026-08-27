using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VideoOS.IdentityServer.Filters;

namespace VideoOS.IdentityServer.Areas.Identity.Pages.Account;

[AllowAnonymous]
[IdentityPageExceptionFilter]
public class LoginSuccessModel : PageModel
{
	public IActionResult OnGet()
	{
		return Page();
	}
}
