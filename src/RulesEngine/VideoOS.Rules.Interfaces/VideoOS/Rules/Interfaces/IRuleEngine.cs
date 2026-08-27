using System;
using Microsoft.Extensions.Logging;
using VideoOS.Management.Common;

namespace VideoOS.Rules.Interfaces;

public interface IRuleEngine
{
	ILogger Logger { get; }

	IGroupConfiguration GroupConfiguration { get; }

	IRuleVariableManager RuleVariableManager { get; }

	bool IsFailover { get; }

	ITimeProfileConfiguration TimeProfileConfiguration { get; }

	void AddRuleActionExecutionRequest(IRuleActionExecutionRequest ruleActionExecutionRequest);

	void AddTriggerEvent(TriggerEvent triggerEvent);

	void AddUnregisterTriggerEvent(UnregisterTriggerEvent unregisterTriggerEvent);

	bool IsUsedInRule(TriggerEventBase triggerEvent);

	EventDefinition GetEventTypeDefinition(Guid eventId);

	void ConfigureRules(IRuleConfiguration ruleConfiguration);
}
