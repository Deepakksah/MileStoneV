using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.Services.CleanupInactiveUsers;

public class InactiveUserCleanupService(IServiceProvider services, ILogger<InactiveUserCleanupService> logger) : IInactiveUserCleanupService
{
	public async Task CleanupInactiveUsersAsync()
	{
		using IServiceScope scope = services.CreateScope();
		ILoginProviderRepository requiredService = scope.ServiceProvider.GetRequiredService<ILoginProviderRepository>();
		List<LoginProvider> list = (await requiredService.GetLoginProviders()).Where((LoginProvider p) => p.EnableInactiveUserDeletion).ToList();
		foreach (LoginProvider item in list)
		{
			await DeleteInactiveUsersForProviderAsync(item);
		}
	}

	private async Task DeleteInactiveUsersForProviderAsync(LoginProvider provider)
	{
		TimeSpan value = TimeSpan.FromDays(provider.InactiveUserDeletionAfterDays);
		DateTime cutOffDate = DateTime.UtcNow.Subtract(value);
		using IServiceScope scope = services.CreateScope();
		IdentityDbContextWrapper dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContextWrapper>();
		List<VmsUser> usersToDelete = await (from u in dbContext.Users
			where u.Logins.Any((IdentityUserLogin<string> l) => l.LoginProvider == ((object)provider.Id).ToString())
			where u.DeactivatedAtUtc < cutOffDate
			select u).ToListAsync();
		if (usersToDelete.Count > 0)
		{
			dbContext.Users.RemoveRange(usersToDelete);
			await dbContext.SaveChangesAsync();
			logger.LogInformation("Deleted {Count} users inactive for {Days} days for provider {ProviderName}", usersToDelete.Count, provider.InactiveUserDeletionAfterDays, provider.DisplayName);
		}
	}
}
