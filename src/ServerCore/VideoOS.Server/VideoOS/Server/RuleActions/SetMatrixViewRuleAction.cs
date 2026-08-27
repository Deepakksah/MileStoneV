using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class SetMatrixViewRuleAction : RuleActionBase
{
	private Server _server;

	private Guid _matrixId;

	public override bool HandlesAllActionTargets => true;

	public SetMatrixViewRuleAction(IRuleEngine ruleEngine, Server server, Guid matrixId, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		_server = server;
		_matrixId = matrixId;
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
		List<Guid> list = new List<Guid>();
		ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
		foreach (ActionTarget expandedActionTarget in expandedActionTargets)
		{
			if (expandedActionTarget.TargetType != ActionTargetType.Device)
			{
				continue;
			}
			foreach (Guid targetItem in expandedActionTarget.TargetItems)
			{
				list.Add(targetItem);
			}
		}
		if (list.Count > 0)
		{
			SetMatrixViewCommand command = new SetMatrixViewCommand(_matrixId, list.ToArray());
			CommandProcessor.ProcessAsync(command, state, new NullResponseSink());
		}
	}
}
