using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class SendNotificationCommandHandler : ICommandHandler
{
	private Server _server;

	public SendNotificationCommandHandler(Server server)
	{
		_server = server;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is SendNotificationCommand cmd))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			_server.NotificationManager.SendNotification(cmd);
			responseSink.AddResponse(new SuccessResponse());
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogWarning("Failed to execute SendNotification command. Error: {ExceptionMessage}", ex2.Message);
			ServerLogManager.Instance.AddLogEntry(LogEntryId.SendNotificationError, ex2.ToString());
			responseSink.AddResponse(new FailResponse());
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
