using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VideoOS.IdentityServer.Services.CleanupInactiveUsers;

public class InactiveUserCleanupBackgroundService(IServiceScopeFactory scopeFactory, ILogger<InactiveUserCleanupBackgroundService> logger) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} is starting.", "InactiveUserCleanupBackgroundService");
		int cleanupIntervalInHours = 1;
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				using IServiceScope scope = scopeFactory.CreateScope();
				IInactiveUserCleanupService requiredService = scope.ServiceProvider.GetRequiredService<IInactiveUserCleanupService>();
				await requiredService.CleanupInactiveUsersAsync();
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Exception thrown in {ServiceName}.", "InactiveUserCleanupBackgroundService");
			}
			await Task.Delay(TimeSpan.FromHours(cleanupIntervalInHours), stoppingToken);
		}
		logger.LogInformation("{ServiceName} is stopping.", "InactiveUserCleanupBackgroundService");
	}
}
