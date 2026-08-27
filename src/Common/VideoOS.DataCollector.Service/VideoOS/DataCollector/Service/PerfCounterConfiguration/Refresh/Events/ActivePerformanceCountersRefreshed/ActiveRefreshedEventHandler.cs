using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Queries.GetConfiguration;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.Events.ActivePerformanceCountersRefreshed;

public class ActiveRefreshedEventHandler : INotificationHandler<ActiveRefreshedEvent>
{
	private readonly ILogger<ActiveRefreshedEventHandler> _logger;

	private readonly IConfigurationCache _configurationCache;

	private readonly IMediator _mediator;

	public ActiveRefreshedEventHandler(ILogger<ActiveRefreshedEventHandler> logger, IConfigurationCache configurationCache, IMediator mediator)
	{
		_logger = logger;
		_configurationCache = configurationCache;
		_mediator = mediator;
	}

	public async Task Handle(ActiveRefreshedEvent notification, CancellationToken cancellationToken)
	{
		GetRuleConfigurationServerQuery request = new GetRuleConfigurationServerQuery(await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken));
		await _mediator.Send((IRequest<Result<Unit>>)request, cancellationToken);
		_logger.LogInformation(string.Format(CultureInfo.InvariantCulture, "Successfully applied new configuration; active counters: {0}", notification.Item.Count));
	}
}
