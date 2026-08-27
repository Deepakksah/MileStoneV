using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.ResourceParameters;

namespace VideoOS.IdentityServer.Controllers;

[Authorize("IdpManagementPolicy")]
[Route("api/[controller]")]
[ApiController]
public class LoginsController : BaseController
{
	private readonly VmsUserManager _userManager;

	private readonly IMapper _mapper;

	public LoginsController(VmsUserManager userManager, IMapper mapper)
	{
		_userManager = userManager;
		_mapper = mapper;
	}

	[HttpGet(Name = "GetLogins")]
	public async Task<ActionResult<LoginDto>> GetLogins([FromQuery] LoginsResourceParameters resourceParameters)
	{
		IEnumerable<IdentityUserLogin<string>> source = await _userManager.GetLoginsAsync(resourceParameters);
		return Ok(_mapper.Map<IEnumerable<LoginDto>>(source));
	}

	[HttpPost]
	public async Task<ActionResult<LoginDto>> AddLogin(LoginForCreationModel loginModel)
	{
		Login login = _mapper.Map<Login>(loginModel);
		VmsUser vmsUser = await _userManager.FindByIdAsync(login.UserId);
		if (vmsUser == null)
		{
			return UserNotFound(loginModel.UserId);
		}
		IdentityResult identityResult = await _userManager.AddLoginAsync(vmsUser, new UserLoginInfo(login.LoginProviderId, login.ProviderKey, login.ProviderDisplayName));
		if (!identityResult.Succeeded)
		{
			return BadRequest(identityResult.Errors);
		}
		LoginDto value = _mapper.Map<LoginDto>(login);
		return CreatedAtRoute("GetLogins", new LoginsResourceParameters
		{
			LoginProviderId = login.LoginProviderId,
			ProviderKey = login.ProviderKey
		}, value);
	}

	[HttpDelete]
	public async Task<ActionResult> DeleteLogin([FromQuery] LoginsResourceParameters resourceParameters)
	{
		if (string.IsNullOrWhiteSpace(resourceParameters.LoginProviderId) || string.IsNullOrWhiteSpace(resourceParameters.ProviderKey))
		{
			return BadRequest(new ApiCodes().NotAllowed("DeleteLogin without path parameters"));
		}
		IdentityUserLogin<string> login = (await _userManager.GetLoginsAsync(resourceParameters)).SingleOrDefault();
		if (login == null)
		{
			return NotFound();
		}
		VmsUser vmsUser = await _userManager.FindByIdAsync(login.UserId);
		if (vmsUser == null)
		{
			return NotFound();
		}
		IdentityResult identityResult = await _userManager.RemoveLoginAsync(vmsUser, login.LoginProvider, login.ProviderKey);
		if (!identityResult.Succeeded)
		{
			return BadRequest(identityResult.Errors);
		}
		return NoContent();
	}
}
