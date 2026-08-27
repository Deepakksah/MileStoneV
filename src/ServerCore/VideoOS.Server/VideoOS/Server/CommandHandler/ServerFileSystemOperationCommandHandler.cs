using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class ServerFileSystemOperationCommandHandler : ICommandHandler
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

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ServerFileSystemOperationCommand serverFileSystemOperationCommand))
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
		switch (serverFileSystemOperationCommand.ServerType)
		{
		case ServerFileSystemServerType.RecordingServer:
		{
			RecordingServer recordingServer = RecordingServer.QueryActiveRecordingServer(serverFileSystemOperationCommand.ServerId);
			if (recordingServer == null)
			{
				Server.Instance.Logger.LogWarning("Could not find recorder with id = {ServerId}.", serverFileSystemOperationCommand.ServerId);
				responseSink.AddResponse(new RecorderNotFoundResponse(serverFileSystemOperationCommand.ServerId));
				break;
			}
			if (recordingServer.Type != RecordingServerServiceType.RecordingServer)
			{
				Server.Instance.Logger.LogWarning("Not possible to run a server file system operation on a failover server. RecorderId = {ServerId}.", serverFileSystemOperationCommand.ServerId);
				responseSink.AddResponse(new RecorderUnavailableResponse(serverFileSystemOperationCommand.ServerId));
				break;
			}
			if (!Server.Instance.RecorderCommManager.IsRecorderAttachedAndConnected(recordingServer.ServiceId))
			{
				responseSink.AddResponse(new RecorderUnavailableResponse(serverFileSystemOperationCommand.ServerId));
				break;
			}
			MessageManagerCallback messageManagerCallback = new MessageManagerCallback(responseSink);
			try
			{
				Server.Instance.MessageManager.SendCommand(recordingServer.ServiceId, serverFileSystemOperationCommand, messageManagerCallback, TimeSpan.FromSeconds(30.0), callContextInfo);
				break;
			}
			catch (RecorderNotFoundException)
			{
				responseSink.AddResponse(new RecorderUnavailableResponse(serverFileSystemOperationCommand.ServerId));
				break;
			}
			catch (NotSupportedByLegacyRecorderException)
			{
				responseSink.AddResponse(new RecorderNotSupportedResponse(serverFileSystemOperationCommand.ServerId));
				break;
			}
		}
		case ServerFileSystemServerType.ManagementServer:
			responseSink.AddResponse(new FailResponse());
			break;
		default:
			responseSink.AddResponse(new FailResponse());
			break;
		}
	}
}
