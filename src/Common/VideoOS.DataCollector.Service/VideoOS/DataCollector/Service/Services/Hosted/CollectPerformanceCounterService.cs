using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Sample.Events;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class CollectPerformanceCounterService : WaitForNotifyCompleteBase
{
	private readonly IMediator _mediator;

	private readonly ILogger<CollectPerformanceCounterService> _logger;

	public CollectPerformanceCounterService(IMediator mediator, INotifyComplete applicationStartup, ILogger<CollectPerformanceCounterService> logger)
		: base(applicationStartup)
	{
		_mediator = mediator;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await WaitForNotifyComplete(stoppingToken);
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				DateTime currentTime = GetCurrentTime();
				await _mediator.Publish(new CollectDataEvent(currentTime), stoppingToken);
				await Delay(TimeSpan.FromSeconds(1.0), stoppingToken);
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception value)
		{
			_logger.LogError($"{value}");
			Exit();
		}
	}

	public virtual DateTime GetCurrentTime()
	{
		return DateTime.UtcNow;
	}

	public virtual Task Delay(TimeSpan timeSpan, CancellationToken token)
	{
		return Task.Delay(timeSpan, token);
	}

	public virtual void Exit()
	{
		Environment.Exit(1);
	}
}
