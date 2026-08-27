using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Refresh;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Queries.GetConfiguration;

public class GetRuleConfigurationServerQueryHandler : IRequestHandler<GetRuleConfigurationServerQuery, Result<Unit>>
{
	private readonly IDataCollection _dataCollectorServiceProxy;

	private readonly IMediator _mediator;

	private readonly ILogger<GetRuleConfigurationServerQueryHandler> _logger;

	public GetRuleConfigurationServerQueryHandler(IDataCollection dataCollectorServiceProxy, IMediator mediator, ILogger<GetRuleConfigurationServerQueryHandler> logger)
	{
		_dataCollectorServiceProxy = dataCollectorServiceProxy;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task<Result<Unit>> Handle(GetRuleConfigurationServerQuery request, CancellationToken cancellationToken)
	{
		CounterStateRulesConfiguration counterStateRulesConfiguration = _dataCollectorServiceProxy.GetCounterStateRulesConfiguration(request.DataCollectorId);
		if (counterStateRulesConfiguration.CounterStateRules.Length != 0)
		{
			await _mediator.Publish(new RefreshRuleConfigurationEvent(counterStateRulesConfiguration), cancellationToken);
		}
		else
		{
			_logger.LogError("Empty rule configuration received from server");
		}
		return default(Unit);
	}
}
