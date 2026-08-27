using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class NotificationRuleAction : RuleActionBase
{
	private Server _server;

	private Guid _notificationId;

	private Guid _ruleId;

	private Guid[] _deviceIds;

	private Guid[] _deviceGroupIds;

	public override bool HandlesAllActionTargets => true;

	public NotificationRuleAction(IRuleEngine ruleEngine, Server server, Guid notificationId, Guid ruleId, Guid[] deviceIds, Guid[] deviceGroupIds)
		: base(ruleEngine)
	{
		_server = server;
		_notificationId = notificationId;
		_ruleId = ruleId;
		_deviceIds = deviceIds;
		_deviceGroupIds = deviceGroupIds;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		Guid[] sourceIds = _deviceIds;
		if (_deviceIds.Length == 1 && _deviceGroupIds.Length == 0)
		{
			if (_deviceIds[0] == Guid.Empty)
			{
				sourceIds = new Guid[1] { triggerEvent.SourceId };
			}
			else if (_deviceIds[0] == SpecialActionTargets.DevicesFromMetaData && expandedActionTargets != null)
			{
				List<Guid> list = new List<Guid>();
				foreach (ActionTarget expandedActionTarget in expandedActionTargets)
				{
					if (expandedActionTarget.TargetType == ActionTargetType.Device)
					{
						list.AddRange(expandedActionTarget.TargetItems);
					}
				}
				sourceIds = list.ToArray();
			}
		}
		ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
		Guid recorderId = triggerEvent.TargetDeviceIds.FirstOrDefault();
		SendMultiNotificationCommand command = new SendMultiNotificationCommand(_notificationId, _ruleId, triggerEvent.EventTypeId, triggerEvent.SourceId, recorderId, sourceIds, _deviceGroupIds, triggerEvent.Time);
		CommandProcessor.ProcessAsync(command, state, ProcessCommandCallback);
	}

	public void ProcessCommandCallback(CommandBase command, ResponseBase response)
	{
		Server.Instance.Logger.LogDebug("Command processed. Command={Command}; Response={Response}", command.GetType().ToString(), response.GetType().ToString());
	}
}
