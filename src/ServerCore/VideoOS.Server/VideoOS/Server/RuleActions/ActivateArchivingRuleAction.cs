using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class ActivateArchivingRuleAction : RuleActionBase
{
	private Guid[] archiveStorageIds;

	public ActivateArchivingRuleAction(IRuleEngine ruleEngine, Guid[] archiveStorageIds)
		: base(ruleEngine)
	{
		this.archiveStorageIds = archiveStorageIds;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		base.RuleEngine.Logger.LogInformation("ActivateArchivingRuleAction, Executing action. ArchiveIds={ArchiveIds}", string.Join(",", archiveStorageIds.Select((Guid g) => g.ToString()).ToArray()));
		if (triggerEvent.IsGlobal)
		{
			Server.Instance.Logger.LogInformation("Skip sending ActivateArchiving command to the recorder because event is global.");
			return;
		}
		ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
		Guid[] array = archiveStorageIds;
		foreach (Guid archiveStorageId in array)
		{
			ActivateArchivingCommand command = new ActivateArchivingCommand(archiveStorageId);
			CommandProcessor.ProcessAsync(command, state, new NullResponseSink());
		}
	}
}
