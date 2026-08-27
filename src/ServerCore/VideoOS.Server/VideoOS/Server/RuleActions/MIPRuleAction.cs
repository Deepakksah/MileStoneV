using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class MIPRuleAction : RuleActionBase
{
	private Guid _actionId;

	private string _actionName;

	public MIPRuleAction(IRuleEngine ruleEngine, Guid actionId, string actionName, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		_actionId = actionId;
		_actionName = actionName;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (expandedActionTargets == null)
		{
			throw new ArgumentNullException("expandedActionTargets");
		}
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		foreach (ActionTarget expandedActionTarget in expandedActionTargets)
		{
			if (expandedActionTarget.TargetType == ActionTargetType.MIP)
			{
				ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
				MIPActionExecuteCommand command = new MIPActionExecuteCommand(_actionId, _actionName, expandedActionTarget.TargetItems.ToArray(), triggerEvent);
				CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
			}
		}
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		Server.Instance.Logger.LogDebug("Command processed. Command={Command}; Response={Response}", command.GetType(), response.GetType());
	}
}
