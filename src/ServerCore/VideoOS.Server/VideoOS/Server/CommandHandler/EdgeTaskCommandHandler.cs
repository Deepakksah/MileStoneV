using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.ServiceChannel;

namespace VideoOS.Server.CommandHandler;

public sealed class EdgeTaskCommandHandler : ICommandHandler
{
	private Server _server;

	public EdgeTaskCommandHandler(Server server)
	{
		_server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is EdgeTaskCommandBase edgeTaskCommandBase))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (edgeTaskCommandBase.TaskType == EdgeTaskType.StorageRetrieval)
		{
			_server.ServiceChannelManager.SendRetrievalTaskChangedMessage(edgeTaskCommandBase.TaskId, EventAction.Updated);
		}
	}
}
