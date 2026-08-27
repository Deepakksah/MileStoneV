using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class ReportMediaCommittedToDatabaseCommandHandler : ICommandHandler
{
	private Server _server;

	public ReportMediaCommittedToDatabaseCommandHandler(Server server)
	{
		_server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ReportMediaCommittedToDatabaseCommand reportMediaCommittedToDatabaseCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		_server.ServiceChannelManager.SendRecordingsChangedMessage(reportMediaCommittedToDatabaseCommand.DeviceId, reportMediaCommittedToDatabaseCommand.StartTime, reportMediaCommittedToDatabaseCommand.EndTime);
	}
}
