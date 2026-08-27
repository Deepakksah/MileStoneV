using System;
using VideoOS.Common.Interfaces.Scheduler;

namespace VideoOS.Rules.Interfaces;

public interface IRuleAction
{
	Guid InstanceId { get; set; }

	TimeSpan Delay { get; set; }

	bool HasDelay { get; }

	bool HasNegativeDelay { get; }

	bool HasPositiveDelay { get; }

	bool HandlesAllActionTargets { get; }

	ActionTargetCollection ActionTargets { get; }

	void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent);

	void HandleUnregisterEvent(ActionTargetCollection expandedActionTargets, UnregisterTriggerEvent unregisterTriggerEvent);

	void Schedule(IScheduler scheduler, ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent);
}
