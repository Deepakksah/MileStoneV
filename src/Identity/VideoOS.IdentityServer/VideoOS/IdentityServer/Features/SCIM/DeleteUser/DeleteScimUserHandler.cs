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

namespace VideoOS.IdentityServer.Features.SCIM.DeleteUser;

public class DeleteScimUserHandler(IDbContextProvider<IdentityDbContextWrapper> dbprovider, IProviderInfoService providerInfoService) : IRequestHandler<DeleteScimUserCommand, Result>
{
	public async Task<Result> Handle(DeleteScimUserCommand query, CancellationToken cancellationToken)
	{
		(string ProviderId, string ProviderDisplayName) providerInfo = providerInfoService.GetProviderInfo();
		VmsUser vmsUser = await dbprovider.Context.Users.Where((VmsUser u) => u.Id == query.Id && u.Logins.Any((IdentityUserLogin<string> x) => x.LoginProvider == providerInfo.ProviderId)).SingleOrDefaultAsync(cancellationToken);
		if (vmsUser == null)
		{
			return Error.NotFound("NotFound", "User with Id '" + query.Id + "' not found");
		}
		dbprovider.Context.Users.Remove(vmsUser);
		await dbprovider.Context.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
