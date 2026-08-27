using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Common.Hosting;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Queries.GetConfiguration;

namespace VideoOS.DataCollector.Service.Services.Hosted;

public class RuleConfigurationRefreshService : WaitForNotifyCompleteBase
{
	private readonly IConfigurationCache _configurationCache;

	private readonly IMediator _mediator;

	private readonly ILogger<RuleConfigurationRefreshService> _logger;

	private readonly RuleOptions _options;

	public RuleConfigurationRefreshService(IConfigurationCache configurationCache, IMediator mediator, INotifyComplete applicationStartup, ILogger<RuleConfigurationRefreshService> logger, IOptions<RuleOptions> options)
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
				GetRuleConfigurationServerQuery request = new GetRuleConfigurationServerQuery(await _configurationCache.GetOrAddDataCollectorIdAsync(stoppingToken));
				await _mediator.Send((IRequest<Result<Unit>>)request, stoppingToken);
				await Delay(TimeSpan.FromSeconds(_options.ConfigurationRefreshIntervalInSec), stoppingToken);
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

	public virtual Task Delay(TimeSpan timeSpan, CancellationToken token)
	{
		return Task.Delay(timeSpan, token);
	}

	public virtual void Exit()
	{
		Environment.Exit(1);
	}
}
