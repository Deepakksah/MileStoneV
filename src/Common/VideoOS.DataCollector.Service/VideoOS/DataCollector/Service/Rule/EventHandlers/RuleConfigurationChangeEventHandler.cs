using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Rule.Events;
using VideoOS.DataCollector.Service.Rule.Refresh;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Refresh.EventHandlers.RefreshRules;

namespace VideoOS.DataCollector.Service.Rule.EventHandlers;

public class RuleConfigurationChangeEventHandler : INotificationHandler<RuleConfigurationChangedEvent>
{
	private readonly IRuleRepository _ruleRepository;

	private readonly IMediator _mediator;

	public RuleConfigurationChangeEventHandler(IRuleRepository ruleRepository, IMediator mediator)
	{
		_ruleRepository = ruleRepository;
		_mediator = mediator;
	}

	public async Task Handle(RuleConfigurationChangedEvent notification, CancellationToken cancellationToken)
	{
		List<RefreshRuleConfigurationEventHandler.RuleConfiguration> list = _ruleRepository.RuleConfigurations.ToList();
		CounterStateRulesConfigurationUpdate[] ruleConfigUpdates = notification.RuleConfigUpdates;
		foreach (CounterStateRulesConfigurationUpdate update in ruleConfigUpdates)
		{
			bool updateIsDefualtRule = string.IsNullOrEmpty(update.RuleConfig.CounterPath);
			RefreshRuleConfigurationEventHandler.RuleConfiguration ruleConfiguration = list.FirstOrDefault((RefreshRuleConfigurationEventHandler.RuleConfiguration r) => r.Rule.CounterRuleId == update.RuleConfig.CounterRuleId && ((!updateIsDefualtRule) ? (!r.IsDefault && r.Rule.CounterPath == update.RuleConfig.CounterPath) : r.IsDefault));
			if (ruleConfiguration != null)
			{
				list.Remove(ruleConfiguration);
			}
			if (!update.Deleted)
			{
				list.Add(new RefreshRuleConfigurationEventHandler.RuleConfiguration(update.RuleConfig, updateIsDefualtRule));
			}
		}
		CounterStateRulesConfiguration item = new CounterStateRulesConfiguration(list.Select((RefreshRuleConfigurationEventHandler.RuleConfiguration r) => r.Rule).ToArray());
		await _mediator.Publish(new RefreshRuleConfigurationEvent(item), cancellationToken);
	}
}
