using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoOS.IdentityServer.Controllers;

[AllowAnonymous]
[ApiController]
public class ErrorController : BaseController
{
	[Route("/error-api")]
	public IActionResult Error()
	{
		return Problem();
	}
}
