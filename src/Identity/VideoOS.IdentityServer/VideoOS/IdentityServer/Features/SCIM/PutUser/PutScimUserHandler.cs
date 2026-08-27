using System;
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

namespace VideoOS.IdentityServer.Features.SCIM.PutUser;

public class PutScimUserHandler(VmsUserManager userManager, IRequestUrlBuilder requestUrlBuilder, IProviderInfoService providerInfoService) : IRequestHandler<PutScimUserCommand, Result<ExpandoObject>>
{
	public async Task<Result<ExpandoObject>> Handle(PutScimUserCommand command, CancellationToken cancellationToken)
	{
		Result<VmsUser> result = await PutUserAsync(command);
		return result.IsError ? ((Result<ExpandoObject>)result.Errors.ToList()) : GenerateSuccessResponse(result.Value);
	}

	private async Task<Result<VmsUser>> PutUserAsync(PutScimUserCommand command)
	{
		VmsUser user = await userManager.FindByIdAsync(command.Id, "Logins");
		if (user == null)
		{
			return Result<VmsUser>.Failure(Error.NotFound("NotFound", "User with Id '" + command.Id + "' not found"));
		}
		if (command.Active)
		{
			user.Activate();
		}
		else
		{
			user.Deactivate();
		}
		IdentityResult identityResult = await userManager.UpdateAsync(user);
		return identityResult.Succeeded ? Result<VmsUser>.Success(user) : Result<VmsUser>.Failure(identityResult.Errors.Select((IdentityError e) => Error.FailureWithCode(e.Code, e.Description)).ToList());
	}

	private Result<ExpandoObject> GenerateSuccessResponse(VmsUser user)
	{
		(string ProviderId, string ProviderDisplayName) providerInfo = providerInfoService.GetProviderInfo();
		ScimUser scimUser = new ScimUser(user.Id, user.UserName, user.Logins.First((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId).ProviderKey, user.Active, new ScimUser.ScimMeta(user.CreatedDate, user.ModifiedDate, requestUrlBuilder.BuildBaseUrl()));
		PutScimUserResponse response = new PutScimUserResponse(scimUser);
		ExpandoObject value = ScimUserMapper.GenerateScimUserResponse(response, Array.Empty<string>());
		return Result<ExpandoObject>.Success(value);
	}
}
