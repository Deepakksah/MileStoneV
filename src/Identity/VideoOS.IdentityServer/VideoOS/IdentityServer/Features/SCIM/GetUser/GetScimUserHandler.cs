using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Model.Result;
using VideoOS.IdentityServer.Repositories.Context;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public class GetScimUserHandler(IDbContextProvider<IdentityDbContextWrapper> dbprovider, IRequestUrlBuilder requestUrlBuilder, IProviderInfoService providerInfoService) : IRequestHandler<GetScimUserQuery, Result<ExpandoObject>>
{
	public async Task<Result<ExpandoObject>> Handle(GetScimUserQuery query, CancellationToken cancellationToken)
	{
		ScimUser scimUser = await GetUserAsync(query);
		return (scimUser == null) ? ((Result<ExpandoObject>)Error.NotFound("NotFound", "User with Id '" + query.Id + "' not found")) : GenerateSuccessResponse(scimUser, query);
	}

	private async Task<ScimUser> GetUserAsync(GetScimUserQuery query)
	{
		string location = requestUrlBuilder.BuildBaseUrl();
		(string ProviderId, string ProviderDisplayName) providerInfo = providerInfoService.GetProviderInfo();
		ScimUser scimUser = await (from u in dbprovider.Context.Users
			where u.Id == query.Id && u.Logins.Any((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId)
			select new ScimUser(u.Id, u.UserName, u.Logins.First((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId).ProviderKey, u.Active, new ScimUser.ScimMeta(u.CreatedDate, u.ModifiedDate, $"{location}"))).SingleOrDefaultAsync();
		if (scimUser == null)
		{
			return null;
		}
		return scimUser;
	}

	private static Result<ExpandoObject> GenerateSuccessResponse(ScimUser scimUser, GetScimUserQuery query)
	{
		GetScimUserResponse response = new GetScimUserResponse(scimUser);
		ExpandoObject value = ScimUserMapper.GenerateScimUserResponse(response, query.Attributes);
		return Result<ExpandoObject>.Success(value);
	}
}
