using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VideoOS.IdentityServer.Areas.Identity.Pages;

[AllowAnonymous]
public class ErrorModel : PageModel
{
	public bool ShowFooter { get; private set; } = true;

	public string ErrorMessageCode { get; private set; } = "UnexpectedError";

	public void OnGet(HttpStatusCode? statusCode, string errorMessage = null)
	{
		SetError(statusCode, errorMessage);
	}

	public void OnPost(HttpStatusCode? statusCode, string errorMessage = null)
	{
		SetError(statusCode, errorMessage);
	}

	private void SetError(HttpStatusCode? statusCode, string errorMessage)
	{
		(ErrorMessageCode, ShowFooter) = statusCode switch
		{
			HttpStatusCode.NotFound => ("PageNotFound", false), 
			HttpStatusCode.Forbidden => ("PageForbidden", false), 
			null => (errorMessage ?? "UnexpectedError", true), 
			_ => ("UnexpectedError", true), 
		};
	}
}
