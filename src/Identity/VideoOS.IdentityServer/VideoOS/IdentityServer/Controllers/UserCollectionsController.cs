using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Exceptions;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Repositories;

namespace VideoOS.IdentityServer.Controllers;

[Authorize("IdpManagementPolicy")]
[Route("api/user-collections")]
[ApiController]
public class UserCollectionsController(VmsUserManager userManager, IMapper mapper) : BaseController
{
	[HttpGet(Name = "GetUserCollection")]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetUserCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> ids)
	{
		string[] ids2 = (ids as string[]) ?? ids.ToArray();
		IEnumerable<VmsUser> source = await userManager.FindByIdsAsync(ids2, "Claims", "Logins");
		IEnumerable<UserDto> value = mapper.Map<IEnumerable<UserDto>>(source);
		return Ok(value);
	}

	[HttpPost]
	public async Task<ActionResult<IEnumerable<UserDto>>> CreateUserCollection(IEnumerable<UserForCreationModel> userCollection)
	{
		List<UserForCreationModel> userForCreationModels = userCollection.ToList();
		IdentityResult identityResult = await userManager.CreateUsersAsync(userForCreationModels);
		if (!identityResult.Succeeded)
		{
			throw new ApiErrorException(identityResult.Errors);
		}
		IEnumerable<string> names = userForCreationModels.Select((UserForCreationModel c) => c.UserName);
		IEnumerable<VmsUser> source = await userManager.FindByNamesAsync(names, "Claims", "Logins");
		IEnumerable<UserDto> value = mapper.Map<IEnumerable<UserDto>>(source);
		string ids = string.Join(",", source.Select((VmsUser a) => a.Id));
		return CreatedAtRoute("GetUserCollection", new { ids }, value);
	}
}
