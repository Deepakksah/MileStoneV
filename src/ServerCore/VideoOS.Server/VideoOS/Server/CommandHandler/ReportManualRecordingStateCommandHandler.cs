using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class ReportManualRecordingStateCommandHandler : ICommandHandler
{
	private Server _server;

	public ReportManualRecordingStateCommandHandler(Server server)
	{
		_server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ReportManualRecordingStateCommand reportManualRecordingStateCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		_server.ServiceChannelManager.SendManualRecordingStateMessage(reportManualRecordingStateCommand.DeviceId, reportManualRecordingStateCommand.IsManualRecording);
	}
}
