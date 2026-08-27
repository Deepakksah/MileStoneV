using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class SetMonitorTextRuleAction : RuleActionBase
{
	private Guid _videoWallId;

	private Guid _monitorId;

	private Guid _layoutId;

	private int _viewPosition;

	private string _textToDisplay;

	public SetMonitorTextRuleAction(IRuleEngine ruleEngine, Guid videoWallId, Guid monitorId, Guid layoutId, int viewPosition, string textToDisplay, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		_videoWallId = videoWallId;
		_monitorId = monitorId;
		_layoutId = layoutId;
		_textToDisplay = textToDisplay;
		_viewPosition = viewPosition;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		CheckArgumentNotNull(expandedActionTargets, "expandedActionTargets");
		CheckArgumentNotNull(triggerEvent, "triggerEvent");
		ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
		VideoWallCommand command = new VideoWallCommand(_videoWallId, _monitorId, _textToDisplay, _layoutId, _viewPosition);
		CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
	}

	private static void CheckArgumentNotNull(object arg, string argName)
	{
		if (arg == null)
		{
			throw new ArgumentNullException(argName);
		}
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		VideoWallCommand videoWallCommand = (VideoWallCommand)command;
		Server.Instance.Logger.LogDebug("Command processed. Command={Command} (operation={Operation}); Response={Response}", videoWallCommand.GetType(), videoWallCommand.OperationType, response.GetType());
	}
}
