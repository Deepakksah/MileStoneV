using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class RemoveMonitorCamerasRuleAction : RuleActionBase
{
	private Guid videoWallId;

	private Guid monitorId;

	private Guid[] deviceIds;

	public override bool HandlesAllActionTargets => true;

	public RemoveMonitorCamerasRuleAction(IRuleEngine ruleEngine, Guid videoWallId, Guid monitorId, Guid[] siteDeviceIds, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		this.videoWallId = videoWallId;
		this.monitorId = monitorId;
		deviceIds = siteDeviceIds;
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
		List<Guid> list = new List<Guid>(deviceIds);
		foreach (ActionTarget expandedActionTarget in expandedActionTargets)
		{
			if (expandedActionTarget.TargetType == ActionTargetType.Device)
			{
				list.AddRange(expandedActionTarget.TargetItems);
			}
		}
		if (list.Count > 0)
		{
			ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
			VideoWallCommand command = new VideoWallCommand(videoWallId, monitorId, list.ToArray(), new Guid[0]);
			CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
		}
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		VideoWallCommand videoWallCommand = (VideoWallCommand)command;
		Server.Instance.Logger.LogDebug("Command processed. Command={Command} (operation={Operation}); Response={Response}", videoWallCommand.GetType(), videoWallCommand.OperationType, response.GetType());
	}
}
