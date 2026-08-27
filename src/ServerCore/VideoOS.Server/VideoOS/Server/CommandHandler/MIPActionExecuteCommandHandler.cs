using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class MIPActionExecuteCommandHandler : ICommandHandler
{
	private Server _server;

	public MIPActionExecuteCommandHandler(Server server)
	{
		_server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is MIPActionExecuteCommand mIPActionExecuteCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (mIPActionExecuteCommand.TriggerEvent.Definition == null)
		{
			mIPActionExecuteCommand.TriggerEvent.Definition = _server.RuleEngine.GetEventTypeDefinition(mIPActionExecuteCommand.TriggerEvent.EventTypeId);
		}
		_server.MIPMessageManager.ActionExecutionRequest(mIPActionExecuteCommand.ActionId, mIPActionExecuteCommand.ActionName, mIPActionExecuteCommand.Targets, mIPActionExecuteCommand.TriggerEvent);
	}
}
