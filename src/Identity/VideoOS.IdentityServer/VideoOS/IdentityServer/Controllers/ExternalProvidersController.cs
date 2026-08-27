using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VideoOS.IdentityServer.Controllers;

[AllowAnonymous]
[Route(".well-known/openid-configuration/[Controller]")]
[ApiController]
public class ExternalProvidersController : ControllerBase
{
	private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

	public ExternalProvidersController(IAuthenticationSchemeProvider authenticationSchemeProvider)
	{
		_authenticationSchemeProvider = authenticationSchemeProvider;
	}

	public async Task<IActionResult> Get()
	{
		IEnumerable<AuthenticationScheme> source = (await _authenticationSchemeProvider.GetAllSchemesAsync()).Where((AuthenticationScheme scheme) => scheme.HandlerType != null && scheme.HandlerType.BaseType.IsGenericType && scheme.HandlerType.BaseType.GetGenericTypeDefinition().IsAssignableFrom(typeof(RemoteAuthenticationHandler<>)));
		return Ok(source.Select((AuthenticationScheme scheme) => new
		{
			LoginProviderId = scheme.Name,
			DisplayName = scheme.DisplayName
		}));
	}
}
