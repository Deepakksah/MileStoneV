using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class SynchronizeDevicePresetsCommandHandler : ICommandHandler
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

	private Server server;

	public SynchronizeDevicePresetsCommandHandler(Server server)
	{
		this.server = server;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is SynchronizeDevicePresetsCommand synchronizeDevicePresetsCommand))
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
		RecordingServer recordingServer = RecordingServer.QueryActiveRecordingServer(synchronizeDevicePresetsCommand.RecorderId);
		if (recordingServer == null)
		{
			Server.Instance.Logger.LogWarning("Could not find recorder with id = {RecorderId}.", synchronizeDevicePresetsCommand.RecorderId);
			responseSink.AddResponse(new RecorderNotFoundResponse(synchronizeDevicePresetsCommand.RecorderId));
			return;
		}
		if (!server.RecorderCommManager.IsRecorderAttachedAndConnected(recordingServer.ServiceId))
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(synchronizeDevicePresetsCommand.RecorderId));
			return;
		}
		MessageManagerCallback messageManagerCallback = new MessageManagerCallback(responseSink);
		try
		{
			server.MessageManager.SendCommand(recordingServer.ServiceId, synchronizeDevicePresetsCommand, messageManagerCallback, TimeSpan.FromMinutes(1.0), callContextInfo);
		}
		catch (RecorderNotFoundException)
		{
			responseSink.AddResponse(new RecorderUnavailableResponse(synchronizeDevicePresetsCommand.RecorderId));
		}
		catch (NotSupportedByLegacyRecorderException)
		{
			responseSink.AddResponse(new RecorderNotSupportedResponse(synchronizeDevicePresetsCommand.RecorderId));
		}
	}
}
