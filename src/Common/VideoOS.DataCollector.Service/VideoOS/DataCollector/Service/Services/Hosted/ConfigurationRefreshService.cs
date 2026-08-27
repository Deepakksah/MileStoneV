using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Queries.Configuration;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class ConfigurationRefreshService : WaitForNotifyCompleteBase
{
	private readonly IConfigurationCache _configurationCache;

	private readonly IMediator _mediator;

	private readonly ILogger<ConfigurationRefreshService> _logger;

	private readonly PerformanceCounterOptions _options;

	public ConfigurationRefreshService(IConfigurationCache configurationCache, IMediator mediator, INotifyComplete applicationStartup, ILogger<ConfigurationRefreshService> logger, IOptions<PerformanceCounterOptions> options)
		: base(applicationStartup)
	{
		_configurationCache = configurationCache;
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
				IMediator mediator = _mediator;
				Result<DataCollectorConfiguration> result = await mediator.Send((IRequest<Result<DataCollectorConfiguration>>)new GetConfigurationServerQuery(await _configurationCache.GetOrAddDataCollectorIdAsync(stoppingToken)), stoppingToken);
				if (!result.IsError)
				{
					if (result.Value.ConfiguredCounters.Length != 0)
					{
						await _mediator.Publish(new RefreshConfigurationEvent(result.Value), stoppingToken);
					}
					else
					{
						_logger.LogError("Empty configuration received from server");
					}
				}
				await TaskDelay(TimeSpan.FromSeconds(_options.ConfigurationRefreshIntervalInSec), stoppingToken);
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

	public virtual Task TaskDelay(TimeSpan delay, CancellationToken stoppingToken)
	{
		return Task.Delay(delay, stoppingToken);
	}

	public virtual void Exit()
	{
		Environment.Exit(1);
	}
}
