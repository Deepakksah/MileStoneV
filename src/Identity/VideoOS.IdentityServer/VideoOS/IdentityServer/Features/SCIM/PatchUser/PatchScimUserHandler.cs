using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Model.Result;
using VideoOS.IdentityServer.Repositories;

namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public class PatchScimUserHandler(VmsUserManager userManager, IRequestUrlBuilder requestUrlBuilder, IProviderInfoService providerInfoService) : IRequestHandler<PatchScimUserCommand, Result<ExpandoObject>>
{
	public async Task<Result<ExpandoObject>> Handle(PatchScimUserCommand command, CancellationToken cancellationToken)
	{
		Result<VmsUser> result = await PatchUserAsync(command);
		return result.IsError ? ((Result<ExpandoObject>)result.Errors.ToList()) : GenerateSuccessResponse(result.Value);
	}

	private async Task<Result<VmsUser>> PatchUserAsync(PatchScimUserCommand command)
	{
		VmsUser user = await userManager.FindByIdAsync(command.Id, "Logins");
		if (user == null)
		{
			return Result<VmsUser>.Failure(Error.NotFound("NotFound", "User with Id '" + command.Id + "' not found"));
		}
		ScimOperation[] operations = command.Operations;
		foreach (ScimOperation scimOperation in operations)
		{
			if (scimOperation.Path.Equals("active", StringComparison.OrdinalIgnoreCase))
			{
				if (scimOperation.Value.Value.GetBoolean())
				{
					user.Activate();
				}
				else
				{
					user.Deactivate();
				}
			}
		}
		IdentityResult identityResult = await userManager.UpdateAsync(user);
		return identityResult.Succeeded ? Result<VmsUser>.Success(user) : Result<VmsUser>.Failure(FromIdentityResult(identityResult));
	}

	private Result<ExpandoObject> GenerateSuccessResponse(VmsUser user)
	{
		(string ProviderId, string ProviderDisplayName) providerInfo = providerInfoService.GetProviderInfo();
		ScimUser scimUser = new ScimUser(user.Id, user.UserName, user.Logins.First((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId).ProviderKey, user.Active, new ScimUser.ScimMeta(user.CreatedDate, user.ModifiedDate, requestUrlBuilder.BuildBaseUrl()));
		PatchScimUserResponse response = new PatchScimUserResponse(scimUser);
		ExpandoObject value = ScimUserMapper.GenerateScimUserResponse(response, Array.Empty<string>());
		return Result<ExpandoObject>.Success(value);
	}

	private static List<Error> FromIdentityResult(IdentityResult identityResult)
	{
		return identityResult.Errors.Select((IdentityError e) => Error.FailureWithCode(e.Code, e.Description)).ToList();
	}
}
