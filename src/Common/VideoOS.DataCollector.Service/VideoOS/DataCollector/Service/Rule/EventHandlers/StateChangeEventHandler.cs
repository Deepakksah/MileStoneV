using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Rule.Entities.Rules;
using VideoOS.DataCollector.Service.Rule.Entities.State;
using VideoOS.DataCollector.Service.Rule.Events;
using VideoOS.DataCollector.Service.Sample.Services.Rule;

namespace VideoOS.DataCollector.Service.Rule.EventHandlers;

public class StateChangeEventHandler : INotificationHandler<StateChangeEvent>
{
	private readonly IRuleStateChangeChannel _channel;

	private readonly IRuleRepository _ruleRepository;

	private readonly IConfigurationCache _configurationCache;

	public StateChangeEventHandler(IRuleStateChangeChannel channel, IRuleRepository ruleRepository, IConfigurationCache configurationCache)
	{
		_channel = channel;
		_ruleRepository = ruleRepository;
		_configurationCache = configurationCache;
	}

	public async Task Handle(StateChangeEvent notification, CancellationToken cancellationToken)
	{
		if (_ruleRepository.MultiRules.TryGetValue(notification.StateChange.CounterRuleId, out IMultiCounterRule rule))
		{
			(bool Changed, StateChange? StateChange) result = await rule.SampleStateChange(notification.StateChange);
			if (result.Changed && result.StateChange != null)
			{
				StateChangeData counterStateChangeData = new StateChangeData(await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken), rule.RuleId, rule.CounterPath, (int)result.StateChange.CounterState, result.StateChange.TriggerValue);
				_channel.TryWrite(counterStateChangeData);
			}
		}
		else
		{
			_channel.TryWrite(notification.StateChange);
		}
	}
}
