using System;

namespace VideoOS.Rules.Interfaces;

public interface IRuleProcessor
{
	event EventHandler<EventProcessingEventArgs> EventProcessing;

	event EventHandler<EventProcessingEventArgs> TriggerEventAsGlobal;

	event EventHandler<EventProcessingEventArgs> EventProcessed;

	event EventHandler<RuleProcessingEventArgs> RuleProcessing;

	event EventHandler<RuleProcessingErrorEventArgs> RuleProcessingError;

	event EventHandler<RuleProcessingEventArgs> RuleProcessed;

	event EventHandler<RuleActionProcessingEventArgs> RuleActionProcessing;

	event EventHandler<RuleActionProcessingEventArgs> RuleActionProcessed;

	event EventHandler<RuleEventArgs> RuleClearStateExternal;

	void Start();

	void Stop();

	void EnableEventQueueing();

	void UpdateGroupConfiguration(IGroupConfigurationHelper groupHelper);

	IRule[] GetRulesByGroupId(Guid groupId);

	ISubRule[] GetSubRules(TriggerEventBase triggerEventBase);

	void AddRule(IRule rule);

	IRule RemoveRule(Guid ruleId);

	void ModifyRule(IRule rule);

	void AddUnregisterTriggerEvent(UnregisterTriggerEvent unregisterTriggerEvent);

	void AddTriggerEvent(TriggerEvent triggerEvent);

	void AddRuleActionExecutionRequest(IRuleActionExecutionRequest ruleActionExecutionRequest);

	void AddTargetChangeEvent(TargetChangeEvent targetChangeEvent);

	void AddClearStateExternalEvent(Guid ruleId);
}
