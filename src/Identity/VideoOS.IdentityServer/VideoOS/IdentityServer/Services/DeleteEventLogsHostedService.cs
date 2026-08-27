using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VideoOS.IdentityServer.Helper;
using VideoOS.IdentityServer.Repositories.Interfaces;
using VideoOS.Utilities.Timers;

namespace VideoOS.IdentityServer.Services;

public class DeleteEventLogsHostedService : IHostedService, IDisposable
{
	private readonly ILogger<DeleteEventLogsHostedService> _logger;

	private readonly VideoOS.Utilities.Timers.ITimer _timer;

	private readonly IDateTimeHelper _dateTimeHelper;

	private readonly IServiceProvider _services;

	public DeleteEventLogsHostedService(IServiceProvider services, VideoOS.Utilities.Timers.ITimer timer, IDateTimeHelper dateTimeHelper, ILogger<DeleteEventLogsHostedService> logger)
	{
		_services = services;
		_dateTimeHelper = dateTimeHelper;
		_timer = timer;
		_timer.Elapsed += DoWork;
		_logger = logger;
	}

	public Task StartAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("DeleteEventLogsService: Running.");
		_timer.StartTimer(TimeSpan.Zero, TimeSpan.FromHours(1.0));
		return Task.CompletedTask;
	}

	private void DoWork(object source, EventArgs e)
	{
		DateTimeOffset timeOffset = _dateTimeHelper.UtcNow.AddDays(-1.0);
		try
		{
			using (IServiceScope serviceScope = _services.CreateScope())
			{
				IEventLogRepository requiredService = serviceScope.ServiceProvider.GetRequiredService<IEventLogRepository>();
				requiredService.DeleteLogsOlderThan(timeOffset);
				requiredService.SaveChangesAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
			}
			_logger.LogInformation("DeleteEventLogsService: Event logs older than '{timeOffset}' deleted.", timeOffset.ToString());
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "DeleteEventLogsHostedService threw exception");
		}
	}

	public Task StopAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("DeleteEventLogsService: Stopping.");
		_timer.StopTimer();
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
}
