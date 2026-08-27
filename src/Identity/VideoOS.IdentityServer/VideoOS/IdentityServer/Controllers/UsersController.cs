using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Repositories;
using VideoOS.IdentityServer.ResourceParameters;

namespace VideoOS.IdentityServer.Controllers;

[Authorize("IdpManagementPolicy")]
[Route("api/[controller]")]
[ApiController]
public class UsersController(VmsUserManager userManager, IMapper mapper) : BaseController
{
	[HttpGet("{id}", Name = "GetUser")]
	public async Task<ActionResult<UserDto>> Get(string id)
	{
		VmsUser vmsUser = await userManager.FindByIdAsync(id, "Claims", "Logins");
		if (vmsUser == null)
		{
			return UserNotFound(id);
		}
		UserDto value = mapper.Map<UserDto>(vmsUser);
		return Ok(value);
	}

	[HttpGet(Name = "GetUsers")]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([ModelBinder(BinderType = typeof(FilterModelBinder))] Expression<Func<VmsUser, bool>> filter, [FromQuery] UsersResourceParameters resourceParameters)
	{
		IEnumerable<VmsUser> source = ((filter == null) ? (await userManager.FindByFilterAsync(resourceParameters, "Claims", "Logins")) : (await userManager.FindByFilterAsync(filter, "Claims", "Logins")));
		IEnumerable<UserDto> value = mapper.Map<IEnumerable<UserDto>>(source);
		return Ok(value);
	}

	[HttpPost(Name = "CreateUser")]
	public async Task<ActionResult<UserDto>> CreateUser(UserForCreationModel createUserModel)
	{
		VmsUser userEntity = VmsUser.Create(createUserModel.UserName, createUserModel.Description, createUserModel.CanChangePassword, createUserModel.ForcePasswordChange, createUserModel.LockoutEnd, createUserModel.Active);
		if (!string.IsNullOrEmpty(createUserModel.Id))
		{
			userEntity.Id = createUserModel.Id;
		}
		IdentityResult identityResult = ((!string.IsNullOrEmpty(createUserModel.Password)) ? (await userManager.CreateAsync(userEntity, createUserModel.Password)) : (await userManager.CreateAsync(userEntity)));
		if (!identityResult.Succeeded)
		{
			return BadRequest(identityResult.Errors);
		}
		UserDto userDto = mapper.Map<UserDto>(userEntity);
		return CreatedAtRoute("GetUser", new
		{
			id = userDto.Id
		}, userDto);
	}

	[HttpPatch("{id}")]
	public async Task<ActionResult<UserDto>> Patch(string id, JsonPatchDocument<UserForUpdateModel> userPatchDocument)
	{
		VmsUser user = await userManager.FindByIdAsync(id, "Claims", "Logins");
		if (user == null)
		{
			return UserNotFound(id);
		}
		if (user.Logins.Count > 0 && (userPatchDocument.Operations.Any((Operation<UserForUpdateModel> s) => s.path.Equals("/Password")) || userPatchDocument.Operations.Any((Operation<UserForUpdateModel> s) => s.path.Equals("/CanChangePassword") || userPatchDocument.Operations.Any((Operation<UserForUpdateModel> operation2) => operation2.path.Equals("/ForcePasswordChange")))))
		{
			return BadRequest(new ApiCodes().NotAllowed("Not allowed to modify password fields for a user with external provider"));
		}
		Operation<UserForUpdateModel> operation = userPatchDocument.Operations.SingleOrDefault((Operation<UserForUpdateModel> o) => o.path.Equals("/Password", StringComparison.OrdinalIgnoreCase));
		if (operation != null && operation.value != null)
		{
			string password = operation.value.ToString();
			foreach (IPasswordValidator<VmsUser> passwordValidator in userManager.PasswordValidators)
			{
				IdentityResult identityResult = await passwordValidator.ValidateAsync(userManager, user, password);
				if (!identityResult.Succeeded)
				{
					return BadRequest(identityResult.Errors);
				}
			}
		}
		UserForUpdateModel userForUpdateModel = new UserForUpdateModel
		{
			Active = user.Active,
			CanChangePassword = user.CanChangePassword,
			Description = user.Description,
			ForcePasswordChange = user.ForcePasswordChange,
			LockoutEnd = user.LockoutEnd
		};
		userPatchDocument.ApplyTo(userForUpdateModel, base.ModelState);
		if (!base.ModelState.IsValid)
		{
			return BadRequest(base.ModelState);
		}
		if (userPatchDocument.Operations.Any((Operation<UserForUpdateModel> op) => op.path.Equals("/Active")))
		{
			if (userForUpdateModel.Active)
			{
				user.Activate();
			}
			else
			{
				user.Deactivate();
			}
		}
		if (userPatchDocument.Operations.Any((Operation<UserForUpdateModel> op) => op.path.Equals("/Password")) && userForUpdateModel.Password != null)
		{
			user.PasswordHash = userManager.PasswordHasher.HashPassword(user, userForUpdateModel.Password);
		}
		if (userPatchDocument.Operations.Any((Operation<UserForUpdateModel> op) => op.path.Equals("/LockoutEnd")))
		{
			user.SetLockoutEnd(userForUpdateModel.LockoutEnd);
		}
		user.CanChangePassword = userForUpdateModel.CanChangePassword;
		user.Description = userForUpdateModel.Description;
		user.ForcePasswordChange = userForUpdateModel.ForcePasswordChange;
		IdentityResult identityResult2 = await userManager.UpdateAsync(user);
		if (!identityResult2.Succeeded)
		{
			return BadRequest(identityResult2.Errors);
		}
		UserDto value = mapper.Map<UserDto>(user);
		return Ok(value);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(string id)
	{
		VmsUser vmsUser = await userManager.FindByIdAsync(id);
		if (vmsUser == null)
		{
			return UserNotFound(id);
		}
		IdentityResult identityResult = await userManager.DeleteAsync(vmsUser);
		if (!identityResult.Succeeded)
		{
			return BadRequest(identityResult.Errors);
		}
		return NoContent();
	}
}
