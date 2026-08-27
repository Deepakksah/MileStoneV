using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class OutboxMessageCleanupService(IServiceProvider services, IOptions<OutboxProcessorOptions> options, ILogger<OutboxMessageCleanupService> logger) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} is starting.", "OutboxMessageCleanupService");
		int cleanupIntervalInHours = 1;
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await CleanupOutboxMessagesAsync();
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "Exception thrown in {ServiceName}.", "OutboxMessageCleanupService");
			}
			await Task.Delay(TimeSpan.FromHours(cleanupIntervalInHours), stoppingToken);
		}
		logger.LogInformation("{ServiceName} is stopping.", "OutboxMessageCleanupService");
	}

	private async Task CleanupOutboxMessagesAsync()
	{
		DateTimeOffset cutoffDate = DateTimeOffset.UtcNow.AddDays(-options.Value.RetentionDurationInDays);
		using IServiceScope scope = services.CreateScope();
		IOutboxStore requiredService = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
		int num = await requiredService.DeleteLogsOlderThanAsync(cutoffDate);
		if (num > 0)
		{
			logger.LogInformation("{ServiceName}: {DeletedMessagesCount} outbox messages older than '{cutoffDate}' deleted.", "OutboxMessageCleanupService", num, cutoffDate.ToString());
		}
	}
}
