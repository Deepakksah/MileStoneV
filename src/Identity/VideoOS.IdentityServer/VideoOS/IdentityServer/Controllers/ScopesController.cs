using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScopesController : BaseController
{
	private readonly IConfigurationRepository _configurationRepository;

	public ScopesController(IConfigurationRepository configurationRepository)
	{
		_configurationRepository = configurationRepository;
	}

	[AllowAnonymous]
	[HttpGet]
	[ProducesResponseType(typeof(BaseResponse<IEnumerable<string>>), 200)]
	public async Task<IActionResult> GetScopes()
	{
		return Ok(new BaseResponse(await _configurationRepository.GetAllValidScopesAsync().IgnoreContext()));
	}
}
