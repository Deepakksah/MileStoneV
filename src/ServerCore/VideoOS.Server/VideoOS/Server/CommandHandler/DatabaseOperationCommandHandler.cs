using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class DatabaseOperationCommandHandler : ICommandHandler
{
	private class MessageManagerCallback : IMessageManagerCallback
	{
		private IResponseSink responseSink;

		public MessageManagerCallback(IResponseSink responseSink)
		{
			this.responseSink = responseSink;
		}

		void IMessageManagerCallback.HandleTimeout(CommandBase command)
		{
			responseSink.AddResponse(new TimeoutResponse());
		}

		void IMessageManagerCallback.HandleResponse(CommandBase command, ResponseBase response)
		{
			responseSink.AddResponse(response);
		}

		bool IMessageManagerCallback.IsLastExpectedResponse(CommandBase command, ResponseBase response)
		{
			return true;
		}
	}

	private Server _server;

	public DatabaseOperationCommandHandler(Server server)
	{
		_server = server;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is DatabaseOperationCommand databaseOperationCommand))
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
		if (databaseOperationCommand.RecorderId == Guid.Empty)
		{
			responseSink.AddResponse(new FailResponse());
			return;
		}
		RecordingServer recordingServer = RecordingServer.QueryActiveRecordingServer(databaseOperationCommand.RecorderId);
		if (recordingServer == null)
		{
			Server.Instance.Logger.LogWarning("Could not find recorder with id = {RecorderId}.", databaseOperationCommand.RecorderId);
			responseSink.AddResponse(new FailResponse());
			return;
		}
		if (recordingServer.Type != RecordingServerServiceType.RecordingServer)
		{
			Server.Instance.Logger.LogWarning("Not possible to run database command on failover server. RecorderId = {RecorderId}.", databaseOperationCommand.RecorderId);
			responseSink.AddResponse(new RecorderUnavailableResponse(databaseOperationCommand.RecorderId));
			return;
		}
		if (!_server.RecorderCommManager.IsRecorderAttachedAndConnected(recordingServer.ServiceId))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(databaseOperationCommand.RecorderId));
			return;
		}
		MessageManagerCallback messageManagerCallback = new MessageManagerCallback(responseSink);
		try
		{
			_server.MessageManager.SendCommand(recordingServer.ServiceId, databaseOperationCommand, messageManagerCallback, TimeSpan.FromMinutes(10.0), callContextInfo);
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(databaseOperationCommand.RecorderId));
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(databaseOperationCommand.RecorderId));
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
