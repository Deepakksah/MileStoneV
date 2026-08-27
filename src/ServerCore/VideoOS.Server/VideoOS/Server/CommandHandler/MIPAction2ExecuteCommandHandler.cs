using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class MIPAction2ExecuteCommandHandler : ICommandHandler
{
	private Server _server;

	public MIPAction2ExecuteCommandHandler(Server server)
	{
		_server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is MIPAction2ExecuteCommand mIPAction2ExecuteCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (mIPAction2ExecuteCommand.TriggerEvent.Definition == null)
		{
			mIPAction2ExecuteCommand.TriggerEvent.Definition = _server.RuleEngine.GetEventTypeDefinition(mIPAction2ExecuteCommand.TriggerEvent.EventTypeId);
		}
		_server.MIPMessageManager.Action2ExecutionRequest(mIPAction2ExecuteCommand.ActionId, mIPAction2ExecuteCommand.ActionName, mIPAction2ExecuteCommand.Targets, mIPAction2ExecuteCommand.Targets2, mIPAction2ExecuteCommand.TriggerEvent);
	}
}
