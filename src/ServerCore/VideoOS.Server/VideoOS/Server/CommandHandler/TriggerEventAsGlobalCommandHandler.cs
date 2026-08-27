using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class TriggerEventAsGlobalCommandHandler : ICommandHandler
{
	private Server server;

	public TriggerEventAsGlobalCommandHandler(Server server)
	{
		this.server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is TriggerEventAsGlobalCommand triggerEventAsGlobalCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		TriggerEvent triggerEvent = triggerEventAsGlobalCommand.TriggerEvent.Clone();
		triggerEvent.IsGlobal = true;
		server.RuleEngine.AddTriggerEvent(triggerEvent);
		RecorderConfigData[] array = RecorderConfigData.QueryRecorders(server.DataComponentFactory);
		bool usedInRules = server.RuleEngine.IsUsedInRule(triggerEventAsGlobalCommand.TriggerEvent);
		RecorderConfigData[] array2 = array;
		foreach (RecorderConfigData recorderConfigData in array2)
		{
			if (recorderConfigData.Id == triggerEventAsGlobalCommand.SourceId || !recorderConfigData.Enabled)
			{
				continue;
			}
			try
			{
				server.RecorderCommManager.TriggerEventAsGlobalAsync(recorderConfigData.Id, triggerEventAsGlobalCommand.SourceId, triggerEventAsGlobalCommand.TriggerEvent, callContextInfo, usedInRules);
			}
			catch (RecorderNotFoundException ex)
			{
				string text = "Unknown";
				string text2 = Server.Instance.ConfigurationLookup.FindEventDisplayNameFromEventTypeID(triggerEventAsGlobalCommand.TriggerEvent.EventTypeId, string.Empty);
				if (!string.IsNullOrEmpty(text2))
				{
					text = text2;
				}
				ServerLogManager.Instance.AddLogEntry(LogEntryId.FailedToDistributeGlobalizedEvents, text, ex.Message);
			}
		}
		responseSink.AddResponse(new SuccessResponse());
	}
}
