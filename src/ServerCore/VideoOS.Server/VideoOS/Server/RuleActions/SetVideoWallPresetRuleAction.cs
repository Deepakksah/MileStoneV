using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class SetVideoWallPresetRuleAction : RuleActionBase
{
	private Guid videoWallId;

	private Guid presetId;

	public SetVideoWallPresetRuleAction(IRuleEngine ruleEngine, Guid videoWallId, Guid presetId, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		this.videoWallId = videoWallId;
		this.presetId = presetId;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
		VideoWallCommand command = new VideoWallCommand(videoWallId, presetId);
		CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		VideoWallCommand videoWallCommand = (VideoWallCommand)command;
		Server.Instance.Logger.LogDebug("Command processed. Command={Command} (operation={Operation}); Response={Response}", videoWallCommand.GetType(), videoWallCommand.OperationType, response.GetType());
	}
}
