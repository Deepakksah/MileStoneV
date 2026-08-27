using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateRecorderStatusCommandHandler : ICommandHandler
{
	private Server _server;

	public UpdateRecorderStatusCommandHandler(Server server)
	{
		_server = server;
	}

	public void Execute(CommandBase command, object state, IResponseSink reponseSink)
	{
		if (!(command is UpdateRecorderStatusCommand updateRecorderStatusCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		_server.RecorderCommManager.SetRecorderStatus(updateRecorderStatusCommand.ServiceId, updateRecorderStatusCommand.Status);
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
