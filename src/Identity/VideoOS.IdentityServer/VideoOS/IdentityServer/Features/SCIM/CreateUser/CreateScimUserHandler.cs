using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Model.Result;
using VideoOS.IdentityServer.Repositories;

namespace VideoOS.IdentityServer.Features.SCIM.CreateUser;

public class CreateScimUserHandler(VmsUserManager userManager, IRequestUrlBuilder requestUrlBuilder, IProviderInfoService providerInfoService) : IRequestHandler<CreateScimUserCommand, Result<ExpandoObject>>
{
	public async Task<Result<ExpandoObject>> Handle(CreateScimUserCommand command, CancellationToken cancellationToken)
	{
		Result<VmsUser> result = await CreateUserAsync(command);
		return result.IsError ? ((Result<ExpandoObject>)result.Errors.ToList()) : GenerateSuccessResponse(result.Value, command);
	}

	private async Task<Result<VmsUser>> CreateUserAsync(CreateScimUserCommand command)
	{
		(string, string) providerInfo = providerInfoService.GetProviderInfo();
		VmsUser user = VmsUser.Create(command.UserName, null, canChangePassword: false, forcePasswordChange: false, null, command.Active);
		UserLoginInfo userLoginInfo = new UserLoginInfo(providerInfo.Item1, command.ExternalId, providerInfo.Item2);
		IdentityResult identityResult = await userManager.CreateUserAsync(user, null, userLoginInfo, new List<Claim>());
		return identityResult.Succeeded ? Result<VmsUser>.Success(user) : Result<VmsUser>.Failure(FromIdentityResult(identityResult));
	}

	private Result<ExpandoObject> GenerateSuccessResponse(VmsUser user, CreateScimUserCommand command)
	{
		string text = requestUrlBuilder.BuildBaseUrl();
		string location = text + "/" + user.Id;
		ScimUser scimUser = new ScimUser(user.Id, user.UserName, command.ExternalId, command.Active, new ScimUser.ScimMeta(user.CreatedDate, user.ModifiedDate, location));
		CreateScimUserResponse response = new CreateScimUserResponse(scimUser);
		ExpandoObject value = ScimUserMapper.GenerateScimUserResponse(response, Array.Empty<string>());
		return Result<ExpandoObject>.Success(value);
	}

	private static List<Error> FromIdentityResult(IdentityResult identityResult)
	{
		return identityResult.Errors.Select(delegate(IdentityError e)
		{
			string code = e.Code;
			if (code == "DuplicateUserName")
			{
				return Error.Conflict("Conflict", e.Description);
			}
			return (code == "LoginAlreadyAssociated") ? Error.Conflict("Conflict", e.Description) : Error.FailureWithCode(e.Code, e.Description);
		}).ToList();
	}
}
