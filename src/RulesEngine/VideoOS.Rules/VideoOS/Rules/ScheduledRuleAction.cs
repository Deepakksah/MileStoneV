using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Interfaces.Scheduler;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public sealed class ScheduledRuleAction : ISchedulable
{
	private IRuleEngine ruleEngine;

	private ActionTargetCollection expandedActionTargets;

	private IRuleAction action;

	private TriggerEvent triggerEvent;

	public IRuleAction RuleAction => action;

	public TriggerEvent TriggerEvent => triggerEvent;

	public ScheduledRuleAction(IRuleEngine ruleEngine, IRuleAction action, ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		this.ruleEngine = ruleEngine;
		this.expandedActionTargets = expandedActionTargets;
		this.action = action;
		this.triggerEvent = triggerEvent;
	}

	public void ProcessTask(Guid taskId)
	{
		try
		{
			ruleEngine.Logger.LogInformation("Scheduler starting to execute rule action. ActionName: {FullName}", action.GetType().FullName);
			action.Execute(expandedActionTargets, triggerEvent);
			ruleEngine.Logger.LogDebug("Scheduler finished executing rule action. ActionName: {FullName}", action.GetType().FullName);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			ruleEngine.Logger.LogWarning("Error executing scheduled rule action. Error: {Ex}", ex2.Message);
		}
	}
}
