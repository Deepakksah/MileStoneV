using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Sample.Events;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class SampleRulesService : WaitForNotifyCompleteBase
{
	private readonly IMediator _mediator;

	private readonly ILogger<SampleRulesService> _logger;

	private readonly RuleOptions _options;

	public SampleRulesService(IMediator mediator, INotifyComplete applicationStartup, ILogger<SampleRulesService> logger, IOptions<RuleOptions> options)
		: base(applicationStartup)
	{
		_mediator = mediator;
		_logger = logger;
		_options = options.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await WaitForNotifyComplete(stoppingToken);
		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				DateTime currentTime = GetCurrentTime();
				await _mediator.Publish(new SampleRulesEvent(currentTime), stoppingToken);
				await Delay(TimeSpan.FromMilliseconds(_options.CounterStateMinSampleIntervalMS), stoppingToken);
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
