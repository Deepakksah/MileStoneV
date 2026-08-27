using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class ActivateUserDefinedEventOnRemoteSiteRuleAction : RuleActionBase
{
	private Guid siteId;

	private string userDefinedEventName;

	public ActivateUserDefinedEventOnRemoteSiteRuleAction(IRuleEngine ruleEngine, Guid siteId, string userDefinedEventName)
		: base(ruleEngine)
	{
		this.siteId = siteId;
		this.userDefinedEventName = userDefinedEventName;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
		ActivateUserDefinedEventOnRemoteSiteCommand command = new ActivateUserDefinedEventOnRemoteSiteCommand(siteId, userDefinedEventName);
		CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
		base.RuleEngine.Logger.LogInformation("ActivateUserDefinedEventOnRemoteSiteRuleAction, Executing action. siteId={SiteId}, userdefinedevent={UserDefinedEventName}", siteId.ToString(), userDefinedEventName);
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		Server.Instance.Logger.LogDebug("Command processed. Command={Command}; Response={Response}", command.GetType(), response.GetType());
	}
}
