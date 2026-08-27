using System;
using VideoOS.Common.Interfaces.Scheduler;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public abstract class RuleActionBase : IRuleAction
{
	private Guid instanceId;

	private TimeSpan delay;

	private ActionTargetCollection actionTargetCollection = new ActionTargetCollection();

	protected IRuleEngine RuleEngine { get; private set; }

	public Guid InstanceId
	{
		get
		{
			return instanceId;
		}
		set
		{
			instanceId = value;
		}
	}

	public TimeSpan Delay
	{
		get
		{
			return delay;
		}
		set
		{
			delay = value;
		}
	}

	public bool HasDelay => delay != TimeSpan.Zero;

	public bool HasNegativeDelay => delay < TimeSpan.Zero;

	public bool HasPositiveDelay => delay > TimeSpan.Zero;

	public virtual bool HandlesAllActionTargets => false;

	public ActionTargetCollection ActionTargets => actionTargetCollection;

	public RuleActionBase(IRuleEngine ruleEngine)
	{
		RuleEngine = ruleEngine;
		delay = TimeSpan.Zero;
	}

	public RuleActionBase(IRuleEngine ruleEngine, TimeSpan delay)
	{
		RuleEngine = ruleEngine;
		this.delay = delay;
	}

	public abstract void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent);

	public virtual void HandleUnregisterEvent(ActionTargetCollection expandedActionTargets, UnregisterTriggerEvent unregisterTriggerEvent)
	{
	}

	public virtual void Schedule(IScheduler scheduler, ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (scheduler == null)
		{
			throw new ArgumentNullException("scheduler");
		}
		if (expandedActionTargets == null)
		{
			throw new ArgumentNullException("expandedActionTargets");
		}
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		TimeSpan ts = DateTime.UtcNow.Subtract(triggerEvent.Time);
		TimeSpan timeSpan = Delay.Subtract(ts);
		ScheduleTask(scheduler, new ScheduledRuleAction(RuleEngine, this, expandedActionTargets, triggerEvent), timeSpan);
	}

	private static void ScheduleTask(IScheduler scheduler, ISchedulable task, TimeSpan delay)
	{
		if (delay < TimeSpan.Zero)
		{
			scheduler.ScheduleTask(task, TimeSpan.Zero);
		}
		else
		{
			scheduler.ScheduleTask(task, delay);
		}
	}
}
