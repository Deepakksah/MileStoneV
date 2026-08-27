using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class MIPRuleAction2 : RuleActionBase
{
	private Guid _actionId;

	private string _actionName;

	private Guid[] _param2;

	private Guid[] _paramGroups2;

	public MIPRuleAction2(IRuleEngine ruleEngine, Guid actionId, string actionName, Guid[] param2, Guid[] paramGroups2, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		_actionId = actionId;
		_actionName = actionName;
		_param2 = param2;
		_paramGroups2 = paramGroups2;
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
				HashSet<Guid> hashSet = new HashSet<Guid>(_param2);
				Guid[] paramGroups = _paramGroups2;
				foreach (Guid groupId in paramGroups)
				{
					hashSet.UnionWith(base.RuleEngine.GroupConfiguration.FindAllDescendantMembersByGroup(groupId));
				}
				MIPAction2ExecuteCommand command = new MIPAction2ExecuteCommand(_actionId, _actionName, expandedActionTarget.TargetItems.ToArray(), hashSet.ToArray(), triggerEvent);
				CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
			}
		}
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		Server.Instance.Logger.LogDebug("Command processed. Command={Command}; Response={Response}", command.GetType(), response.GetType());
	}
}
