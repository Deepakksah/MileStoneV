using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Features.SCIM.Shared;
using VideoOS.IdentityServer.Model.Result;
using VideoOS.IdentityServer.Repositories.Context;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public class GetScimUserListHandler(IDbContextProvider<IdentityDbContextWrapper> databaseProvider, IRequestUrlBuilder requestUrlBuilder, IOptions<GetScimUserSettings> getUserSettings, IProviderInfoService providerInfoService) : IRequestHandler<GetScimUserListQuery, Result<ExpandoObject>>
{
	public async Task<Result<ExpandoObject>> Handle(GetScimUserListQuery query, CancellationToken cancellationToken)
	{
		int responseLimit = getUserSettings.Value.MaxResults;
		(int, int) paginationParameters = GetPaginationParameters(query, responseLimit);
		int startIndex = paginationParameters.Item1;
		int count = paginationParameters.Item2;
		Expression<Func<VmsUser, bool>> expression = FilterExpression.GetFilterExpression(query.Filter, providerInfoService);
		int totalCount = await GetUsersCountAsync(expression);
		List<ScimUser> users = await GetUsersAsync(startIndex, count, expression);
		return (query.StartIndex.HasValue || query.Count.HasValue || totalCount > responseLimit || !query.Filter.IsNullOrEmpty()) ? GeneratePaginationResponse(totalCount, users, startIndex, query.Attributes) : GenerateResponse(totalCount, users, query.Attributes);
	}

	private static (int startIndex, int count) GetPaginationParameters(GetScimUserListQuery query, int responseLimit)
	{
		int item = ((!(query.StartIndex > 0)) ? 1 : query.StartIndex.Value);
		int num = ((!query.Count.HasValue) ? responseLimit : ((!(query.Count < 0)) ? ((!(query.Count > responseLimit)) ? query.Count.Value : responseLimit) : 0));
		int item2 = num;
		return (startIndex: item, count: item2);
	}

	private async Task<int> GetUsersCountAsync(Expression<Func<VmsUser, bool>> filterExpression)
	{
		DbSet<VmsUser> users = databaseProvider.Context.Users;
		(string ProviderId, string ProviderDisplayName) providerInfo = providerInfoService.GetProviderInfo();
		return await (from u in users.Where(filterExpression)
			where u.Logins.Any((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId)
			select u).CountAsync();
	}

	private async Task<List<ScimUser>> GetUsersAsync(int startIndex, int count, Expression<Func<VmsUser, bool>> filterExpression)
	{
		string baseUrl = requestUrlBuilder.BuildBaseUrl();
		(string ProviderId, string ProviderDisplayName) providerInfo = providerInfoService.GetProviderInfo();
		IQueryable<ScimUser> source = from u in (from u in databaseProvider.Context.Users.AsNoTracking()
				where u.Logins.Any((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId)
				select u).Where(filterExpression)
			orderby u.Id
			select new ScimUser(u.Id, u.UserName, u.Logins.First((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId).ProviderKey, u.Active, new ScimUser.ScimMeta(u.CreatedDate, u.ModifiedDate, $"{baseUrl}/{u.Id}"));
		return await source.Skip(startIndex - 1).Take(count).ToListAsync();
	}

	private static Result<ExpandoObject> GenerateResponse(int totalResult, List<ScimUser> users, string[] attributes)
	{
		GetScimUserListResponse response = new GetScimUserListResponse(totalResult, users);
		ExpandoObject value = ScimUserMapper.GenerateScimUserListResponse(response, attributes, new string[1] { "Schemas" });
		return Result<ExpandoObject>.Success(value);
	}

	private static Result<ExpandoObject> GeneratePaginationResponse(int totalResult, List<ScimUser> users, int startIndex, string[] attributes)
	{
		GetScimUserPaginationResponse response = new GetScimUserPaginationResponse(totalResult, users, users.Count, startIndex);
		ExpandoObject value = ScimUserMapper.GenerateScimUserListResponse(response, attributes, new string[1] { "Schemas" });
		return Result<ExpandoObject>.Success(value);
	}
}
